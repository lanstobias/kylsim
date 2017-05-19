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
    /// <summary>
    /// FormKylsim
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FormKylsim : Form
    {
        // Create graphics
        private Graphics canvas;
        Brush Brush      = new SolidBrush(Color.Black);
        Font font        = new Font("Courier", 8);
        Pen ComponentPen = new Pen(Color.Red);
        Pen LinePen      = new Pen(Color.Blue);

        // Create Nodes
        Node node1 = new Node("N1", 100, 200, 10, 10, 4, false);
        Node node2 = new Node("N2", 300, 200, 10, 10, 2, false);
        Node node3 = new Node("N3", 400, 200, 10, 10, 7.5, false);

        /// <summary>
        /// Initializes a new instance of the <see cref="FormKylsim"/> class.
        /// </summary>
        public FormKylsim()
        {
            InitializeComponent();
            this.canvas = this.CreateGraphics();
        }

        /// <summary>
        /// Handles the Paint event of the FormKylsim control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void FormKylsim_Paint(object sender, PaintEventArgs e)
        {
            // Create Valves
            Valve valve1 = new Valve("V1", 200, 200, 10, 10, 7.5, 10, node1, node2);

            // Draw nodes
            node1.Draw(canvas, Brush, Font, ComponentPen);
            node2.Draw(canvas, Brush, Font, ComponentPen);
            node3.Draw(canvas, Brush, Font, ComponentPen);

            // Draw valve
            valve1.Draw(canvas, Brush, Font, ComponentPen, LinePen);
            valve1.Display(canvas, Brush, Font);
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
            node1.Display(canvas, Brush, Font);
            node2.Display(canvas, Brush, Font);
            node3.Display(canvas, Brush, Font);
            
        }

        private void FormKylsim_Load(object sender, EventArgs e)
        {

        }
    }
}
