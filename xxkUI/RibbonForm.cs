using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using xxkUI.Form;

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonForm()
        {
            InitializeComponent();
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }

        private void btnLogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            Login lg = new Login();
        
            if (lg.ShowDialog() == DialogResult.OK)
            {
                currentUser.Caption = currentUser.Caption + lg.Username;
            }
            else
            {
                return;
            }
        }
    }
}