namespace xxkUI.Controls
{
    partial class ObsDataControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnObsDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnObsValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnEditData = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteData = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnInsertData = new DevExpress.XtraEditors.SimpleButton();
            this.popObsdata = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popObsdata)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(247, 359);
            this.gridControl.TabIndex = 17;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnObsDate,
            this.gridColumnObsValue});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView_ShowingEditor);
            this.gridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView_CellValueChanged);
            this.gridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseDown);
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
            // btnEditData
            // 
            this.btnEditData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditData.Location = new System.Drawing.Point(128, 367);
            this.btnEditData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEditData.Name = "btnEditData";
            this.btnEditData.Size = new System.Drawing.Size(55, 30);
            this.btnEditData.TabIndex = 21;
            this.btnEditData.Text = "编辑";
            this.btnEditData.Click += new System.EventHandler(this.btnEditData_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteData.Location = new System.Drawing.Point(65, 367);
            this.btnDeleteData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(55, 30);
            this.btnDeleteData.TabIndex = 20;
            this.btnDeleteData.Text = "删除";
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(189, 367);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(55, 30);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnInsertData
            // 
            this.btnInsertData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInsertData.Location = new System.Drawing.Point(4, 367);
            this.btnInsertData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInsertData.Name = "btnInsertData";
            this.btnInsertData.Size = new System.Drawing.Size(55, 30);
            this.btnInsertData.TabIndex = 18;
            this.btnInsertData.Text = "插入";
            this.btnInsertData.Click += new System.EventHandler(this.btnInsertData_Click);
            // 
            // popObsdata
            // 
            this.popObsdata.Name = "popObsdata";
            // 
            // ObsDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.btnEditData);
            this.Controls.Add(this.btnDeleteData);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnInsertData);
            this.Name = "ObsDataControl";
            this.Size = new System.Drawing.Size(247, 401);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popObsdata)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btnEditData;
        private DevExpress.XtraEditors.SimpleButton btnDeleteData;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnInsertData;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnObsDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnObsValue;
        private DevExpress.XtraBars.PopupMenu popObsdata;
    }
}
