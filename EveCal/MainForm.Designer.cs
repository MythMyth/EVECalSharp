namespace EveCal
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AssetButton = new System.Windows.Forms.Button();
            this.MakePlanButton = new System.Windows.Forms.Button();
            this.RunList = new System.Windows.Forms.ListView();
            this.columnHeader0 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.BuyList = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.HaulList = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.CopyRunPlan = new System.Windows.Forms.Button();
            this.CopyBuyPlan = new System.Windows.Forms.Button();
            this.CopyHaulPlan = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OutputText = new System.Windows.Forms.TextBox();
            this.allBPList = new System.Windows.Forms.ListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.login_btn = new System.Windows.Forms.Button();
            this.inventCheckBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AssetButton
            // 
            this.AssetButton.Location = new System.Drawing.Point(12, 12);
            this.AssetButton.Name = "AssetButton";
            this.AssetButton.Size = new System.Drawing.Size(107, 23);
            this.AssetButton.TabIndex = 2;
            this.AssetButton.Text = "Update Asset";
            this.AssetButton.UseVisualStyleBackColor = true;
            this.AssetButton.Click += new System.EventHandler(this.AssetButton_Click);
            // 
            // MakePlanButton
            // 
            this.MakePlanButton.Location = new System.Drawing.Point(125, 12);
            this.MakePlanButton.Name = "MakePlanButton";
            this.MakePlanButton.Size = new System.Drawing.Size(107, 23);
            this.MakePlanButton.TabIndex = 3;
            this.MakePlanButton.Text = "Make Plan";
            this.MakePlanButton.UseVisualStyleBackColor = true;
            this.MakePlanButton.Click += new System.EventHandler(this.MakePlanButton_Click);
            // 
            // RunList
            // 
            this.RunList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RunList.BackColor = System.Drawing.SystemColors.Info;
            this.RunList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2});
            this.RunList.GridLines = true;
            this.RunList.Location = new System.Drawing.Point(715, 41);
            this.RunList.MultiSelect = false;
            this.RunList.Name = "RunList";
            this.RunList.Size = new System.Drawing.Size(313, 678);
            this.RunList.TabIndex = 4;
            this.RunList.UseCompatibleStateImageBehavior = false;
            this.RunList.View = System.Windows.Forms.View.Details;
            this.RunList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.RunList_ItemSelectionChanged);
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "Item";
            this.columnHeader0.Width = 200;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = " ";
            this.columnHeader1.Width = 20;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Amount";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BuyList
            // 
            this.BuyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.BuyList.BackColor = System.Drawing.SystemColors.Info;
            this.BuyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.BuyList.GridLines = true;
            this.BuyList.Location = new System.Drawing.Point(1049, 41);
            this.BuyList.MultiSelect = false;
            this.BuyList.Name = "BuyList";
            this.BuyList.Size = new System.Drawing.Size(305, 678);
            this.BuyList.TabIndex = 5;
            this.BuyList.UseCompatibleStateImageBehavior = false;
            this.BuyList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Item";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = " ";
            this.columnHeader4.Width = 20;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Amount";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // HaulList
            // 
            this.HaulList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.HaulList.BackColor = System.Drawing.SystemColors.Info;
            this.HaulList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.HaulList.GridLines = true;
            this.HaulList.Location = new System.Drawing.Point(1371, 41);
            this.HaulList.MultiSelect = false;
            this.HaulList.Name = "HaulList";
            this.HaulList.Size = new System.Drawing.Size(426, 678);
            this.HaulList.TabIndex = 6;
            this.HaulList.UseCompatibleStateImageBehavior = false;
            this.HaulList.View = System.Windows.Forms.View.Details;
            this.HaulList.SelectedIndexChanged += new System.EventHandler(this.HaulList_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Item";
            this.columnHeader6.Width = 300;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = " ";
            this.columnHeader7.Width = 30;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Amount";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader8.Width = 70;
            // 
            // CopyRunPlan
            // 
            this.CopyRunPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CopyRunPlan.Location = new System.Drawing.Point(715, 725);
            this.CopyRunPlan.Name = "CopyRunPlan";
            this.CopyRunPlan.Size = new System.Drawing.Size(313, 23);
            this.CopyRunPlan.TabIndex = 7;
            this.CopyRunPlan.Text = "Copy";
            this.CopyRunPlan.UseVisualStyleBackColor = true;
            this.CopyRunPlan.Click += new System.EventHandler(this.CopyRunPlan_Click);
            // 
            // CopyBuyPlan
            // 
            this.CopyBuyPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CopyBuyPlan.Location = new System.Drawing.Point(1049, 725);
            this.CopyBuyPlan.Name = "CopyBuyPlan";
            this.CopyBuyPlan.Size = new System.Drawing.Size(305, 23);
            this.CopyBuyPlan.TabIndex = 7;
            this.CopyBuyPlan.Text = "Copy";
            this.CopyBuyPlan.UseVisualStyleBackColor = true;
            this.CopyBuyPlan.Click += new System.EventHandler(this.CopyBuyPlan_Click);
            // 
            // CopyHaulPlan
            // 
            this.CopyHaulPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CopyHaulPlan.Location = new System.Drawing.Point(1371, 725);
            this.CopyHaulPlan.Name = "CopyHaulPlan";
            this.CopyHaulPlan.Size = new System.Drawing.Size(426, 23);
            this.CopyHaulPlan.TabIndex = 7;
            this.CopyHaulPlan.Text = "Copy";
            this.CopyHaulPlan.UseVisualStyleBackColor = true;
            this.CopyHaulPlan.Click += new System.EventHandler(this.CopyHaulPlan_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(715, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Run:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1049, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Buy:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1371, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Haul:";
            // 
            // OutputText
            // 
            this.OutputText.AcceptsReturn = true;
            this.OutputText.AcceptsTab = true;
            this.OutputText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.OutputText.Location = new System.Drawing.Point(351, 41);
            this.OutputText.Multiline = true;
            this.OutputText.Name = "OutputText";
            this.OutputText.Size = new System.Drawing.Size(358, 707);
            this.OutputText.TabIndex = 11;
            // 
            // allBPList
            // 
            this.allBPList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.allBPList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9});
            this.allBPList.Location = new System.Drawing.Point(12, 41);
            this.allBPList.MultiSelect = false;
            this.allBPList.Name = "allBPList";
            this.allBPList.Size = new System.Drawing.Size(333, 707);
            this.allBPList.TabIndex = 12;
            this.allBPList.UseCompatibleStateImageBehavior = false;
            this.allBPList.View = System.Windows.Forms.View.Tile;
            this.allBPList.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.allBPList_ItemMouseHover);
            this.allBPList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.allBPList_MouseDoubleClick);
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "";
            // 
            // login_btn
            // 
            this.login_btn.Location = new System.Drawing.Point(270, 12);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(75, 23);
            this.login_btn.TabIndex = 13;
            this.login_btn.Text = "Character";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.login_btn_Click);
            // 
            // inventCheckBtn
            // 
            this.inventCheckBtn.Location = new System.Drawing.Point(531, 12);
            this.inventCheckBtn.Name = "inventCheckBtn";
            this.inventCheckBtn.Size = new System.Drawing.Size(178, 23);
            this.inventCheckBtn.TabIndex = 14;
            this.inventCheckBtn.Text = "Invent Material Check";
            this.inventCheckBtn.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1809, 760);
            this.Controls.Add(this.inventCheckBtn);
            this.Controls.Add(this.login_btn);
            this.Controls.Add(this.allBPList);
            this.Controls.Add(this.OutputText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CopyHaulPlan);
            this.Controls.Add(this.CopyBuyPlan);
            this.Controls.Add(this.CopyRunPlan);
            this.Controls.Add(this.HaulList);
            this.Controls.Add(this.BuyList);
            this.Controls.Add(this.RunList);
            this.Controls.Add(this.MakePlanButton);
            this.Controls.Add(this.AssetButton);
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button AssetButton;
        private Button MakePlanButton;
        private ListView RunList;
        private ColumnHeader Items;
        private ColumnHeader columnHeader0;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ListView BuyList;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ListView HaulList;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private Button CopyRunPlan;
        private Button CopyBuyPlan;
        private Button CopyHaulPlan;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox OutputText;
        private ListView allBPList;
        private ColumnHeader columnHeader9;
        private Button login_btn;
        private Button inventCheckBtn;
    }
}