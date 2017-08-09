namespace xxkUI.Controls
{
    partial class TChartControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TChartControl));
            this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            this.tChart = new Steema.TeeChart.TChart();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.btnCancelEdit = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveData = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditData = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteData = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddData = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlObsdata = new DevExpress.XtraGrid.GridControl();
            this.gridViewObsdata = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnObsDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnObsValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnNote = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
            this.splitContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlObsdata)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewObsdata)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl
            // 
            this.splitContainerControl.CustomHeaderButtons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraEditors.ButtonsPanelControl.GroupBoxButton()});
            this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl.Name = "splitContainerControl";
            this.splitContainerControl.Panel1.Controls.Add(this.tChart);
            this.splitContainerControl.Panel1.Text = "Panel1";
            this.splitContainerControl.Panel2.Controls.Add(this.xtraTabControl1);
            this.splitContainerControl.Panel2.Text = "Panel2";
            this.splitContainerControl.Size = new System.Drawing.Size(1777, 840);
            this.splitContainerControl.SplitterPosition = 1127;
            this.splitContainerControl.TabIndex = 0;
            this.splitContainerControl.Text = "splitContainerControl1";
            // 
            // tChart
            // 
            // 
            // 
            // 
            this.tChart.Aspect.ColorPaletteIndex = 20;
            this.tChart.Aspect.View3D = false;
            // 
            // 
            // 
            this.tChart.Axes.Automatic = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Bottom.Grid.DrawEvery = 2;
            this.tChart.Axes.Bottom.Grid.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChart.Axes.Bottom.Labels.Font.Size = 9;
            this.tChart.Axes.Bottom.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart.Axes.Bottom.Title.Font.Size = 11;
            this.tChart.Axes.Bottom.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Left.AxisPen.Visible = false;
            // 
            // 
            // 
            this.tChart.Axes.Left.Grid.DrawEvery = 2;
            this.tChart.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.Gray;
            this.tChart.Axes.Left.Labels.Font.Size = 9;
            this.tChart.Axes.Left.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            this.tChart.Axes.Left.MinorTicks.Visible = false;
            // 
            // 
            // 
            this.tChart.Axes.Left.Ticks.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart.Axes.Left.Title.Font.Size = 11;
            this.tChart.Axes.Left.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Right.AxisPen.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart.Axes.Right.Labels.Font.Size = 9;
            this.tChart.Axes.Right.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart.Axes.Top.Labels.Font.Size = 9;
            this.tChart.Axes.Top.Labels.Font.SizeFloat = 9F;
            this.tChart.CurrentTheme = Steema.TeeChart.ThemeType.Report;
            this.tChart.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Header.Font.Brush.Color = System.Drawing.Color.Gray;
            this.tChart.Header.Font.Size = 12;
            this.tChart.Header.Font.SizeFloat = 12F;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart.Legend.Font.Size = 9;
            this.tChart.Legend.Font.SizeFloat = 9F;
            // 
            // 
            // 
            this.tChart.Legend.Pen.Visible = false;
            // 
            // 
            // 
            this.tChart.Legend.Shadow.Visible = false;
            this.tChart.Legend.Transparent = true;
            this.tChart.Location = new System.Drawing.Point(0, 0);
            this.tChart.Name = "tChart";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart.Panel.Brush.Gradient.Visible = false;
            this.tChart.Size = new System.Drawing.Size(1127, 840);
            this.tChart.TabIndex = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart.Walls.Back.Brush.Visible = false;
            this.tChart.Walls.Back.Transparent = true;
            this.tChart.Walls.Back.Visible = false;
            // 
            // 
            // 
            this.tChart.Zoom.MouseButton = System.Windows.Forms.MouseButtons.None;
            this.tChart.ClickLegend += new System.Windows.Forms.MouseEventHandler(this.tChart_ClickLegend);
            this.tChart.AfterDraw += new Steema.TeeChart.PaintChartEventHandler(this.tChart_AfterDraw);
            this.tChart.ClickSeries += new Steema.TeeChart.SeriesEventHandler(this.tChart_ClickSeries);
            this.tChart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tChart_KeyDown);
            this.tChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tChart_MouseDown);
            this.tChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tChart_MouseMove);
            this.tChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tChart_MouseUp);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabControl1.Size = new System.Drawing.Size(645, 840);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.btnCancelEdit);
            this.xtraTabPage1.Controls.Add(this.btnSaveData);
            this.xtraTabPage1.Controls.Add(this.btnEditData);
            this.xtraTabPage1.Controls.Add(this.btnDeleteData);
            this.xtraTabPage1.Controls.Add(this.btnAddData);
            this.xtraTabPage1.Controls.Add(this.gridControlObsdata);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(639, 811);
            this.xtraTabPage1.Text = "监测数据";
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelEdit.Image")));
            this.btnCancelEdit.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnCancelEdit.Location = new System.Drawing.Point(327, 774);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(29, 23);
            this.btnCancelEdit.TabIndex = 30;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveData.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveData.Image")));
            this.btnSaveData.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSaveData.Location = new System.Drawing.Point(362, 774);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(29, 23);
            this.btnSaveData.TabIndex = 29;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // btnEditData
            // 
            this.btnEditData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEditData.Image = ((System.Drawing.Image)(resources.GetObject("btnEditData.Image")));
            this.btnEditData.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnEditData.Location = new System.Drawing.Point(292, 774);
            this.btnEditData.Name = "btnEditData";
            this.btnEditData.Size = new System.Drawing.Size(29, 23);
            this.btnEditData.TabIndex = 28;
            this.btnEditData.Click += new System.EventHandler(this.btnEditData_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDeleteData.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteData.Image")));
            this.btnDeleteData.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDeleteData.Location = new System.Drawing.Point(257, 774);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(29, 23);
            this.btnDeleteData.TabIndex = 27;
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // btnAddData
            // 
            this.btnAddData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddData.Image = ((System.Drawing.Image)(resources.GetObject("btnAddData.Image")));
            this.btnAddData.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnAddData.Location = new System.Drawing.Point(222, 774);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(29, 23);
            this.btnAddData.TabIndex = 26;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // gridControlObsdata
            // 
            this.gridControlObsdata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlObsdata.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControlObsdata.Location = new System.Drawing.Point(0, 0);
            this.gridControlObsdata.MainView = this.gridViewObsdata;
            this.gridControlObsdata.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControlObsdata.Name = "gridControlObsdata";
            this.gridControlObsdata.Size = new System.Drawing.Size(639, 764);
            this.gridControlObsdata.TabIndex = 25;
            this.gridControlObsdata.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewObsdata});
            // 
            // gridViewObsdata
            // 
            this.gridViewObsdata.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnObsDate,
            this.gridColumnObsValue,
            this.gridColumnNote});
            this.gridViewObsdata.GridControl = this.gridControlObsdata;
            this.gridViewObsdata.Name = "gridViewObsdata";
            this.gridViewObsdata.OptionsBehavior.Editable = false;
            this.gridViewObsdata.OptionsView.ShowGroupPanel = false;
            this.gridViewObsdata.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridViewObsdata_ShowingEditor);
            this.gridViewObsdata.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewObsdata_CellValueChanged);
            this.gridViewObsdata.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewObsdata_MouseDown);
            // 
            // gridColumnObsDate
            // 
            this.gridColumnObsDate.Caption = "观测时间";
            this.gridColumnObsDate.FieldName = "obvdate";
            this.gridColumnObsDate.Name = "gridColumnObsDate";
            this.gridColumnObsDate.Visible = true;
            this.gridColumnObsDate.VisibleIndex = 0;
            // 
            // gridColumnObsValue
            // 
            this.gridColumnObsValue.Caption = "观测值";
            this.gridColumnObsValue.FieldName = "obvvalue";
            this.gridColumnObsValue.Name = "gridColumnObsValue";
            this.gridColumnObsValue.Visible = true;
            this.gridColumnObsValue.VisibleIndex = 1;
            // 
            // gridColumnNote
            // 
            this.gridColumnNote.Caption = "备注";
            this.gridColumnNote.FieldName = "note";
            this.gridColumnNote.Name = "gridColumnNote";
            this.gridColumnNote.Visible = true;
            this.gridColumnNote.VisibleIndex = 2;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(639, 811);
            this.xtraTabPage2.Text = "曲线设置";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barbtnInsertRec";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // TChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl);
            this.Name = "TChartControl";
            this.Size = new System.Drawing.Size(1777, 840);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
            this.splitContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlObsdata)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewObsdata)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
        private Steema.TeeChart.TChart tChart;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.SimpleButton btnCancelEdit;
        private DevExpress.XtraEditors.SimpleButton btnSaveData;
        private DevExpress.XtraEditors.SimpleButton btnEditData;
        private DevExpress.XtraEditors.SimpleButton btnDeleteData;
        private DevExpress.XtraEditors.SimpleButton btnAddData;
        private DevExpress.XtraGrid.GridControl gridControlObsdata;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewObsdata;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnObsDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnObsValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNote;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer timer1;
    }
}
