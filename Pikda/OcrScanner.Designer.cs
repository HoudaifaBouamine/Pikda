using System.Windows.Forms;

namespace Pikda
{
    partial class OcrScannerForm
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
            this.LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AreasViewGrid = new DevExpress.XtraGrid.GridControl();
            this.camera = new DevExpress.XtraEditors.Camera.CameraControl();
            this.LayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).BeginInit();
            
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 2;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 977F));
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.LayoutPanel.Controls.Add(this.AreasViewGrid, 1, 0);
            this.LayoutPanel.Controls.Add(this.camera, 0, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 1;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Size = new System.Drawing.Size(1230, 566);
            this.LayoutPanel.TabIndex = 0;
            // 
            // AreasViewGrid
            // 
            this.AreasViewGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AreasViewGrid.Location = new System.Drawing.Point(980, 3);
            this.AreasViewGrid.Name = "AreasViewGrid";
            this.AreasViewGrid.Size = new System.Drawing.Size(247, 560);
            this.AreasViewGrid.TabIndex = 0;
            // 
            // camera
            // 
            this.camera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camera.Location = new System.Drawing.Point(3, 3);
            this.camera.Name = "camera";
            this.camera.Size = new System.Drawing.Size(971, 560);
            this.camera.TabIndex = 1;
            this.camera.Text = "cameraControl1";
            this.camera.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureEdit_Paint);
            this.camera.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cameraControl1_MouseClick);
            this.camera.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureEdit_MouseDown);
            this.camera.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureEdit_MouseMove);
            this.camera.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureEdit_MouseUp);
            // 
            // OcrScannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 566);
            this.Controls.Add(this.LayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OcrScannerForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.OcrScannerForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OcrScannerForm_Paint);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingFun);
            this.LayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private DevExpress.XtraGrid.GridControl AreasViewGrid;
        private DevExpress.XtraEditors.Camera.CameraControl camera;
    }
}

