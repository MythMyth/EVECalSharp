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
    public partial class InventionCheck : Form
    {
        List<string> DataCore = new List<string>() { "Datacore - Amarrian Starship Engineering",
                                                     "Datacore - Caldari Starship Engineering",
                                                     "Datacore - Electromagnetic Physics",
                                                     "Datacore - Electronic Engineering",
                                                     "Datacore - Gallentean Starship Engineering",
                                                     "Datacore - Graviton Physics",
                                                     "Datacore - High Energy Physics",
                                                     "Datacore - Hydromagnetic Physics",
                                                     "Datacore - Laser Physics",
                                                     "Datacore - Mechanical Engineering",
                                                     "Datacore - Minmatar Starship Engineering",
                                                     "Datacore - Molecular Engineering",
                                                     "Datacore - Nanite Engineering",
                                                     "Datacore - Nuclear Physics",
                                                     "Datacore - Plasma Physics",
                                                     "Datacore - Quantum Physics",
                                                     "Datacore - Rocket Science"};
        List<string> Decryptor = new List<string>() { "Accelerant Decryptor",
                                                      "Attainment Decryptor",
                                                      "Augmentation Decryptor",
                                                      "Parity Decryptor",
                                                      "Process Decryptor",
                                                      "Symmetry Decryptor",
                                                      "Optimized Attainment Decryptor",
                                                      "Optimized Augmentation Decryptor"};

        Dictionary<string, int> datacores;
        Dictionary<string, int> decryptor;
        int datacoreLim = 1000;
        int decryptorLim = 1000;
        string buyStr = "";
        public InventionCheck()
        {
            InitializeComponent();
            datacores = Storage.GetNumberOfItems(DataCore);
            decryptor = Storage.GetNumberOfItems(Decryptor);

            foreach (string datacore in DataCore)
            {
                ListViewItem item = new ListViewItem(datacore);
                if (datacores.ContainsKey(datacore))
                    item.SubItems.Add(datacores[datacore] + "");
                else item.SubItems.Add("0");
                currentAssetList.Items.Add(item);
            }

            foreach (string decry in decryptor.Keys)
            {
                ListViewItem item = new ListViewItem(decry);
                item.SubItems.Add(decryptor[decry] + "");
                currentAssetList.Items.Add(item);
            }

            foreach (string datacore in DataCore)
            {
                if (datacores.ContainsKey(datacore))
                {
                    if (datacores[datacore] < datacoreLim / 2)
                    {
                        int buyNum = datacoreLim - datacores[datacore];
                        buyStr += datacore + "\t" + buyNum + "\n";

                        ListViewItem item = new ListViewItem(datacore);
                        item.SubItems.Add(buyNum + "");
                        buyList.Items.Add(item);
                    }
                }
                else
                {
                    buyStr += datacore + "\t" + datacoreLim + "\n";
                    ListViewItem item = new ListViewItem(datacore);
                    item.SubItems.Add(datacoreLim + "");
                    buyList.Items.Add(item);
                }
            }

            foreach(string decry in decryptor.Keys)
            {
                if (decryptor[decry] < decryptorLim / 2)
                {
                    ListViewItem item = new ListViewItem(decry);
                    item.SubItems.Add(decryptor[decry] + "");
                    buyList.Items.Add(item);
                    buyStr += decry + "\t" + (decryptorLim - decryptor[decry]) + "\n";
                }
            }

        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(buyStr);
            
        }
    }
}
