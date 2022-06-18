namespace EveCal
{
    using HaulDetail = Dictionary<Tuple<FacilityType, FacilityType>, Dictionary<string, int>>;
    public partial class MainForm : Form
    {        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BuildPlan plan = new BuildPlan();
            plan.Add("MA Ishtar 0 3 6", 1);
            plan.Add("MA Cerberus 0 3 6", 1);
            plan.MakePlan(true);
            Tuple<List<ItemWorkDetail>, HaulDetail> works = plan.MakePlan(true);
            RunList.Items.Clear();
            BuyList.Items.Clear();
            HaulList.Items.Clear();
            foreach (ItemWorkDetail work in works.Item1)
            {
                if(work.jobRun == 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.amount);
                    BuyList.Items.Add(item);
                }    
                    
            }
            foreach (ItemWorkDetail work in works.Item1)
            {
                if (work.jobRun != 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.jobRun);
                    RunList.Items.Add(item);
                }
            }

            foreach (var key in works.Item2.Keys)
            {
                FacilityType haulFrom = key.Item1, haulTo = key.Item2;
                ListViewGroup gr = new ListViewGroup("" + haulFrom + " -> " + haulTo);
                HaulList.Groups.Add(gr);
                foreach(string item in works.Item2[key].Keys)
                {
                    ListViewItem listItem = new ListViewItem(item);
                    listItem.SubItems.Add(" x ");
                    listItem.SubItems.Add("" + works.Item2[key][item]);
                    listItem.Group = gr;
                    HaulList.Items.Add(listItem);
                }
            }

        }

        private void AssetButton_Click(object sender, EventArgs e)
        {
            AssetViewer assetView = new AssetViewer();
            assetView.Show();
        }

        private void MakePlanButton_Click(object sender, EventArgs e)
        {

        }
    }
}