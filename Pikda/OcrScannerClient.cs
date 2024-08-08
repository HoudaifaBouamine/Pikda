using DevExpress.Data.Camera;
using DevExpress.Data.Helpers;
using DevExpress.Utils.Extensions;
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using VisioForge.Core.VideoCapture;
using VisioForge.Libs.DirectShowLib;
using VisioForge.Libs.MediaFoundation.OPM;
using VisioForge.Libs.ZXing;

namespace Pikda
{
    public partial class OcrScannerClientForm : DevExpress.XtraEditors.XtraForm
    {
        #region State

        readonly OcrService ocrService = new OcrService();
        readonly OcrRepository ocrRepository = new OcrRepository();
        VideoCaptureCore VideoCaptureCore;
        private Image Image { get => camera.TakeSnapshot(); }

        List<AreaViewClientDto> AreaViewDtos
        {
            get
            {
                if (currentOcrModel == null) return new List<AreaViewClientDto>();
                var l = currentOcrModel.Areas.Select(s => new AreaViewClientDto
                {
                    Prop = s.Name,
                    Value = s.Value,
                }).ToList();

                if (AreasViewGrid != null)
                    AreasViewGrid.DataSource = l;

                return l;
            }
        }
        //List<AreaViewClientDto> AreaViewDtos
        //{
        //    get
        //    {
        //        Console.WriteLine("wowooowowowo");
        //        if (list != null && Loading == false) return list;

        //        if (currentOcrModel == null || ocrService is null || camera is null) return new List<AreaViewClientDto>();

        //        list = currentOcrModel.Areas.Select(s =>
        //        {
        //            var cameraIsLoading = camera.TakeSnapshot() == null;
        //            Loading = cameraIsLoading && (StartTime == null || DateTime.Now < StartTime + TimeSpan.FromMilliseconds(100));

        //            var area = new AreaViewClientDto
        //            {
        //                Prop = s.Name,
        //                Value = Loading ? "Loading..." : GetTextFromRect(s),
        //                IsImage = "Image" == s.Name,

        //            };


        //            //area.SetPlaceholder(s.Placeholder);

        //            return area;

        //        }).ToList();

        //        if (AreasViewGrid != null)
        //            AreasViewGrid.DataSource = list;

        //        return list;
        //    }
        //}
        List<OcrModel> ocrModels = new List<OcrModel>();

        OcrModel currentOcrModel { get; set; }

        #endregion

        public OcrScannerClientForm(int modelId)
        {
            

            ocrModels = ocrRepository.GetOcrModels();
            currentOcrModel = ocrModels.FirstOrDefault(o => o.Id == modelId);

            if (currentOcrModel == null)
            {
                XtraInputBoxArgs args = new XtraInputBoxArgs();

                ComboBoxEdit cbEdit = new ComboBoxEdit();
                cbEdit.Properties.Items.AddRange(Props);

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

            InitializeCameraSettings(camera.Device);
        }

        private IAMCameraControl cameraControl;
        private IAMVideoProcAmp videoProcAmp;
        
        private void InitializeCameraSettings(CameraDevice device)
        {
            // Initialize DirectShow interfaces
            InitializeDirectShowInterfaces(device);

            // Set initial values for focus, brightness, and sharpness
            SetCameraProperty(CameraControlProperty.Focus, 160); // Example value

            SetVideoProcAmpProperty(VideoProcAmpProperty.Brightness, 215); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Sharpness, 150); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Saturation, 0); // Example value
            SetVideoProcAmpProperty(VideoProcAmpProperty.Contrast, 255); // Example value
            device.Resolution = new Size(1280, 720);
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


        #region Behaviours

        void UpdateOcrModelInDb()
        {
            var areaView = AreasViewGrid.DataSource as List<AreaViewClientDto>;

            if (areaView == null) return;

            currentOcrModel.Areas.ForEach(a =>
            {
                var areaRow = areaView.FirstOrDefault(b => b.Prop == a.Name);
                if (areaRow is null) return;

            });

            ocrRepository.UpdateOcrModel(currentOcrModel);
        }

        void InitializeAreasView()
        {
            AreasViewGrid.DataSource = AreaViewDtos;

            GridView view = AreasViewGrid.MainView as GridView;

            var propCol = view.Columns["Prop"];
            var valCol = view.Columns["Value"];

            var propsLookUp = new RepositoryItemLookUpEdit();
            propsLookUp.DataSource = Props;
            propsLookUp.ReadOnly = true;

            AreasViewGrid.RepositoryItems.Add(propsLookUp);
            propCol.ColumnEdit = propsLookUp;
        }

        private string[] Props = new string[] { "FirstName", "LastName", "BirthDay","CardNumber", "Gender", "BloadType", "Image" };

        private List<AreaViewClientDto> GetAreas()
        {
            if (currentOcrModel.Areas == null) return new List<AreaViewClientDto>();
            return currentOcrModel.Areas.Select(s => new AreaViewClientDto
            {
                Prop = s.Name,
                Value = s.Value,
                
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


        private void OcrScannerForm_Load(object sender, EventArgs e)
        {
            //InitializeCameraSettings(camera.Device);

        }

        DateTime? StartTime = null;
        private async void OcrScannerForm_Paint(object sender, PaintEventArgs e)
        {
            StartTime = DateTime.Now;

            


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
            //UpdateOcrModelInDb();
            Image image = Image;
            string error = "";

            var area = currentOcrModel.Areas.FirstOrDefault(a => a.Name == "Image");

            if (area != null)
            {
                var rect = area.ToRectangle(new Rectangle(new Point(0, 0), image.Size));
                image = ((Bitmap)image).Clone(rect, image.PixelFormat);
            }

            DateTime birthDate;

            try
            {
                birthDate = GetBirthDateFromIdCard();
                if
                    (
                        birthDate == null ||
                        birthDate == default ||
                        DateTime.Now.Subtract(new TimeSpan(100 * 365,0,0,0)) > birthDate ||
                        DateTime.Now < birthDate
                    )
                {
                    error += "\nBirth Date faild to be parsed";
                }

            }
            catch (Exception ex) 
            {
                error += "\n"  +ex.Message;
                birthDate = default;
            }

            OcrObject = new OcrObject
            {
                FirstName = AreaViewDtos.Where(a => a.Prop == "FirstName").FirstOrDefault()?.Value,
                LastName = AreaViewDtos.Where(a => a.Prop == "LastName").FirstOrDefault()?.Value,
                BirthDate = birthDate,
                BloodType = GetBloodTypeFromIdCard(),
                CardNumber = AreaViewDtos.Where(a => a.Prop == "CardNumber").FirstOrDefault()?.Value,
                Gender = GetGenderFromIdCard(),
                Image = image,
                ErrorMessage = error,
                Result = true
            };

            Gender GetGenderFromIdCard()
            {
                var genderAsString = AreaViewDtos.Where(a => a.Prop == "Gender").FirstOrDefault()?.Value;

                switch (genderAsString)
                {
                    case "ذكر":
                        return Gender.Male;
                    case "أنثى":
                        return Gender.Female;
                    
                    default:
                        return Gender.NotDefined;
                }
            }

            BloodType GetBloodTypeFromIdCard()
            {
                var bloodTypeAsString = AreaViewDtos.Where(a => a.Prop == "BloodType").FirstOrDefault()?.Value;

                switch (bloodTypeAsString)
                {
                    case "A+":
                        return BloodType.APositive;

                    case "A-":
                        return BloodType.ANegative;

                    case "B+":
                        return BloodType.BPositive;

                    case "B-":
                        return BloodType.BNegative;

                    case "AB+":
                        return BloodType.AbPositive;

                    case "AB-":
                        return BloodType.AbNegative;

                    case "O+":
                        return BloodType.OPositive;

                    case "O-":
                        return BloodType.ONegative;

                    default:
                        return BloodType.NotDefined;
                }
            }

            DateTime GetBirthDateFromIdCard()
            {
                var parts = AreaViewDtos.Where(a => a.Prop == "BirthDay").FirstOrDefault()?.Value.Split('.');
                if (parts == null) return default;
                return new DateTime
                    (
                        year: Convert.ToInt32(parts[0]),
                        month: Convert.ToInt32(parts[1]),
                        day: Convert.ToInt32(parts[2])
                    );
            }
        }


        public OcrObject OcrObject { get; set; }


        private void OcrScannerClientForm_Shown(object sender, EventArgs e)
        {



            UpdateOcrModelInDb();
            //UpdateAreas();
            InitializeAreasView();
            
        }

        void UpdateAreas()
        {

            var image = Image;
            currentOcrModel.Areas.ForEach(a =>
            {

                if (a.Name != "Image")
                {
                    var rect = a.ToRectangle(new Rectangle(new Point(0, 0), image.Size));

                    Image subImage = ((Bitmap)image).Clone(rect, image.PixelFormat);


                    a.Value = ocrService.Process(subImage, a.Language);

                }
            });
        }

        private void LayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_ReRead_Click(object sender, EventArgs e)
        {

            UpdateOcrModelInDb();
            UpdateAreas();
            InitializeAreasView();
        }
    }
}
