namespace EveCal
{
    partial class InventionCheck
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
            this.currentAssetList = new System.Windows.Forms.ListView();
            this.buyList = new System.Windows.Forms.ListView();
            this.copyBtn = new System.Windows.Forms.Button();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // currentAssetList
            // 
            this.currentAssetList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.currentAssetList.Location = new System.Drawing.Point(12, 12);
            this.currentAssetList.Name = "currentAssetList";
            this.currentAssetList.Size = new System.Drawing.Size(231, 416);
            this.currentAssetList.TabIndex = 0;
            this.currentAssetList.UseCompatibleStateImageBehavior = false;
            this.currentAssetList.View = System.Windows.Forms.View.List;
            // 
            // buyList
            // 
            this.buyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.buyList.Location = new System.Drawing.Point(277, 12);
            this.buyList.Name = "buyList";
            this.buyList.Size = new System.Drawing.Size(231, 416);
            this.buyList.TabIndex = 0;
            this.buyList.UseCompatibleStateImageBehavior = false;
            this.buyList.View = System.Windows.Forms.View.List;
            // 
            // copyBtn
            // 
            this.copyBtn.Location = new System.Drawing.Point(532, 12);
            this.copyBtn.Name = "copyBtn";
            this.copyBtn.Size = new System.Drawing.Size(128, 23);
            this.copyBtn.TabIndex = 1;
            this.copyBtn.Text = "Copy";
            this.copyBtn.UseVisualStyleBackColor = true;
            this.copyBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // InventionCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 450);
            this.Controls.Add(this.copyBtn);
            this.Controls.Add(this.buyList);
            this.Controls.Add(this.currentAssetList);
            this.Name = "InventionCheck";
            this.Text = "InventionCheck";
            this.ResumeLayout(false);

        }

        #endregion

        private ListView currentAssetList;
        private ListView buyList;
        private Button copyBtn;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
    }
}