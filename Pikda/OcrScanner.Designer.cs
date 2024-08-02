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
            this.camera = new DevExpress.XtraEditors.Camera.CameraControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AreasViewGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.LayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 2;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 977F));
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.LayoutPanel.Controls.Add(this.camera, 0, 0);
            this.LayoutPanel.Controls.Add(this.panel1, 1, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 1;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Size = new System.Drawing.Size(1230, 566);
            this.LayoutPanel.TabIndex = 0;
            this.LayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.LayoutPanel_Paint);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Refresh);
            this.panel1.Controls.Add(this.AreasViewGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(980, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 560);
            this.panel1.TabIndex = 2;
            // 
            // AreasViewGrid
            // 
            this.AreasViewGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AreasViewGrid.Location = new System.Drawing.Point(0, 0);
            this.AreasViewGrid.MainView = this.gridView1;
            this.AreasViewGrid.Name = "AreasViewGrid";
            this.AreasViewGrid.Size = new System.Drawing.Size(247, 560);
            this.AreasViewGrid.TabIndex = 1;
            this.AreasViewGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.AreasViewGrid;
            this.gridView1.Name = "gridView1";
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Refresh.Location = new System.Drawing.Point(0, 512);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(247, 48);
            this.btn_Refresh.TabIndex = 2;
            this.btn_Refresh.Text = "Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingFun);
            this.Load += new System.EventHandler(this.OcrScannerForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OcrScannerForm_Paint);
            this.LayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private DevExpress.XtraEditors.Camera.CameraControl camera;
        private Panel panel1;
        private Button btn_Refresh;
        private DevExpress.XtraGrid.GridControl AreasViewGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}

