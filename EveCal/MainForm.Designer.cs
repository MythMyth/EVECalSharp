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
            this.MakePlanButton.Location = new System.Drawing.Point(12, 51);
            this.MakePlanButton.Name = "MakePlanButton";
            this.MakePlanButton.Size = new System.Drawing.Size(107, 23);
            this.MakePlanButton.TabIndex = 3;
            this.MakePlanButton.Text = "Make Plan";
            this.MakePlanButton.UseVisualStyleBackColor = true;
            this.MakePlanButton.Click += new System.EventHandler(this.MakePlanButton_Click);
            // 
            // RunList
            // 
            this.RunList.BackColor = System.Drawing.SystemColors.Info;
            this.RunList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2});
            this.RunList.GridLines = true;
            this.RunList.Location = new System.Drawing.Point(251, 12);
            this.RunList.MultiSelect = false;
            this.RunList.Name = "RunList";
            this.RunList.Size = new System.Drawing.Size(214, 426);
            this.RunList.TabIndex = 4;
            this.RunList.UseCompatibleStateImageBehavior = false;
            this.RunList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "Item";
            this.columnHeader0.Width = 120;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = " ";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Amount";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BuyList
            // 
            this.BuyList.BackColor = System.Drawing.SystemColors.Info;
            this.BuyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.BuyList.GridLines = true;
            this.BuyList.Location = new System.Drawing.Point(486, 12);
            this.BuyList.MultiSelect = false;
            this.BuyList.Name = "BuyList";
            this.BuyList.Size = new System.Drawing.Size(214, 426);
            this.BuyList.TabIndex = 5;
            this.BuyList.UseCompatibleStateImageBehavior = false;
            this.BuyList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Item";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = " ";
            this.columnHeader4.Width = 30;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Amount";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // HaulList
            // 
            this.HaulList.BackColor = System.Drawing.SystemColors.Info;
            this.HaulList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.HaulList.GridLines = true;
            this.HaulList.Location = new System.Drawing.Point(719, 12);
            this.HaulList.MultiSelect = false;
            this.HaulList.Name = "HaulList";
            this.HaulList.Size = new System.Drawing.Size(214, 426);
            this.HaulList.TabIndex = 6;
            this.HaulList.UseCompatibleStateImageBehavior = false;
            this.HaulList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Item";
            this.columnHeader6.Width = 120;
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
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 450);
            this.Controls.Add(this.HaulList);
            this.Controls.Add(this.BuyList);
            this.Controls.Add(this.RunList);
            this.Controls.Add(this.MakePlanButton);
            this.Controls.Add(this.AssetButton);
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

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
    }
}