using System;
using System.Windows.Forms;

namespace kylsim
{

    public partial class FormSetGt : Form
    {
        public VVS vvsComp;

        public FormSetGt(VVS obj)
        {
            vvsComp = obj;
            InitializeComponent();
            setUpDownValue(obj.gt);
        }

        private void FormSetGt_Load(object sender, EventArgs e)
        {
        }

        public void setUpDownValue(double gt)
        {
            numericUpDown1.Value = new decimal(gt);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Log.Write(vvsComp.LogFileName, vvsComp.Name + " ändrar gångtiden från " + vvsComp.gt + " till " + (numericUpDown1.Value).ToString());
            vvsComp.gt = (double)numericUpDown1.Value;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
