using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyWebBrowser
{
    public partial class Buy : UserControl
    {
        public Buy()
        {
            InitializeComponent();
        }

        private void Buy_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(50, 200, 200, 200);
        }
    }
}
