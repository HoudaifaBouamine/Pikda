namespace Pikda
{
    partial class OCR_Form
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
            this.PictureEditor = new DevExpress.XtraEditors.PictureEdit();
            this.AreasViewGrid = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CardsTable = new System.Windows.Forms.ListView();
            this.LayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureEditor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.ColumnCount = 3;
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.LayoutPanel.Controls.Add(this.PictureEditor, 0, 0);
            this.LayoutPanel.Controls.Add(this.AreasViewGrid, 2, 0);
            this.LayoutPanel.Controls.Add(this.CardsTable, 0, 0);
            this.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.RowCount = 1;
            this.LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutPanel.Size = new System.Drawing.Size(1230, 566);
            this.LayoutPanel.TabIndex = 0;
            // 
            // PictureEditor
            // 
            this.PictureEditor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureEditor.Location = new System.Drawing.Point(253, 3);
            this.PictureEditor.Name = "PictureEditor";
            this.PictureEditor.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.PictureEditor.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.PictureEditor.Size = new System.Drawing.Size(654, 560);
            this.PictureEditor.TabIndex = 3;
            this.PictureEditor.ImageChanged += new System.EventHandler(this.PictureEditor_ImageChanged);
            this.PictureEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureEdit_Paint);
            this.PictureEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureEdit_MouseDown);
            this.PictureEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureEdit_MouseMove);
            this.PictureEditor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureEdit_MouseUp);
            // 
            // AreasViewGrid
            // 
            this.AreasViewGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AreasViewGrid.Location = new System.Drawing.Point(913, 3);
            this.AreasViewGrid.MainView = this.gridView1;
            this.AreasViewGrid.Name = "AreasViewGrid";
            this.AreasViewGrid.Size = new System.Drawing.Size(314, 560);
            this.AreasViewGrid.TabIndex = 0;
            this.AreasViewGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.AreasViewGrid;
            this.gridView1.Name = "gridView1";
            // 
            // CardsTable
            // 
            this.CardsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardsTable.HideSelection = false;
            this.CardsTable.Location = new System.Drawing.Point(3, 3);
            this.CardsTable.MultiSelect = false;
            this.CardsTable.Name = "CardsTable";
            this.CardsTable.Size = new System.Drawing.Size(244, 560);
            this.CardsTable.TabIndex = 1;
            this.CardsTable.UseCompatibleStateImageBehavior = false;
            this.CardsTable.View = System.Windows.Forms.View.Tile;
            this.CardsTable.SelectedIndexChanged += new System.EventHandler(this.CardsTable_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 566);
            this.Controls.Add(this.LayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.LayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureEditor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AreasViewGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel LayoutPanel;
        private DevExpress.XtraGrid.GridControl AreasViewGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.ListView CardsTable;
        private DevExpress.XtraEditors.PictureEdit PictureEditor;
    }
}

