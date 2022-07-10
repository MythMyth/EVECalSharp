namespace EveCal
{
    partial class SSO
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.add_char = new System.Windows.Forms.Button();
            this.delete_char = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(268, 397);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // add_char
            // 
            this.add_char.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.add_char.Location = new System.Drawing.Point(12, 415);
            this.add_char.Name = "add_char";
            this.add_char.Size = new System.Drawing.Size(133, 23);
            this.add_char.TabIndex = 1;
            this.add_char.Text = "Add";
            this.add_char.UseVisualStyleBackColor = true;
            this.add_char.Click += new System.EventHandler(this.add_char_Click);
            // 
            // delete_char
            // 
            this.delete_char.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delete_char.Location = new System.Drawing.Point(151, 415);
            this.delete_char.Name = "delete_char";
            this.delete_char.Size = new System.Drawing.Size(131, 23);
            this.delete_char.TabIndex = 2;
            this.delete_char.Text = "Delete";
            this.delete_char.UseVisualStyleBackColor = true;
            this.delete_char.Click += new System.EventHandler(this.delete_char_Click);
            // 
            // SSO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 450);
            this.Controls.Add(this.delete_char);
            this.Controls.Add(this.add_char);
            this.Controls.Add(this.listView1);
            this.Name = "SSO";
            this.Text = "SSO";
            this.ResumeLayout(false);

        }

        #endregion

        private ListView listView1;
        private Button add_char;
        private Button delete_char;
    }
}