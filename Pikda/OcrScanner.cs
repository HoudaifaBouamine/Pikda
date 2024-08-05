using DevExpress.Data.Camera;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Camera;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using Pikda.Dtos;
using Pikda.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VisioForge.Core.VideoCapture;
using VisioForge.Libs.DirectShowLib;

namespace Pikda
{
    public partial class OcrScannerForm : DevExpress.XtraEditors.XtraForm
    {
        #region State

        readonly OcrService ocrService;
        readonly OcrRepository ocrRepository;
        VideoCaptureCore VideoCaptureCore;
        private Image Image { get => camera.TakeSnapshot(); }
        List<AreaViewDto> AreaViewDtos
        {
            get
            {
                if (currentOcrModel == null) return new List<AreaViewDto>();
                var l = currentOcrModel.Areas.Select(s => new AreaViewDto
                {
                    Prop = s.Name,
                    Value = s.Value,
                    PlaceHolder = s.Placeholder,
                    Language = s.Language
                }).ToList();

                if (AreasViewGrid != null)
                    AreasViewGrid.DataSource = l;

                return l;
            }
        }
        List<OcrModel> ocrModels = new List<OcrModel>();

        OcrModel currentOcrModel { get; set; }

        public OcrScannerForm()
        {
            this.ocrService = new OcrService();
            this.ocrRepository = new OcrRepository();

            ocrModels = ocrRepository.GetOcrModels();

            
            InitializeComponent();

            InitializeAreasView();

            var logitech = CameraControl.GetDevices().FirstOrDefault(x => x.Name.Contains("Webcam"));
            if (logitech != null)
            {
                camera.Device = CameraControl.GetDevice(logitech);
            }

        }

        #endregion

        public OcrScannerForm(int modelId)
        {
            this.ocrService = new OcrService();
            this.ocrRepository = new OcrRepository();

            ocrModels = ocrRepository.GetOcrModels();
            currentOcrModel = ocrModels.FirstOrDefault(o => o.Id == modelId);

            if (currentOcrModel == null)
            {
                XtraInputBoxArgs args = new XtraInputBoxArgs();

                ComboBoxEdit cbEdit = new ComboBoxEdit();
                cbEdit.Properties.Items.AddRange(Props2);

                args.Caption = "Enter new model name";
                args.Prompt = "Model Name";
                args.DefaultButtonIndex = 0;

                args.Editor = cbEdit;

                var created = false;
                while (!created)
                {
                    string name = (string)XtraInputBox.Show(args);

                    if (string.IsNullOrEmpty(name))
                    {
                        MessageBox.Show($"Model name can not be empty", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ocrModels.FirstOrDefault(o=>o.Name == name) != null)
                    {
                        MessageBox.Show($"Model name already exist", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else {

                        currentOcrModel = new OcrModel(name);
                        ocrRepository.CreateOcrModelAsync(currentOcrModel).Wait();

                    }
                }
            }
            InitializeComponent();

            InitializeAreasView();

            var logitech = CameraControl.GetDevices().FirstOrDefault(x => x.Name.Contains("Webcam"));
            if (logitech != null)
            {
                camera.Device = CameraControl.GetDevice(logitech);
            }

        }

        private IAMCameraControl cameraControl;
        private IAMVideoProcAmp videoProcAmp;
        
        private void InitializeCameraSettings(CameraDevice device)
        {
            // Initialize DirectShow interfaces
            InitializeDirectShowInterfaces(device);

            // Set initial values for focus, brightness, and sharpness
            SetCameraProperty(CameraControlProperty.Focus, 160); // Example value
            
            device.Resolution = new Size(864, 480);
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
            //if (camera.Device != null && camera.Device.IsRunning)
            //    ShowNativeCameraSettings(camera.Device, this);
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

        #region Draw
        private void PictureEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentOcrModel == null) return;
            if (e.Button != MouseButtons.Left || Image is null)
                return;

            if (!ImageBorder.Contains(e.Location))
                return;

            StartPoint = e.Location;
            CurrentRect = new Rectangle(e.Location, new Size(0, 0));
        }

        private void PictureEdit_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentOcrModel == null) return;
            if (StartPoint == UnDefinedPoint) return;

            if (e.Button != MouseButtons.Left || Image is null)
                return;

            EndPoint = e.Location;

            CurrentRect = new Rectangle
                (
                    Math.Min(StartPoint.X, EndPoint.X),
                    Math.Min(StartPoint.Y, EndPoint.Y),
                    Math.Abs(StartPoint.X - EndPoint.X),
                    Math.Abs(StartPoint.Y - EndPoint.Y)
                );
            CurrentRect.Intersect(ImageBorder);

            //PictureEditor.Invalidate();
        }

        
        
        private void PictureEdit_MouseUp(object sender, MouseEventArgs e)
        {
            if (StartPoint == UnDefinedPoint) return;

            if (e.Button != MouseButtons.Left || Image is null)
                return;

            if (CurrentRect.Width * CurrentRect.Height == 0)
                return;

            // Add the current rectangle to the list
            CurrentRect.Intersect(ImageBorder);

            XtraInputBoxArgs args = new XtraInputBoxArgs();

            ComboBoxEdit cbEdit = new ComboBoxEdit();
            cbEdit.Properties.Items.AddRange(Props);

            args.Caption = "Adding New Property";
            args.Prompt = "Property Name";
            args.DefaultButtonIndex = 0;

            args.Editor = cbEdit;

            string result = (string)XtraInputBox.Show(args);

            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Can not create rectangle without prop's name", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                CurrentRect = UnDefinedRect;
                return;
            }

            Props.Remove(result);

            var newArea = GetAreaDtoFromRect(ImageBorder, result, CurrentRect);

            SetDefaultLanguageAndPlaceHoldersForIdCard(newArea);

            var rect = newArea.ToRectangle(new Rectangle(new Point(0, 0), Image.Size));


            Image subImage = ((Bitmap) Image).Clone(rect, Image.PixelFormat);
            var ocrText = ocrService.Process(subImage, newArea.Language);
    

            //subImage.Save("../../Images/" + Guid.NewGuid() + ".jpg");

            //Console.WriteLine($"\nImage width : {Image.Width}, height : {Image.Height}");
            //Console.WriteLine($"\nCamera width : {camera.Device.Resolution.Width}, height : {camera.Device.Resolution.Height}");
            //Console.WriteLine($"\nImage border x:{ImageBorder.X} y:{ImageBorder.Y} w: {ImageBorder.Width}, h : {ImageBorder.Height}");
            //Console.WriteLine($"\nReal Rect x:{rect.X} y:{rect.Y} w:{rect.Width} h:{rect.Height}");
            //Console.WriteLine($"\nShown Rect x:{CurrentRect.X} y:{CurrentRect.Y} w:{CurrentRect.Width} h:{CurrentRect.Height}");

            newArea.Value = ocrText;
            currentOcrModel.AddArea(newArea);

            UpdateOcrModelInDb();

            Rectangles.Add((CurrentRect, result));

            AreasViewGrid.DataSource = GetAreas();
       
            StartPoint = UnDefinedPoint;
            CurrentRect = UnDefinedRect;

            
            AreasViewGrid.Invalidate();
        }

        void SetDefaultLanguageAndPlaceHoldersForIdCard(Area area)
        {
                    //private string[] Props { get; set; } = new string[] { "FirstName", "LastName", "BirthDay", "CardNumber", "Gender", "BloadType", "Image" };

            switch (area.Name)
            {
                case "CardNumber":
                    area.Language = "ara";
                    area.Placeholder = "رقم التعريف الوطني";
                break;

                case "FirstName":
                    area.Language = "eng";
                    area.Placeholder = "Prénom(s)";
                break;

                case "LastName":
                    area.Language = "eng";
                    area.Placeholder = "Nom";
                break;

                case "BirthDay":
                    area.Language = "ara";
                    area.Placeholder = "تاريخ الميلاد";
                break;

                case "BloadType":
                    area.Language = "eng";
                    area.Placeholder = "Rh";
                break;

                case "Gender":
                    area.Language = "ara";
                    area.Placeholder = "الجنس";
                break;
            }
        }

        Area GetAreaDtoFromRect(Rectangle border, string name, Rectangle r)
        {
            r = new Rectangle
            (
                    x: r.X - (int)(((float)(camera.Width - border.Width + 1)) / 2),
                    y: r.Y - (int)(((float)(camera.Height - border.Height + 1)) / 2),
                    width: r.Width,
                    height: r.Height
            );

            return new Area(name, border, r);
        }

        #endregion

        #region Behaviours

        void UpdateOcrModelInDb()
        {
            var areaView = AreasViewGrid.DataSource as List<AreaViewDto>;

            currentOcrModel.Areas.ForEach(a =>
            {
                var areaRow = areaView.FirstOrDefault(b => b.Prop == a.Name);
                if (areaRow is null) return;

                a.Placeholder = areaRow.PlaceHolder;
                a.Language = areaRow.Language;
            });

            ocrRepository.UpdateOcrModel(currentOcrModel);
        }

        void InitializeAreasView()
        {
            AreasViewGrid.DataSource = AreaViewDtos;

            GridView view = AreasViewGrid.MainView as GridView;

            var propCol = view.Columns["Prop"];
            var valCol = view.Columns["Value"];
            var langCol = view.Columns["Language"];

            var propsLookUp = new RepositoryItemLookUpEdit();
            propsLookUp.DataSource = Props2;
            AreasViewGrid.RepositoryItems.Add(propsLookUp);
            
            propCol.ColumnEdit = propsLookUp;
            propsLookUp.ReadOnly = true;

            var langsLookUp = new RepositoryItemLookUpEdit();
            langsLookUp.DataSource = Languages;
            AreasViewGrid.RepositoryItems.Add(langsLookUp);
            langCol.ColumnEdit = langsLookUp;

            langsLookUp.SelectionChanged += LangsLookUp_SelectionChanged;
        }

        private void LangsLookUp_SelectionChanged(object sender, DevExpress.XtraEditors.Controls.PopupSelectionChangedEventArgs e)
        {
            var wow = (AreaViewDto)AreasViewGrid.ViewCollection[e.RecordIndex].DataSource;
            Console.WriteLine("wow => " + wow.Prop);
            
            var name = wow.Prop;

            var area = currentOcrModel.Areas.Where(a => a.Name == name).First();
            area.Language = wow.Language;
        }

        private List<string> Props { get; set; } = new List<string> { "FirstName", "LastName", "BirthDay", "CardNumber", "Gender", "BloadType", "Image" };
        private List<string> Props2 { get; set; } = new List<string> { "FirstName", "LastName", "BirthDay", "CardNumber", "Gender", "BloadType", "Image" };
        private string[] Languages = new string[] { "ara", "eng" };

        private List<AreaViewDto> GetAreas()
        {
            if (currentOcrModel.Areas == null) return new List<AreaViewDto>();
            return currentOcrModel.Areas.Select(s => new AreaViewDto
            {
                Prop = s.Name,
                Value = s.Value,
                PlaceHolder = s.Placeholder,
                Language = s.Language
            }).ToList();
        }
        
        #endregion

        #region Drawing

        private void PictureEdit_Paint(object sender, PaintEventArgs e)
        {
            if(currentOcrModel == null) return;
            ImageBorder = CalcImageBorder();
            ReCalcRectangles();

            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);

            foreach (var rect in Rectangles)
            {
                //Console.WriteLine($"rect w : {rect.Item1.Width}, h: {rect.Item1.Height}, x:{rect.Item1.X}, y:{rect.Item1.Y}");
                g.DrawRectangle(pen, rect.Item1);
            }

            if (CurrentRect != UnDefinedRect)
                g.DrawRectangle(pen, CurrentRect);

            pen = new Pen(Color.Red);
            g.DrawRectangle(pen, ImageBorder);

            pen.Dispose();
            ResumeLayout(true);

            void ReCalcRectangles()
            {
                var areas = currentOcrModel.Areas;
                Rectangles = areas.Select(a => GetRectFromAreaDto(ImageBorder, a)).ToList();
            }

        }
        (Rectangle, string) GetRectFromAreaDto(Rectangle border, Area area)
        {
            var rect = area.ToRectangle(border);

            return (
                new Rectangle
                (
                    x: rect.X + (int)(((float)(camera.Width - border.Width + 1)) / 2),
                    y: rect.Y + (int)(((float)(camera.Height - border.Height + 1)) / 2),
                    width: rect.Width,
                    height: rect.Height
                ),
                area.Name
            );
        }

        private Rectangle CalcImageBorder()
        {
            var s_w = camera.Size.Width;
            var s_h = camera.Size.Height;
            var c_w = camera.Device.Resolution.Width;
            var c_h = camera.Device.Resolution.Height;

            var c_ratio = ((float)c_w) / c_h;
            var s_ratio = ((float)s_w) / s_h;

            
            
            if(c_ratio > s_ratio)
            {
                var cc_w = s_w;
                var cc_h = (int) (((float)cc_w)/c_ratio);
                return new Rectangle(0, (s_h - cc_h) /2, cc_w, cc_h);
            }
            else if (c_ratio < s_ratio)
            {
                var cc_h = s_h;
                var cc_w = (int)(cc_h * c_ratio);
                return new Rectangle((s_w-cc_w)/2, 0, cc_w, cc_h);
            }

            return new Rectangle(0,0,s_w,s_h);

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
            if (currentOcrModel == null) return;

            var image = (Image)camera.TakeSnapshot();
            var text = ocrService.Process(image ,"ara");
            System.IO.File.AppendAllText("../../Test.txt", "\n\n\n =>=> wow text is :\n\n" + text);

            //foreach (var a in currentOcrModel.Areas)
            //{
            //    var r = GetRectFromAreaDto(new Rectangle(new Point(0, 0), image.Size),a);

            //    var name = ocrService.Process(image, r.Item1, "ara");
            //    System.IO.File.AppendAllText("../../Test-Rect.txt", $" => \nimage size x:{image.Size.Width} h:{image.Size.Height}\nx:{r.Item1.X} y:{r.Item1.Y} w:{r.Item1.Width} h:{r.Item1.Height}" + text);
            //}

            //System.IO.File.AppendAllText("../../Test-Rect.txt", "\n\n\n" + text);

        }

        private void OcrScannerForm_Load(object sender, EventArgs e)
        {
            InitializeCameraSettings(camera.Device);

        }

        private async void OcrScannerForm_Paint(object sender, PaintEventArgs e)
        {
            if (currentOcrModel == null)
            {
                XtraInputBoxArgs args = new XtraInputBoxArgs();

                args.Caption = "Adding New Property";
                args.Prompt = "Property Name";
                args.DefaultButtonIndex = 0;

                args.Editor = new TextEdit();


                var created = false;
                while (!created)
                {
                    string name = (string)XtraInputBox.Show(args);

                    Console.WriteLine("\n\nshowen , name is " + name + "\n\n");

                    if (string.IsNullOrEmpty(name))
                    {
                        MessageBox.Show($"Model name can not be empty", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ocrModels.FirstOrDefault(o => o.Name == name) != null)
                    {
                        MessageBox.Show($"Model name already exist", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {

                        currentOcrModel = new OcrModel(name);
                        await ocrRepository.CreateOcrModelAsync(currentOcrModel);
                        created = true;
                    }


                    Console.WriteLine("wow");
                }
            }
        }

        private void ClosingFun(object sender, EventArgs e)
        {
            UpdateOcrModelInDb();
        }

        private void LayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AreasViewGrid_Click(object sender, EventArgs e)
        {

        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            var image = Image;


            
            UpdateOcrModelInDb();
            currentOcrModel.Areas.ForEach(a =>
            {
                var rect = a.ToRectangle(new Rectangle(new Point(0, 0), Image.Size));

                Image subImage = ((Bitmap)image).Clone(rect, Image.PixelFormat);

                a.Value = ocrService.Process(subImage, a.Language);
            });
            InitializeAreasView();
        }
    }
}
