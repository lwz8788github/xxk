using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common.Data;

namespace xxkUI.Form
{
    public partial class ObsData : DevExpress.XtraEditors.XtraForm
    {
      
    
        public ObsData()
        {
            InitializeComponent();
            this.gridView.OptionsBehavior.Editable = false;//不可编辑
        }

        public void LoadDataSource(List<LineObsBean> obslist)
        {
            this.gridControl.DataSource = obslist;

        }

        public void LoadDataSource(DataTable _dt)
        {
            this.gridControl.DataSource = null;
            this.gridControl.DataSource = _dt;
          
            this.gridControl.Refresh();
        }

        private void btnInsertData_Click(object sender, EventArgs e)
        {
            int focusedRow = this.gridView.FocusedRowHandle;
            //AddRows(focusedRow);
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            int focusedRow = this.gridView.FocusedRowHandle;
            SubRows(focusedRow);
        }

        private void btnEditData_Click(object sender, EventArgs e)
        {
            this.gridView.OptionsBehavior.Editable = true;//不可编辑
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="row_num">行索引</param>
        private void AddRows(string row_num)
        {
            (this.gridControl.DataSource as DataTable).Rows.Add(new object[] { row_num, "" });
           // this.gridControl.DataSource = dt;
        }
        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="rowindex">行索引</param>
        private void SubRows(int rowindex)
        {
            (this.gridControl.DataSource as DataTable).Rows.RemoveAt(rowindex);
           
        }
        /// <summary>
        /// 修改行
        /// </summary>
        /// <param name="rowindex">行索引</param>
        /// <param name="colindex">列索引</param>
        /// <param name="value">值</param>
        private void ModifyRows(int rowindex, int colindex, string value)
        {
            //dt.Rows[rowindex][colindex] = value;
            //this.gridControl.DataSource = dt;
        }

    }

    [Description("新增数据类")]
    public class AddRowBean {

        [Description("经度")]
        private double _Longtitude;
        public double Longtitude
        {
            get { return _Longtitude; }
            set { _Longtitude = value; }
        }
    }
}