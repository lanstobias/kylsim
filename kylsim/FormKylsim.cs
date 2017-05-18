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

        Node node1 = new Node("Test nod", 10, 10, 10, 10, null, null, null, 4, false);

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
            // Create pen
            Pen pen = new Pen(Color.Red);

            node1.Draw(canvas);
            node1.Display(canvas);

            // Draw line
            canvas.DrawLine(pen, 100, 100, 200, 200);
        }
    }
}
