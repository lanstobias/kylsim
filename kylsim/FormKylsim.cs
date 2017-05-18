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
        //
        private Graphics canvas;


        Brush brush = new SolidBrush(Color.Black);
        Font font = new Font("Courier", 8);
        Pen ComponentPen = new Pen(Color.Red);
        Pen LinePen = new Pen(Color.Blue);
        Node node1 = new Node("N1", 100, 200, 10, 10, null, null, null, 4, false);
        Node node2 = new Node("N2", 200, 200, 10, 10, null, null, null, 2, false);
        Node node3 = new Node("N3", 300, 200, 10, 10, null, null, null, 7.5, false);
        Valve valve1=new Valve("V1", 400, 200, 10, 10, null, null, null, 7.5, 10);
        public FormKylsim()
        {
            InitializeComponent();

            this.canvas = this.CreateGraphics();
        }

        //
        // Pain händelse
        //        
        /// <summary>
        /// Handles the Paint event of the FormKylsim control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void FormKylsim_Paint(object sender, PaintEventArgs e)
        {
            // Create pen
            node1.Draw(canvas, brush, font, ComponentPen);
            node2.Draw(canvas, brush, font, ComponentPen);
            node3.Draw(canvas, brush, font, ComponentPen);
            valve1.Draw(canvas, brush, font, ComponentPen);

            // Draw line
            canvas.DrawLine(LinePen, 105, 205, 305, 205);
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
            node1.Display(canvas, brush, font);
            node2.Display(canvas, brush, font);
            node3.Display(canvas, brush, font);
           

        }

        private void FormKylsim_Load(object sender, EventArgs e)
        {

        }
    }
}
