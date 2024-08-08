using Pikda.Models;
using Pikda.OcrServiceIntegration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pikda
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_ScanCard_Click(object sender, EventArgs e)
        {

            Console.WriteLine("cmb item => " + comboBox1.SelectedItem.ToString());
            var id = wow.FirstOrDefault(w => w.Name == comboBox1.SelectedItem.ToString()).Id;

            var ocrDevice = new OcrDevice();
            var ocrObject = ocrDevice.ReadCard(id);
            
            Console.WriteLine("\n -> card number   : " + ocrObject.CardNumber);
            Console.WriteLine("\n -> birth date    : " + ocrObject.BirthDate);
            Console.WriteLine("\n -> image: " + ocrObject.Image.Width + "x" + ocrObject.Image.Height);
            Console.WriteLine("\n -> first name: " + ocrObject.FirstName);
            Console.WriteLine("\n -> last name: " + ocrObject.LastName);
            Console.WriteLine("\n -> error message : " + ocrObject.ErrorMessage);

        }

        private void btn_AddModel_Click(object sender, EventArgs e)
        {
            //var ocrModel = new OCR_Form(new OcrService(),new OcrRepository());

            var ocrDevice = new OcrDevice();
            var id = ocrDevice.CreatModel();

            if(id == 0)
            {
                Console.WriteLine("Faild to create ocr model");
                return;
            }
            

            var db = new AppDbContext();
            wow = db.OcrModels.ToList();

            var newOcr = wow.FirstOrDefault(m => m.Id == id);
            //if (newOcr == null)
            //    throw new Exception("Something when wrong");

            comboBox1.Items.AddRange(wow.Select(w => w.Name).ToArray());
        }

        List<OcrModel> wow;
        private void MainForm_Load(object sender, EventArgs e)
        {
            var db = new AppDbContext();
            wow = db.OcrModels.ToList();
            if(wow.Count > 0)
            {
            comboBox1.Items.AddRange(wow.Select(w=>w.Name).ToArray());
                comboBox1.SelectedIndex = 0;
            }
            
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            var db = new AppDbContext();
            wow = db.OcrModels.ToList();
            comboBox1.Items.AddRange(wow.Select(w => w.Name).ToArray());

        }
    }
}
