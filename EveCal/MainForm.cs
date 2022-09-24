namespace EveCal
{
    using EveCal.BPs;
    using System.Web.Http;
    using System.Web.Http.Results;
    using HaulDetail = Dictionary<Tuple<FacilityType, FacilityType>, Dictionary<string, int>>;
    public partial class MainForm : Form
    {
        Tuple<List<ItemWorkDetail>, HaulDetail> currPlan;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Create groups

            allBPList.Clear();
            Dictionary<FacilityType, ListViewGroup> gr_map = new Dictionary<FacilityType, ListViewGroup>();
            foreach(FacilityType facilityType in Enum.GetValues(typeof(FacilityType)))
            {
                gr_map.Add(facilityType, new ListViewGroup(facilityType.ToString()));
                allBPList.Groups.Add(gr_map[facilityType]);
            }

            Dictionary<string, BP> all = Loader.GetAllBP();
            foreach(string key in all.Keys)
            {
                ListViewItem item = new ListViewItem(key);
                item.Group = gr_map[all[key].MakeAt()];
                allBPList.Items.Add(item);
            }

        }

        private void AssetButton_Click(object sender, EventArgs e)
        {
            AssetViewer assetView = new AssetViewer();
            assetView.Show();
        }

        private void MakePlanButton_Click(object sender, EventArgs e)
        {

            string[] lines = OutputText.Text.Split('\n');

            BuildPlan plan = new BuildPlan();
            foreach (string line in lines)
            {
                string[] part = line.Split('\t');
                if(part.Length < 2)
                {
                    plan.Add(part[0].Trim(), 1);
                } else
                {
                    plan.Add(part[0].Trim(), int.Parse(part[1]));
                }
            }

            currPlan = plan.MakePlan(true);
            RunList.Items.Clear();
            RunList.Groups.Clear();
            BuyList.Items.Clear();
            HaulList.Items.Clear();
            HaulList.Groups.Clear();
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun == 0 && work.amount != 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.amount);
                    BuyList.Items.Add(item);
                }
            }
            ShowRunPlan();
            ShowHaulPlan();
            
            FileStream fs;
            if (File.Exists("BuildPlan.txt"))
            {
                fs = File.Open("BuildPlan.txt", FileMode.Truncate);
            } else
            {
                fs = File.Open("BuildPlan.txt", FileMode.OpenOrCreate);
            }
            StreamWriter writer = new StreamWriter(fs);
            writer.Write("Run plan:\n\n");
            writer.Write(GenerateRunPlan());
            writer.Write("\n\nBuy plan:\n\n");
            writer.Write(GenerateBuyPlan());
            writer.Write("\n\nHaul plan:\n\n");
            writer.Write(GenerateHaulPlan());
            writer.Close();
            fs.Close();
        }

        private void ShowHaulPlan()
        {
            List<ListViewGroup> list = new List<ListViewGroup>();
            foreach (var key in currPlan.Item2.Keys)
            {
                FacilityType haulFrom = key.Item1, haulTo = key.Item2;
                ListViewGroup gr = new ListViewGroup("" + Storage.GetFacilityName(haulFrom) + " \n-> \n" + Storage.GetFacilityName(haulTo));
                list.Add(gr);
                foreach (string item in currPlan.Item2[key].Keys)
                {
                    ListViewItem listItem = new ListViewItem(item);
                    listItem.SubItems.Add(" x ");
                    listItem.SubItems.Add("" + currPlan.Item2[key][item]);
                    listItem.Group = gr;
                    HaulList.Items.Add(listItem);
                }
            }
            list = list.OrderBy(o => o.Header).ToList();
            foreach(ListViewGroup gr in list)
            {
                HaulList.Groups.Add(gr);
            }
        }

        private void ShowRunPlan()
        {
            Dictionary<FacilityType, ListViewGroup> gr_map = new Dictionary<FacilityType, ListViewGroup>();
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun != 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.jobRun);
                    BP bp = Loader.Get(work.name);
                    if (!gr_map.ContainsKey(bp.MakeAt()))
                    {
                        ListViewGroup group = new ListViewGroup(bp.MakeAt().ToString());
                        gr_map.Add(bp.MakeAt(), group);
                        RunList.Groups.Add(group);
                    }
                    item.Group = gr_map[bp.MakeAt()];
                    RunList.Items.Add(item);
                }
            }

            ListViewGroup cgroup = new ListViewGroup("Copy");
            RunList.Groups.Add(cgroup);

            Dictionary<FacilityType, Dictionary<string, int>> all_bpc = Storage.GetAllBPC();
            Dictionary<string, int> copying = Storage.GetRunningJob(ActivityType.Copying);

            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun != 0)
                {
                    BP bp = Loader.Get(work.name);
                    if (bp == null) continue;
                    int bpc_need = bp.BPCNeed(work.jobRun);
                    if (bpc_need < 0) continue;
                    string bpc_name = work.name + " Blueprint";

                    if(all_bpc[bp.MakeAt()].ContainsKey(bpc_name) && bpc_need < all_bpc[bp.MakeAt()][bpc_name])
                    {
                        all_bpc[bp.MakeAt()][bpc_name] -= bpc_need;
                        bpc_need = 0;
                    } else if(all_bpc[bp.MakeAt()].ContainsKey(bpc_name))
                    {
                        bpc_need -= all_bpc[bp.MakeAt()][bpc_name];
                        all_bpc[bp.MakeAt()].Remove(bpc_name);
                    }

                    if (all_bpc[FacilityType.COPY_RESEARCH].ContainsKey(bpc_name) && bpc_need < all_bpc[FacilityType.COPY_RESEARCH][bpc_name])
                    {
                        all_bpc[FacilityType.COPY_RESEARCH][bpc_name] -= bpc_need;
                        bpc_need = 0;
                    }
                    else if (all_bpc[FacilityType.COPY_RESEARCH].ContainsKey(bpc_name))
                    {
                        bpc_need -= all_bpc[FacilityType.COPY_RESEARCH][bpc_name];
                        all_bpc[FacilityType.COPY_RESEARCH].Remove(bpc_name);
                    }

                    if (copying.ContainsKey(bpc_name) && copying[bpc_name] > bpc_need)
                    {
                        copying[bpc_name] -= bpc_need;
                        bpc_need = 0;
                    } else if(copying.ContainsKey(bpc_name))
                    {
                        bpc_need -= copying[bpc_name];
                        copying.Remove(bpc_name);
                    }

                    if (bpc_need == 0) continue;
                    ListViewItem item = new ListViewItem(bpc_name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" +  bpc_need);
                    item.Group = cgroup;
                    RunList.Items.Add(item);
                }
            }
        }

        private void CopyRunPlan_Click(object sender, EventArgs e)
        {
            
            Clipboard.SetText(GenerateRunPlan());
        }
        string GenerateRunPlan()
        {
            if (currPlan == null) return "";
            int maxLen = 0;
            string text = "";
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun != 0)
                {
                    maxLen = Math.Max(maxLen, work.name.Length);
                }
            }
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun != 0)
                {
                    string line = work.name + new string(' ', maxLen - work.name.Length + 4) + "||    " + work.jobRun + "\n";
                    text += line;
                }
            }
            return text;
        }

        private void CopyBuyPlan_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GenerateBuyPlan());
        }

        string GenerateBuyPlan()
        {
            if (currPlan == null) return "";
            int maxLen = 0;
            string text = "";
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun == 0 && work.amount != 0)
                {
                    maxLen = Math.Max(maxLen, work.name.Length);
                }
            }
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun == 0 && work.amount != 0)
                {
                    string line = work.name.Trim() + "\t" + work.amount + "\n";
                    text += line;
                }
            }
            return text;
        }

        private void CopyHaulPlan_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GenerateHaulPlan());
        }

        string GenerateHaulPlan()
        {
            if (currPlan == null) return "";
            string text = "";
            foreach (var haulroad in currPlan.Item2.Keys)
            {
                string road = "" + Storage.GetFacilityName(haulroad.Item1) + "\n->\n" + Storage.GetFacilityName(haulroad.Item2) + "\n";
                text += road;

                int maxLen = 0;
                foreach (string item in currPlan.Item2[haulroad].Keys)
                {
                    maxLen = Math.Max(maxLen, item.Length);
                }
                foreach (string item in currPlan.Item2[haulroad].Keys)
                {
                    string line = "    " + item + new string(' ', maxLen - item.Length + 4) + "||    " + currPlan.Item2[haulroad][item] + "\n";
                    text += line;
                }
                text += "\n" + new string('=', 20) + "\n\n";
            }
            
            return text;
        }

        string itemName = "";

        private void allBPList_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if(e.Item != null)
            {
                itemName = e.Item.Text;
            } else
            {
                itemName = "";
            }
        }

        private void allBPList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string text = OutputText.Text.Trim();
            if(text != "")
            {
                text += "\r\n" + itemName;
            } else
            {
                text = itemName;
            }
            OutputText.Text = text;
        }
                private void login_btn_Click(object sender, EventArgs e)
        {
            SSO sso = new SSO();
            sso.Show();
        }

        private void HaulList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(HaulList.SelectedItems.Count > 0)
            {
                string name = HaulList.SelectedItems[0].SubItems[0].Text;
                Clipboard.SetText(name.Trim());
            }
        }

        private void RunList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(RunList.SelectedItems.Count > 0)
            {
                string name = RunList.SelectedItems[0].SubItems[0].Text;
                Clipboard.SetText(name.Trim());
            }
        }

        private void inventCheckBtn_Click(object sender, EventArgs e)
        {
            (new InventionCheck()).Show();
        }

        private void btnBPTypeManagement_Click(object sender, EventArgs e)
        {
            (new BPCVariant()).Show();
        }
    }
}