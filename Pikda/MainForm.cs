using Pikda.Models;
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
            var scanner = new OcrScannerClientForm(wow.FirstOrDefault(w=>w.Name == comboBox1.SelectedItem.ToString()).Id);
            scanner.ShowDialog();
            
            scanner.Dispose();
        }

        private void btn_AddModel_Click(object sender, EventArgs e)
        {
            //var ocrModel = new OCR_Form(new OcrService(),new OcrRepository());
            var scanner = new OcrScannerForm();
            scanner.ShowDialog();
            scanner.Dispose();
        }

        List<OcrModel> wow;
        private void MainForm_Load(object sender, EventArgs e)
        {
            var db = new AppDbContext();
            wow = db.OcrModels.ToList();
            comboBox1.Items.AddRange(wow.Select(w=>w.Name).ToArray());
            
        }

    }
}
