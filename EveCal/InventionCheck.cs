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
        public InventionCheck()
        {
            InitializeComponent();

        }

        private void copyBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
