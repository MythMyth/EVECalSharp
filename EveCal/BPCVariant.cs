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
        }
    }
}
