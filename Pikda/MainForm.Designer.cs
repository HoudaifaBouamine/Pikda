namespace Pikda
{
    partial class MainForm
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
            this.btn_ScanCard = new System.Windows.Forms.Button();
            this.btn_AddModel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_ScanCard
            // 
            this.btn_ScanCard.Location = new System.Drawing.Point(39, 86);
            this.btn_ScanCard.Name = "btn_ScanCard";
            this.btn_ScanCard.Size = new System.Drawing.Size(150, 46);
            this.btn_ScanCard.TabIndex = 0;
            this.btn_ScanCard.Text = "Scan Card";
            this.btn_ScanCard.UseVisualStyleBackColor = true;
            this.btn_ScanCard.Click += new System.EventHandler(this.btn_ScanCard_Click);
            // 
            // btn_AddModel
            // 
            this.btn_AddModel.Location = new System.Drawing.Point(227, 86);
            this.btn_AddModel.Name = "btn_AddModel";
            this.btn_AddModel.Size = new System.Drawing.Size(150, 46);
            this.btn_AddModel.TabIndex = 1;
            this.btn_AddModel.Text = "Add Model";
            this.btn_AddModel.UseVisualStyleBackColor = true;
            this.btn_AddModel.Click += new System.EventHandler(this.btn_AddModel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_AddModel);
            this.Controls.Add(this.btn_ScanCard);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_ScanCard;
        private System.Windows.Forms.Button btn_AddModel;
    }
}