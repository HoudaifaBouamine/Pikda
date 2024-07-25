using DevExpress.XtraEditors;
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
    public partial class OCR_Form : DevExpress.XtraEditors.XtraForm
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

        OcrModel _currentOcrModel = null;
        OcrModel currentOcrModel { 
            
            get
            {
                return _currentOcrModel;
            }
            set
            {
                if (_currentOcrModel == value) return;
                _currentOcrModel = value;
                RefreshUI();

                void RefreshUI()
                {
                    if (AreasViewGrid != null)
                        AreasViewGrid.DataSource = GetAreas();

                    if (PictureEditor != null)
                        PictureEditor.Image = currentOcrModel.Image;

                }
            }
        }

        private ListViewItem SelectedItem
        {
            get
            {
                if (CardsTable.SelectedIndices.Count <= 0 || CardsTable.Items.Count <= 0)
                {
                    return null; // TODO : Prevent this when there is at least one Item
                }
                int intselectedindex = CardsTable.SelectedIndices[0];
                if (intselectedindex < 0) return null;

                return CardsTable.Items[intselectedindex];
            }
        }
        private Image Image => PictureEditor.Image;

        #endregion

        public OCR_Form(OcrService ocrService, OcrRepository ocrRepository)
        {
            this.ocrService = ocrService;
            this.ocrRepository = ocrRepository;

            ocrModels = ocrRepository.GetOcrModels();

            InitializeComponent();

            InitializeCardsTable();
            InitializeAreasView();
        }


        #region Behaviours
        void InitializeCardsTable()
        {
            CardsTable.Items.AddRange(ocrModels.Select(o=>new ListViewItem(o.Name)).ToArray());


            if (CardsTable.Items.Count > 0)
            {
                PictureEditor.Enabled = true;
                CardsTable.SelectedIndices.Add(0);
            }
            else
            {
                PictureEditor.Enabled = false;
            }

            var ctx = CardsTable.ContextMenu = new ContextMenu();
            ctx.MenuItems.Add("Add New Card", CardsTable_AddNewCard);

        }
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
        private void CardsTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedItem is null) return;

            var cardName = SelectedItem.Text;

            var card = ocrModels.Where(o => o.Name == cardName).FirstOrDefault();
            if (card == null) throw new Exception("card not exist"); // TODO : Better ux later

            currentOcrModel = card; // this will update the current image
            
        }

        
        private async void PictureEditor_ImageChanged(object sender, EventArgs e)
        {
            if (currentOcrModel != null)
            {
                if(currentOcrModel.Image != null)
                {
                    if(currentOcrModel.Image.Size == Image.Size)
                    {
                        return;
                    }
                }

                currentOcrModel.SetImage(Image);
                ocrRepository.UpdateOcrModel(currentOcrModel);
            }
        }

        async void CardsTable_AddNewCard(object sender, EventArgs e)
        {

            var result = XtraInputBox.Show("Card Name", "Adding New Card",$"Card-{Guid.NewGuid().ToString().Substring(0,4)}");
            if(string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Can not create card without name", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ListViewItem cardItem in CardsTable.Items)
                if (cardItem.Text == result)
                {
                    MessageBox.Show($"Card with the name {result} already exist !", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            PictureEditor.Enabled = true;

            var newOcr = new OcrModel(result);
            await ocrRepository.CreateOcrModelAsync(newOcr);
            Console.WriteLine("Ocr created by repo : " + JsonSerializer.Serialize(newOcr));
            ocrModels.Add(newOcr);
            CardsTable.Items.Add(result);
            currentOcrModel = newOcr;
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

        private void PictureEdit_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || Image is null)
                return;

            if (!ImageBorder.Contains(e.Location))
                return;

            StartPoint = e.Location;
            CurrentRect = new Rectangle(e.Location, new Size(0, 0));
        }

        private void PictureEdit_MouseMove(object sender, MouseEventArgs e)
        {
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

            PictureEditor.Invalidate();
        }

        private async void PictureEdit_MouseUp(object sender, MouseEventArgs e)
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
            cbEdit.Properties.Items.Add("FirstName");
            cbEdit.Properties.Items.Add("LastName");
            cbEdit.Properties.Items.Add("BirthDate");

            args.Caption = "Adding New Property";
            args.Prompt = "Property Name";
            args.DefaultButtonIndex = 0;

            args.Editor = cbEdit;

            string result = (string) XtraInputBox.Show(args);

            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Can not create rectangle without prop's name", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                CurrentRect = UnDefinedRect;
                return;
            }


            var imageRect = new Rectangle(0,0,Image.Width,Image.Height);
            var newArea = GetAreaDtoFromRect(ImageBorder, result, CurrentRect);
            var ocrText = ocrService.Process(currentOcrModel.ImageUrl,newArea.ToRectangle(imageRect), "ara");

            Console.WriteLine($"\nwidth : {Image.Width}, height : {Image.Height}");

            newArea.Value = ocrText;
            currentOcrModel.AddArea(newArea);
            ocrRepository.UpdateOcrModel(currentOcrModel);
            Console.WriteLine("ocr areas count : " + currentOcrModel.Areas.Count());
            Rectangles.Add((CurrentRect, result));

            AreasViewGrid.DataSource = GetAreas();

            StartPoint = UnDefinedPoint;
            CurrentRect = UnDefinedRect;

            Area GetAreaDtoFromRect(Rectangle border, string name, Rectangle rect)
            {
                rect = new Rectangle
                    (
                        x: rect.X - (int)(((float)(PictureEditor.Width - border.Width)) / 2),
                        y: rect.Y - (int)(((float)(PictureEditor.Height - border.Height)) / 2),
                        width: rect.Width,
                        height: rect.Height
                    );

                return new Area(name, border, rect);
            }

            AreasViewGrid.Invalidate();
        }

        private void PictureEdit_Paint(object sender, PaintEventArgs e)
        {
            if (Image is null) return;

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
                            x: rect.X + (int)(((float)(PictureEditor.Width - border.Width)) / 2),
                            y: rect.Y + (int)(((float)(PictureEditor.Height - border.Height)) / 2),
                            width: rect.Width,
                            height: rect.Height
                        ),
                        area.Name
                    );
            }

        }

        private Rectangle CalcImageBorder()
        {

            var wFactor = (double)PictureEditor.Width / PictureEditor.Image.Width;
            var hFactor = (double)PictureEditor.Height / PictureEditor.Image.Height;

            var (minFactor, isWidthMinFactor) = (wFactor < hFactor ? wFactor : hFactor, (wFactor < hFactor));

            return new Rectangle
                (
                    x: isWidthMinFactor ? 0 : (PictureEditor.Width - (int)(PictureEditor.Image.Width * minFactor)) / 2,
                    y: isWidthMinFactor ? (PictureEditor.Height - (int)(PictureEditor.Image.Height * minFactor)) / 2 : 0,
                    width: (int)(PictureEditor.Image.Width * minFactor),
                    height: (int)(PictureEditor.Image.Height * minFactor)
                );
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
