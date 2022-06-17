using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveCal
{
    public partial class AssetViewer : Form
    {

        Dictionary<FacilityType, Button> ButtonMap = new Dictionary<FacilityType, Button>();
        Button currBtn;
        public AssetViewer()
        {
            InitializeComponent();
            ButtonMap.Add(FacilityType.SOURCE, button1);
            ButtonMap.Add(FacilityType.ADV_COMPONENT, button2);
            ButtonMap.Add(FacilityType.ADV_LARGE_SHIP, button3);
            ButtonMap.Add(FacilityType.ADV_MED_SHIP, button4);
            ButtonMap.Add(FacilityType.ADV_SMALL_SHIP, button5);
            ButtonMap.Add(FacilityType.FUEL_COMP, button6);
            ButtonMap.Add(FacilityType.LARGE_SHIP, button7);
            ButtonMap.Add(FacilityType.MEDIUM_SHIP, button8);
            ButtonMap.Add(FacilityType.SMALL_SHIP, button9);
            ButtonMap.Add(FacilityType.REACTION, button10);
            SetButtonTitle();

        }

        void SetButtonTitle()
        {
            ButtonMap[FacilityType.SOURCE].Tag = FacilityType.SOURCE;
            ButtonMap[FacilityType.SOURCE].Text = "Home station";

            ButtonMap[FacilityType.ADV_COMPONENT].Tag = FacilityType.ADV_COMPONENT;
            ButtonMap[FacilityType.ADV_COMPONENT].Text = "Advance Component";

            ButtonMap[FacilityType.ADV_LARGE_SHIP].Tag = FacilityType.ADV_LARGE_SHIP;
            ButtonMap[FacilityType.ADV_LARGE_SHIP].Text = "T2 Large Ship";

            ButtonMap[FacilityType.ADV_MED_SHIP].Tag = FacilityType.ADV_MED_SHIP;
            ButtonMap[FacilityType.ADV_MED_SHIP].Text = "T2 Medium Ship";

            ButtonMap[FacilityType.ADV_SMALL_SHIP].Tag = FacilityType.ADV_SMALL_SHIP;
            ButtonMap[FacilityType.ADV_SMALL_SHIP].Text = "T2 Small Ship";

            ButtonMap[FacilityType.FUEL_COMP].Tag = FacilityType.FUEL_COMP;
            ButtonMap[FacilityType.FUEL_COMP].Text = "Fuel and Component";

            ButtonMap[FacilityType.LARGE_SHIP].Tag = FacilityType.LARGE_SHIP;
            ButtonMap[FacilityType.LARGE_SHIP].Text = "Large Ship";

            ButtonMap[FacilityType.MEDIUM_SHIP].Tag = FacilityType.MEDIUM_SHIP;
            ButtonMap[FacilityType.MEDIUM_SHIP].Text = "Medium Ship";

            ButtonMap[FacilityType.SMALL_SHIP].Tag = FacilityType.SMALL_SHIP;
            ButtonMap[FacilityType.SMALL_SHIP].Text = "Small Ship";

            ButtonMap[FacilityType.REACTION].Tag = FacilityType.REACTION;
            ButtonMap[FacilityType.REACTION].Text = "Reaction";
        }

        private void button_Click(object sender, EventArgs e)
        {
            if(currBtn != null) currBtn.BackColor = Color.White;
            currBtn = (Button)sender;
            currBtn.BackColor = Color.Aqua;
            Dictionary<string, int> map = Storage.GetFacilityAsset((FacilityType)currBtn.Tag);
            string text = "";
            foreach(string key in map.Keys)
            {
                text += key + "\t" + map[key] + "\r\n";
            }
            AssetTextBox.Text = text;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if(currBtn != null) Storage.UpdateAsset(AssetTextBox.Text, (FacilityType)currBtn.Tag);
        }
    }
}
