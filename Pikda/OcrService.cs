using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;

namespace Pikda
{
    public class OcrService
    {
        public const string folderName = "..\\..\\Images";
        public const string trainedDataFolderName = "..\\..\\tessdata";

        public string Process(string imagePath, Rectangle rect, string lang)
        {
            Console.WriteLine($"--> Image Saved at : {imagePath}");

            var result = _process(imagePath, lang, new Rect(rect.X, rect.Y, rect.Width, rect.Height));

            return string.IsNullOrWhiteSpace(result) ? "Failed To Read" : result;
        }
        public string Process(Image image, string imageName, Rectangle rect, string lang)
        {
            var fileName = imageName;
            var imagePath = Path.Combine(folderName, fileName);

            image.Save(imagePath);

            Console.WriteLine($"--> Image Saved at : {imagePath}");

            var result = _process(imagePath, lang, new Rect(rect.X, rect.Y, rect.Width, rect.Height));

            return string.IsNullOrWhiteSpace(result) ? "Ocr is finished. Return empty" : result;
        }
        public string Process(Image image, string imageName, string lang)
        {
            var fileName = imageName;
            var imagePath = Path.Combine(folderName, fileName);

            image.Save(imagePath);

            Console.WriteLine($"--> Image Saved at : {imagePath}");

            var result = _process(imagePath, lang);

            return string.IsNullOrWhiteSpace(result) ? "Ocr is finished. Return empty" : result;
        }


        private string _process(string imagePath, string lang, Rect rect)
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            string result = "";

            using (var engine = new TesseractEngine(tessPath, lang, EngineMode.Default))
            {
                var img = Pix.LoadFromFile(imagePath);

                img = img.ConvertTo8(0);
                img = img.BinarizeSauvola(50,0.35f,false);
                
                //var btmFromPix = PixConverter.ToBitmap(img);

                //using (var fileStream = new FileStream(imagePath, FileMode.Create))
                //{
                //    btmFromPix.Save(Path.Combine("..", "..", "preImages", Guid.NewGuid() + ".jpg"));
                //}

                Page page = default;

                try
                {
                    page = engine.Process(img, rect);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    return "error : " + result;
                }
                result = page.GetText();
                return result;
            }
        }

        public string Process (Image image, Rectangle rect, string lang)
        {
            var result = _process(image, lang, new Rect(rect.X, rect.Y, rect.Width, rect.Height));

            return string.IsNullOrWhiteSpace(result) ? "OCR Failed To Read Rectangle" : result;
        }
        public string Process(Image image, string lang)
        {
            var result = _process(image,lang);

            return string.IsNullOrWhiteSpace(result) ? "OCR Failed To Read Rectangle" : result;
        }

        private string _process(Image image, string lang)
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            string result = "";

            using (var engine = new TesseractEngine(tessPath, lang, EngineMode.Default))
            {
                var img = Pix.LoadFromMemory(ImageToByteArray(image));

                //img = img.ConvertTo8(0);
                //img = img.BinarizeSauvola(16, 0.35f, false);

                Page page = default;

                try
                {
                    page = engine.Process(img);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    return "error : " + result;
                }
                result = page.GetText();
                return result;
            }

        }
        private string _process(Image image, string lang, Rect rect)
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            string result = "";

            using (var engine = new TesseractEngine(tessPath, lang, EngineMode.Default))
            {
                var img = Pix.LoadFromMemory(ImageToByteArray(image));

                img = img.ConvertTo8(0);
                img = img.BinarizeSauvola(50, 0.35f, false);

                Page page = default;

                try
                {
                    page = engine.Process(img, rect);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    return "error : " + result;
                }
                result = page.GetText();
                return result;
            }

        }
        private static byte[] ImageToByteArray(Image imageIn)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(imageIn, typeof(byte[]));
            return xByte;
        }
        private string _process(string imagePath, string lang)
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            string result = "";

            using (var engine = new TesseractEngine(tessPath, lang, EngineMode.Default))
            {
                var img = Pix.LoadFromFile(imagePath);


                img = img.ConvertTo8(0);// 0 to not be collermaped (required by BinarizeSauvolaTiled), 1 to grayscale image  
                img = img.BinarizeSauvolaTiled(50, 0.35f, 1, 1);

                //var btmFromPix = PixConverter.ToBitmap(img);

                //using (var fileStream = new FileStream(imagePath, FileMode.Create))
                //{
                //    btmFromPix.Save("../../preImages/" + Guid.NewGuid() + ".jpg");
                //}

                var page = engine.Process(img);

                result = page.GetText();
            }

            return result;
        }
    }
}
