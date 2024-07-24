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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Forms;
using VisioForge.Core.VideoCapture;
using VisioForge.Libs.DirectShowLib;
using VisioForge.Libs.NDI;

namespace Pikda
{
    public partial class OcrScannerForm : DevExpress.XtraEditors.XtraForm
    {
        #region State

        readonly OcrService ocrService;
        readonly OcrRepository ocrRepository;
        VideoCaptureCore VideoCaptureCore;

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

                if (AreasViewGrid != null)
                    AreasViewGrid.DataSource = l;

                return l;
            }
        }
        List<OcrModel> ocrModels = new List<OcrModel>();

        OcrModel currentOcrModel { get; set; }

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

            InitializeCameraSettings(cameraControl1.Device);
        }

        private IAMCameraControl cameraControl;
        private IAMVideoProcAmp videoProcAmp;
        
        private void InitializeCameraSettings(CameraDevice device)
        {

            // Initialize DirectShow interfaces
            InitializeDirectShowInterfaces(device);

            // Set initial values for focus, brightness, and sharpness
            SetCameraProperty(CameraControlProperty.Focus, 150); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Brightness, 20); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Sharpness, 128); // Example value
        }

        private void InitializeDirectShowInterfaces(CameraDevice device)
        {
            var fieldInfo = typeof(CameraDeviceBase).GetField("sourceFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            var sourceFilter = fieldInfo.GetValue(device);
            var filterGraph = new FilterGraph() as IFilterGraph2;

            filterGraph.AddFilter(sourceFilter as IBaseFilter, "Video Capture");

            cameraControl = sourceFilter as IAMCameraControl;
            videoProcAmp = sourceFilter as IAMVideoProcAmp;
        }


        private void SetCameraProperty(CameraControlProperty property, int value, CameraControlFlags flags = CameraControlFlags.Manual)
        {
            if (cameraControl != null)
            {
                cameraControl.Set(property, value, flags);
            }
        }

        private void SetVideoProcAmpProperty(VideoProcAmpProperty property, int value, VideoProcAmpFlags flags = VideoProcAmpFlags.Manual)
        {
            if (videoProcAmp != null)
            {
                videoProcAmp.Set(property, value, flags);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cameraControl1.Device != null && cameraControl1.Device.IsRunning)
                ShowNativeCameraSettings(cameraControl1.Device, this);
        }

        void ShowNativeCameraSettings(CameraDevice device, Form ownerForm)
        {
            // Set properties programmatically here instead of showing UI
            SetCameraProperty(CameraControlProperty.Focus, 60); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Brightness, 140); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Sharpness, 150); // Example value
        }

        [ComImport, Guid("B196B28B-BAB4-101A-B69C-00AA00341D07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ISpecifyPropertyPages
        {
            [PreserveSig]
            int GetPages(out CAUUID pPages);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CAUUID
        {
            public int cElems;
            public IntPtr pElems;
        }

        internal static class Import
        {
            [DllImport("oleaut32.dll")]
            public static extern int OleCreatePropertyFrame(IntPtr hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int cObjects, [MarshalAs(UnmanagedType.Interface)] ref object ppUnk, int cPages, IntPtr lpPageClsID, int lcid, int dwReserved, IntPtr lpvReserved);
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
            return new Rectangle(0, 0, s.Width, s.Height);
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

        private void cameraControl1_MouseClick(object sender, MouseEventArgs e)
        {
            var image = (Image)cameraControl1.TakeSnapshot();
            var text = ocrService.Process(image, "wow", "ara");
            System.IO.File.AppendAllText("../../Test.txt", "\n\n\n =>=> wow text is :\n\n" + text);
        }
    }
}
