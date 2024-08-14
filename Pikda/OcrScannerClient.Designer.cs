using System.Windows.Forms;

namespace Pikda
{
    partial class OcrScannerClientForm
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_reInit_Camera = new System.Windows.Forms.Button();
            this.btn_ReRead = new System.Windows.Forms.Button();
            this.AreasViewGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.LayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 2;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 958F));
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.LayoutPanel.Controls.Add(this.camera, 0, 0);
            this.LayoutPanel.Controls.Add(this.panelControl1, 1, 0);
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
            this.camera.Size = new System.Drawing.Size(952, 560);
            this.camera.TabIndex = 1;
            this.camera.Text = "cameraControl1";
            this.camera.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureEdit_Paint);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.richTextBox1);
            this.panelControl1.Controls.Add(this.btn_reInit_Camera);
            this.panelControl1.Controls.Add(this.btn_ReRead);
            this.panelControl1.Controls.Add(this.AreasViewGrid);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(961, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(266, 560);
            this.panelControl1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(2, 307);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(267, 157);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // btn_reInit_Camera
            // 
            this.btn_reInit_Camera.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_reInit_Camera.Location = new System.Drawing.Point(2, 470);
            this.btn_reInit_Camera.Name = "btn_reInit_Camera";
            this.btn_reInit_Camera.Size = new System.Drawing.Size(262, 44);
            this.btn_reInit_Camera.TabIndex = 3;
            this.btn_reInit_Camera.Text = "Reload Camera";
            this.btn_reInit_Camera.UseVisualStyleBackColor = true;
            this.btn_reInit_Camera.Click += new System.EventHandler(this.btn_reInit_Camera_Click);
            // 
            // btn_ReRead
            // 
            this.btn_ReRead.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_ReRead.Location = new System.Drawing.Point(2, 514);
            this.btn_ReRead.Name = "btn_ReRead";
            this.btn_ReRead.Size = new System.Drawing.Size(262, 44);
            this.btn_ReRead.TabIndex = 2;
            this.btn_ReRead.Text = "Read Again";
            this.btn_ReRead.UseVisualStyleBackColor = true;
            this.btn_ReRead.Click += new System.EventHandler(this.btn_ReRead_Click);
            // 
            // AreasViewGrid
            // 
            this.AreasViewGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AreasViewGrid.Location = new System.Drawing.Point(2, 2);
            this.AreasViewGrid.MainView = this.gridView1;
            this.AreasViewGrid.Name = "AreasViewGrid";
            this.AreasViewGrid.Size = new System.Drawing.Size(262, 556);
            this.AreasViewGrid.TabIndex = 1;
            this.AreasViewGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.AreasViewGrid;
            this.gridView1.Name = "gridView1";
            // 
            // OcrScannerClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 566);
            this.Controls.Add(this.LayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OcrScannerClientForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingFun);
            this.Load += new System.EventHandler(this.OcrScannerForm_Load);
            this.Shown += new System.EventHandler(this.OcrScannerClientForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OcrScannerForm_Paint);
            this.LayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private DevExpress.XtraEditors.Camera.CameraControl camera;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Button btn_ReRead;
        private DevExpress.XtraGrid.GridControl AreasViewGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private Button btn_reInit_Camera;
        private RichTextBox richTextBox1;
    }
}

