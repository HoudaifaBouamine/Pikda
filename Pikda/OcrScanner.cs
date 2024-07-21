using DevExpress.Data.Camera;
using DevExpress.Utils.SystemDrawingConversions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Camera;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using Pikda.Dtos;
using Pikda.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Pikda
{
    public partial class OcrScannerForm : DevExpress.XtraEditors.XtraForm
    {
        #region State

        readonly OcrService ocrService;
        readonly OcrRepository ocrRepository;
        
        List<AreaViewDto> AreaViewDtos
        {
            get
            {
                if (currentOcrModel == null) return new List<AreaViewDto>();
                var l = currentOcrModel.Areas.Select(s => new AreaViewDto
                {
                    Prop = s.Name,
                    Value = s.Value
                }).ToList();

                if(AreasViewGrid != null)
                    AreasViewGrid.DataSource = l;

                return l;
            }
        } 
        List<OcrModel> ocrModels = new List<OcrModel>();

        OcrModel currentOcrModel { get;set; }

   
        //private Image Image => PictureEditor.Image;

        #endregion

        public OcrScannerForm(int modelId)
        {
            this.ocrService = new OcrService();
            this.ocrRepository = new OcrRepository();
   
            ocrModels = ocrRepository.GetOcrModels();
            currentOcrModel = ocrModels.FirstOrDefault(o => o.Id == modelId);
            InitializeComponent();

            InitializeAreasView();

            

            var logitech = CameraControl.GetDevices().FirstOrDefault(x => x.Name.Contains("Webcam"));
            if (logitech != null)
            {
                cameraControl1.Device = CameraControl.GetDevice(logitech);
            }
        }


        #region Behaviours
       
        void InitializeAreasView()
        {
            AreasViewGrid.DataSource = AreaViewDtos;

            GridView view = AreasViewGrid.MainView as GridView;

            var propCol = view.Columns["Prop"];
            var valCol = view.Columns["Value"];

            var propsLookUp = new RepositoryItemLookUpEdit();
            propsLookUp.DataSource = new string[] { "FirstName", "LastName", "BirthDay" };
            AreasViewGrid.RepositoryItems.Add(propsLookUp);
            propCol.ColumnEdit = propsLookUp;
        }
       

        private List<AreaViewDto> GetAreas()
        {
            if (currentOcrModel.Areas == null) return new List<AreaViewDto>();
            return currentOcrModel.Areas.Select(s => new AreaViewDto
            {
                Prop = s.Name,
                Value = s.Value
            }).ToList();
        }

        #endregion

        #region Drawing

        private void PictureEdit_Paint(object sender, PaintEventArgs e)
        {

            ImageBorder = CalcImageBorder();
            ReCalcRectangles();

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);

            foreach (var rect in Rectangles)
            {
                Console.WriteLine($"rect w : {rect.Item1.Width}, h: {rect.Item1.Height}, x:{rect.Item1.X}, y:{rect.Item1.Y}");
                g.DrawRectangle(pen, rect.Item1);
            }

            if (CurrentRect != UnDefinedRect)
                g.DrawRectangle(pen, CurrentRect);

            pen.Dispose();
            ResumeLayout(true);

            void ReCalcRectangles()
            {

                var areas = currentOcrModel.Areas;
                Rectangles = areas.Select(a => GetRectFromAreaDto(ImageBorder, a)).ToList();

            }

            (Rectangle, string) GetRectFromAreaDto(Rectangle border, Area area)
            {
                var rect = area.ToRectangle(border);

                return (
                        new Rectangle
                        (
                            x: rect.X + (int)(((float)(cameraControl1.Width - border.Width)) / 2),
                            y: rect.Y + (int)(((float)(cameraControl1.Height - border.Height)) / 2),
                            width: rect.Width,
                            height: rect.Height
                        ),
                        area.Name
                    );
            }

        }

        private Rectangle CalcImageBorder()
        {
            var s = cameraControl1.Size;
            return new Rectangle(0,0,s.Width,s.Height);
        }

        private readonly Panel _picturePanel;

        private List<(Rectangle, string)> Rectangles = new List<(Rectangle, string)>();
        private Rectangle CurrentRect;
        private static readonly Rectangle UnDefinedRect = new Rectangle(-404, -404, -404, -404);

        private Rectangle PrevImageBorder = UnDefinedRect;
        private Rectangle _imageBorder = UnDefinedRect;
        private Rectangle ImageBorder
        {
            get
            {
                return _imageBorder;
            }
            set
            {
                if (PrevImageBorder == UnDefinedRect || _imageBorder == UnDefinedRect)
                {
                    PrevImageBorder = _imageBorder = value;
                    return;
                }

                PrevImageBorder = _imageBorder;
                _imageBorder = value;
            }
        }

        private static readonly Point UnDefinedPoint = new Point(-404, -404);
        private static readonly string UnDefinedName = "Not Named";
        private Point StartPoint = UnDefinedPoint;
        private Point EndPoint;

        #endregion


    }
}
