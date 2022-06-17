namespace EveCal
{
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
            List<ItemWorkDetail> works = plan.MakePlan(true);
            planList.Items.Clear();
            planList.Groups.Add(new ListViewGroup("Buy"));
            planList.Groups.Add(new ListViewGroup("Run"));
            planList.Groups.Add(new ListViewGroup("Haul"));
            foreach (ItemWorkDetail work in works)
            {
                if(work.jobRun == 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.amount);
                    item.Group = planList.Groups[0];
                    planList.Items.Add(item);
                }    
                    
            }
            foreach (ItemWorkDetail work in works)
            {
                if (work.jobRun != 0)
                {
                    ListViewItem item = new ListViewItem(work.name);
                    item.SubItems.Add("x");
                    item.SubItems.Add("" + work.jobRun);
                    item.Group = planList.Groups[1];
                    planList.Items.Add(item);
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