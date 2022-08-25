namespace EveCal
{
    partial class AssetViewer
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
            this.reload = new System.Windows.Forms.Button();
            this.FacilityList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.coverLabel = new System.Windows.Forms.Label();
            this.AssetList = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.FacilityListBox = new System.Windows.Forms.ListBox();
            this.CopyBtn = new System.Windows.Forms.Button();
            this.RunningBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reload
            // 
            this.reload.Location = new System.Drawing.Point(1038, 726);
            this.reload.Name = "reload";
            this.reload.Size = new System.Drawing.Size(450, 42);
            this.reload.TabIndex = 6;
            this.reload.Text = "Reload Asset";
            this.reload.UseVisualStyleBackColor = true;
            this.reload.Click += new System.EventHandler(this.reload_Click);
            // 
            // FacilityList
            // 
            this.FacilityList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.FacilityList.Location = new System.Drawing.Point(1038, 12);
            this.FacilityList.MultiSelect = false;
            this.FacilityList.Name = "FacilityList";
            this.FacilityList.Size = new System.Drawing.Size(450, 708);
            this.FacilityList.TabIndex = 7;
            this.FacilityList.UseCompatibleStateImageBehavior = false;
            this.FacilityList.View = System.Windows.Forms.View.List;
            this.FacilityList.SelectedIndexChanged += new System.EventHandler(this.FacilityList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 300;
            // 
            // coverLabel
            // 
            this.coverLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.coverLabel.Location = new System.Drawing.Point(12, 9);
            this.coverLabel.Name = "coverLabel";
            this.coverLabel.Size = new System.Drawing.Size(1476, 759);
            this.coverLabel.TabIndex = 8;
            this.coverLabel.Text = "coverLabel";
            this.coverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.coverLabel.Visible = false;
            // 
            // AssetList
            // 
            this.AssetList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AssetList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.AssetList.GridLines = true;
            this.AssetList.Location = new System.Drawing.Point(457, 12);
            this.AssetList.Name = "AssetList";
            this.AssetList.Size = new System.Drawing.Size(571, 708);
            this.AssetList.TabIndex = 9;
            this.AssetList.UseCompatibleStateImageBehavior = false;
            this.AssetList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 450;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Amount";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 100;
            // 
            // FacilityListBox
            // 
            this.FacilityListBox.FormattingEnabled = true;
            this.FacilityListBox.ItemHeight = 15;
            this.FacilityListBox.Location = new System.Drawing.Point(12, 12);
            this.FacilityListBox.Name = "FacilityListBox";
            this.FacilityListBox.Size = new System.Drawing.Size(414, 709);
            this.FacilityListBox.TabIndex = 10;
            this.FacilityListBox.SelectedIndexChanged += new System.EventHandler(this.FacilityListBox_SelectedIndexChanged);
            // 
            // CopyBtn
            // 
            this.CopyBtn.Location = new System.Drawing.Point(457, 726);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(571, 42);
            this.CopyBtn.TabIndex = 11;
            this.CopyBtn.Text = "Copy";
            this.CopyBtn.UseVisualStyleBackColor = true;
            this.CopyBtn.Click += new System.EventHandler(this.CopyBtn_Click);
            // 
            // RunningBtn
            // 
            this.RunningBtn.Location = new System.Drawing.Point(12, 727);
            this.RunningBtn.Name = "RunningBtn";
            this.RunningBtn.Size = new System.Drawing.Size(414, 41);
            this.RunningBtn.TabIndex = 12;
            this.RunningBtn.Text = "Running";
            this.RunningBtn.UseVisualStyleBackColor = true;
            this.RunningBtn.Click += new System.EventHandler(this.RunningBtn_Click);
            // 
            // AssetViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 780);
            this.Controls.Add(this.coverLabel);
            this.Controls.Add(this.RunningBtn);
            this.Controls.Add(this.CopyBtn);
            this.Controls.Add(this.FacilityListBox);
            this.Controls.Add(this.AssetList);
            this.Controls.Add(this.FacilityList);
            this.Controls.Add(this.reload);
            this.Name = "AssetViewer";
            this.Text = "AssetViewer";
            this.ResumeLayout(false);

        }

        #endregion
        private Button reload;
        private ListView FacilityList;
        private ColumnHeader columnHeader1;
        private Label coverLabel;
        private ListView AssetList;
        private ColumnHeader columnHeader2;
        public ColumnHeader columnHeader3;
        private ListBox FacilityListBox;
        private Button CopyBtn;
        private Button RunningBtn;
    }
}