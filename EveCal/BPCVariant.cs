using EveCal.BPs;
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
    public partial class BPCVariant : Form
    {
        public BPCVariant()
        {
            InitializeComponent();
            foreach(string decryptor in Cache.DescriptorList.Keys)
            {
                DescriptorList.Items.Add(decryptor);
            }

            Dictionary<string, BP> allBP = Loader.GetAllBP();

            foreach (string bp in allBP.Keys)
            {
                if ((allBP[bp] is AdvSmallShip) || (allBP[bp] is AdvMediumShip) || (allBP[bp] is AdvLargeShip) || ( allBP[bp] is Module && ((Module)allBP[bp]).IsAdvModule()) )
                {
                    BPCList.Items.Add(bp);
                }
            }
        }
    }
}
