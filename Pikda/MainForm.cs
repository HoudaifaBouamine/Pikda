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
            var scanner = new OcrScannerClientForm(9);
            scanner.ShowDialog();
        }

        private void btn_AddModel_Click(object sender, EventArgs e)
        {
            //var ocrModel = new OCR_Form(new OcrService(),new OcrRepository());
            var scanner = new OcrScannerForm();
            scanner.ShowDialog();
            scanner.Dispose();
        }
    }
}
