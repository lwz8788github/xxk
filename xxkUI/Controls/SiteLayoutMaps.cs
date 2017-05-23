using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Controls
{
    public partial class SiteLayoutMaps : UserControl
    {
        public SiteLayoutMaps()
        {
            InitializeComponent();
        }

        private void pictureEdit_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void pictureEdit_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void pictureEdit_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEdit.Height = pictureEdit.Height + 5;
            pictureEdit.Width = pictureEdit.Width + 5;
        }
    }
}
