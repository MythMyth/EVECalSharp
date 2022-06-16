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
            string output = "";
            foreach(ItemWorkDetail work in works)
            {
                if(work.jobRun == 0)
                    output += work.name + " Buy: " + work.amount + "\r\n";
            }
            output += "==========================\r\n";
            foreach (ItemWorkDetail work in works)
            {
                if (work.jobRun != 0)
                    output += work.name + " Run:" + work.jobRun + "\r\n";
            }
            outputTxt.Text = output;
        }

        private void AssetButton_Click(object sender, EventArgs e)
        {

        }

        private void MakePlanButton_Click(object sender, EventArgs e)
        {

        }
    }
}