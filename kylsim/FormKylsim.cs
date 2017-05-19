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
        Node node1 = new Node("N1", 100, 200, 10, 10, 5, false);
        Node node2 = new Node("N2", 300, 200, 10, 10, 1, true);
        Node node3 = new Node("N3", 500, 200, 10, 10, 1, false);

        //Create valve
        Valve valve1 = new Valve("V1", 200, 200, 10, 10, 1, 10, null, null);
        Valve valve2 = new Valve("V2", 400, 200, 10, 10, 1, 10, null,null);

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
            // Set valve nodeIn, NodeOut
            valve1.NodeIn = node1;
            valve1.NodeOut = node2;
            valve2.NodeIn = node2;
            valve2.NodeOut = node3;

            // Draw nodes
            node1.Draw(canvas, Brush, Font, ComponentPen);
            node2.Draw(canvas, Brush, Font, ComponentPen);
            node3.Draw(canvas, Brush, Font, ComponentPen);

            // Draw valve
            valve1.Draw(canvas, Brush, Font, ComponentPen, LinePen);
            
            valve2.Draw(canvas, Brush, Font, ComponentPen, LinePen);
            
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
            valve1.Display(canvas, Brush, Font);
            valve2.Display(canvas, Brush, Font);
            valve1.Dynamics();
            valve2.Dynamics();
            node1.Dynamics();
            node2.Dynamics();
            node3.Dynamics();

        }

        private void FormKylsim_Load(object sender, EventArgs e)
        {

        }
    }
}
