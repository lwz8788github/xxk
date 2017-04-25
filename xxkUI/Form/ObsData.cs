using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace xxkUI.Form
{
    public partial class ObsData : DevExpress.XtraEditors.XtraForm
    {
        public ObsData()
        {
            InitializeComponent();
        }

        public void LoadDataSource(List<LineObsBean> obslist)
        {
            this.gridControl.DataSource = obslist;
            
        }

        public void LoadDataSource(DataTable dt)
        {
            this.gridControl.DataSource = null;
            this.gridControl.DataSource = dt;
            this.gridControl.Refresh();
        }

    }
}