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
            this.outputTxt = new System.Windows.Forms.TextBox();
            this.AssetButton = new System.Windows.Forms.Button();
            this.MakePlanButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // outputTxt
            // 
            this.outputTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTxt.Location = new System.Drawing.Point(434, 12);
            this.outputTxt.Multiline = true;
            this.outputTxt.Name = "outputTxt";
            this.outputTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTxt.Size = new System.Drawing.Size(354, 426);
            this.outputTxt.TabIndex = 1;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MakePlanButton);
            this.Controls.Add(this.AssetButton);
            this.Controls.Add(this.outputTxt);
            this.Name = "MainForm";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox outputTxt;
        private Button AssetButton;
        private Button MakePlanButton;
    }
}