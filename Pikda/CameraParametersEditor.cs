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
    public partial class CameraParametersEditor : Form
    {
        public CameraParametersEditor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var json = richTextBox1.Text;

            CameraParameters.SetFromJson(json);
        }
    }
}
