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
        private Kylsim kylsim = new Kylsim();
        private Graphics canvas;

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
            kylsim.Draw(canvas);
            kylsim.Dynamics();
            kylsim.Display(canvas);
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Handles the Load event of the FormKylsim control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FormKylsim_Load(object sender, EventArgs e)
        {

        }
    }
}
