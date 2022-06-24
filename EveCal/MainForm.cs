namespace EveCal
{
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

            plan.MakePlan(true);
            currPlan = plan.MakePlan(true);
            RunList.Items.Clear();
            BuyList.Items.Clear();
            HaulList.Items.Clear();
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
            foreach (ItemWorkDetail work in currPlan.Item1)
            {
                if (work.jobRun != 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.jobRun);
                    RunList.Items.Add(item);
                }
            }

            foreach (var key in currPlan.Item2.Keys)
            {
                FacilityType haulFrom = key.Item1, haulTo = key.Item2;
                ListViewGroup gr = new ListViewGroup("" + haulFrom + " -> " + haulTo);
                HaulList.Groups.Add(gr);
                foreach (string item in currPlan.Item2[key].Keys)
                {
                    ListViewItem listItem = new ListViewItem(item);
                    listItem.SubItems.Add(" x ");
                    listItem.SubItems.Add("" + currPlan.Item2[key][item]);
                    listItem.Group = gr;
                    HaulList.Items.Add(listItem);
                }
            }

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
                string road = "" + haulroad.Item1 + "->" + haulroad.Item2+ "\n";
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
    }
}