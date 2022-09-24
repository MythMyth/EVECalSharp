namespace EveCal
{
    partial class BPCVariant
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BPCList = new System.Windows.Forms.ListBox();
            this.DescriptorList = new System.Windows.Forms.ListBox();
            this.NameTxt = new System.Windows.Forms.Label();
            this.OutputTxt = new System.Windows.Forms.Label();
            this.MeTxt = new System.Windows.Forms.Label();
            this.TETxt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BPCList
            // 
            this.BPCList.FormattingEnabled = true;
            this.BPCList.ItemHeight = 15;
            this.BPCList.Location = new System.Drawing.Point(12, 76);
            this.BPCList.Name = "BPCList";
            this.BPCList.Size = new System.Drawing.Size(254, 364);
            this.BPCList.TabIndex = 0;
            // 
            // DescriptorList
            // 
            this.DescriptorList.FormattingEnabled = true;
            this.DescriptorList.ItemHeight = 15;
            this.DescriptorList.Location = new System.Drawing.Point(298, 74);
            this.DescriptorList.Name = "DescriptorList";
            this.DescriptorList.Size = new System.Drawing.Size(254, 364);
            this.DescriptorList.TabIndex = 0;
            // 
            // NameTxt
            // 
            this.NameTxt.AutoSize = true;
            this.NameTxt.Location = new System.Drawing.Point(12, 18);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(39, 15);
            this.NameTxt.TabIndex = 1;
            this.NameTxt.Text = "Name";
            // 
            // OutputTxt
            // 
            this.OutputTxt.AutoSize = true;
            this.OutputTxt.Location = new System.Drawing.Point(12, 45);
            this.OutputTxt.Name = "OutputTxt";
            this.OutputTxt.Size = new System.Drawing.Size(45, 15);
            this.OutputTxt.TabIndex = 2;
            this.OutputTxt.Text = "Output";
            // 
            // MeTxt
            // 
            this.MeTxt.AutoSize = true;
            this.MeTxt.Location = new System.Drawing.Point(269, 18);
            this.MeTxt.Name = "MeTxt";
            this.MeTxt.Size = new System.Drawing.Size(24, 15);
            this.MeTxt.TabIndex = 3;
            this.MeTxt.Text = "ME";
            // 
            // TETxt
            // 
            this.TETxt.AutoSize = true;
            this.TETxt.Location = new System.Drawing.Point(269, 45);
            this.TETxt.Name = "TETxt";
            this.TETxt.Size = new System.Drawing.Size(19, 15);
            this.TETxt.TabIndex = 3;
            this.TETxt.Text = "TE";
            // 
            // BPCVariant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 450);
            this.Controls.Add(this.TETxt);
            this.Controls.Add(this.MeTxt);
            this.Controls.Add(this.OutputTxt);
            this.Controls.Add(this.NameTxt);
            this.Controls.Add(this.DescriptorList);
            this.Controls.Add(this.BPCList);
            this.Name = "BPCVariant";
            this.Text = "BPCVariant";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox BPCList;
        private ListBox DescriptorList;
        private Label NameTxt;
        private Label OutputTxt;
        private Label MeTxt;
        private Label TETxt;
    }
}