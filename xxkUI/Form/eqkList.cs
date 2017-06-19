using DevExpress.XtraEditors;
using GMap.NET;
using Steema.TeeChart.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class eqkList : DevExpress.XtraEditors.XtraForm
    {
        public eqkList(List<EqkBean> eqkShowList)
        {
            InitializeComponent();
            LoadEqkData(eqkShowList);
        }
        private void LoadEqkData(List<EqkBean> eqkShowList)
        {
           
            //this.gridView.RefreshData();
        }
        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>


        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
          
        }
    }
}
