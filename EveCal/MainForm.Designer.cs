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
            this.planList = new System.Windows.Forms.ListView();
            this.columnHeader0 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
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
            // planList
            // 
            this.planList.BackColor = System.Drawing.SystemColors.Info;
            this.planList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2});
            this.planList.GridLines = true;
            this.planList.Location = new System.Drawing.Point(286, 12);
            this.planList.MultiSelect = false;
            this.planList.Name = "planList";
            this.planList.Size = new System.Drawing.Size(502, 426);
            this.planList.TabIndex = 4;
            this.planList.UseCompatibleStateImageBehavior = false;
            this.planList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "Item";
            this.columnHeader0.Width = 360;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = " ";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Amount";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.planList);
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
        private ListView planList;
        private ColumnHeader Items;
        private ColumnHeader columnHeader0;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
    }
}