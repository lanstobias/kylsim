using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kylsim
{
    public partial class FormKylsim : Form
    {
        private Graphics canvas;

        public FormKylsim()
        {
            InitializeComponent();

            this.canvas = this.CreateGraphics();
        }

        //
        // Pain händelse
        //
        private void FormKylsim_Paint(object sender, PaintEventArgs e)
        {
            // Skapa penna
            Pen pen = new Pen(Color.Red);

            // Rita med pennan ;DDD
            canvas.DrawLine(pen, 100, 100, 200, 200);
        }
    }
}
