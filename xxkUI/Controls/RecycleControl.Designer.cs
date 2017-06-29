namespace xxkUI.Controls
{
    partial class RecycleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecycleControl));
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteSelected = new DevExpress.XtraEditors.SimpleButton();
            this.btnRecoverySelected = new DevExpress.XtraEditors.SimpleButton();
            this.btnRecoveryRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridControlRecycle = new DevExpress.XtraGrid.GridControl();
            this.gridViewRecycle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRecyName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRecyFileType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRecySize = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRecySite = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRecyTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRecyRecoveryBtn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridDeleteCompleteBtn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.rICheckEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRecycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRecycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecyRecoveryBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDeleteCompleteBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rICheckEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(0, 11);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(94, 26);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空回收站";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteSelected.Image")));
            this.btnDeleteSelected.Location = new System.Drawing.Point(98, 11);
            this.btnDeleteSelected.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(76, 26);
            this.btnDeleteSelected.TabIndex = 2;
            this.btnDeleteSelected.Text = "删除选中";
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnRecoverySelected
            // 
            this.btnRecoverySelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRecoverySelected.Image = ((System.Drawing.Image)(resources.GetObject("btnRecoverySelected.Image")));
            this.btnRecoverySelected.Location = new System.Drawing.Point(178, 12);
            this.btnRecoverySelected.Margin = new System.Windows.Forms.Padding(2);
            this.btnRecoverySelected.Name = "btnRecoverySelected";
            this.btnRecoverySelected.Size = new System.Drawing.Size(87, 26);
            this.btnRecoverySelected.TabIndex = 3;
            this.btnRecoverySelected.Text = "还原选中";
            this.btnRecoverySelected.Click += new System.EventHandler(this.btnRecoverySelected_Click);
            // 
            // btnRecoveryRefresh
            // 
            this.btnRecoveryRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRecoveryRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRecoveryRefresh.Image")));
            this.btnRecoveryRefresh.Location = new System.Drawing.Point(269, 11);
            this.btnRecoveryRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRecoveryRefresh.Name = "btnRecoveryRefresh";
            this.btnRecoveryRefresh.Size = new System.Drawing.Size(106, 26);
            this.btnRecoveryRefresh.TabIndex = 4;
            this.btnRecoveryRefresh.Text = "刷新回收站";
            this.btnRecoveryRefresh.Click += new System.EventHandler(this.btnRecoveryRefresh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnRecoveryRefresh);
            this.groupBox1.Controls.Add(this.btnDeleteSelected);
            this.groupBox1.Controls.Add(this.btnRecoverySelected);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 265);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(648, 42);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // gridControlRecycle
            // 
            this.gridControlRecycle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlRecycle.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControlRecycle.Location = new System.Drawing.Point(0, 0);
            this.gridControlRecycle.MainView = this.gridViewRecycle;
            this.gridControlRecycle.Margin = new System.Windows.Forms.Padding(2);
            this.gridControlRecycle.Name = "gridControlRecycle";
            this.gridControlRecycle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gcRecyRecoveryBtn,
            this.gridDeleteCompleteBtn,
            this.rICheckEdit});
            this.gridControlRecycle.Size = new System.Drawing.Size(648, 265);
            this.gridControlRecycle.TabIndex = 6;
            this.gridControlRecycle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewRecycle});
            // 
            // gridViewRecycle
            // 
            this.gridViewRecycle.Appearance.EvenRow.BackColor = System.Drawing.Color.Azure;
            this.gridViewRecycle.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridViewRecycle.Appearance.OddRow.BackColor = System.Drawing.Color.FloralWhite;
            this.gridViewRecycle.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewRecycle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcRecyName,
            this.gcRecyFileType,
            this.gcRecySize,
            this.gcRecySite,
            this.gcRecyTime});
            this.gridViewRecycle.GridControl = this.gridControlRecycle;
            this.gridViewRecycle.Name = "gridViewRecycle";
            this.gridViewRecycle.OptionsBehavior.Editable = false;
            this.gridViewRecycle.OptionsSelection.MultiSelect = true;
            this.gridViewRecycle.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridViewRecycle.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewRecycle.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewRecycle.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridViewRecycle.OptionsView.ShowGroupPanel = false;
            // 
            // gcRecyName
            // 
            this.gcRecyName.Caption = "文件名";
            this.gcRecyName.FieldName = "RecyName";
            this.gcRecyName.Name = "gcRecyName";
            this.gcRecyName.Visible = true;
            this.gcRecyName.VisibleIndex = 1;
            this.gcRecyName.Width = 309;
            // 
            // gcRecyFileType
            // 
            this.gcRecyFileType.Caption = "文件类型";
            this.gcRecyFileType.FieldName = "RecyFileType";
            this.gcRecyFileType.Name = "gcRecyFileType";
            this.gcRecyFileType.Visible = true;
            this.gcRecyFileType.VisibleIndex = 4;
            this.gcRecyFileType.Width = 246;
            // 
            // gcRecySize
            // 
            this.gcRecySize.Caption = "文件大小";
            this.gcRecySize.FieldName = "RecySize";
            this.gcRecySize.Name = "gcRecySize";
            this.gcRecySize.Visible = true;
            this.gcRecySize.VisibleIndex = 5;
            this.gcRecySize.Width = 156;
            // 
            // gcRecySite
            // 
            this.gcRecySite.Caption = "原始位置";
            this.gcRecySite.FieldName = "RecySite";
            this.gcRecySite.Name = "gcRecySite";
            this.gcRecySite.Visible = true;
            this.gcRecySite.VisibleIndex = 2;
            this.gcRecySite.Width = 335;
            // 
            // gcRecyTime
            // 
            this.gcRecyTime.Caption = "删除时间";
            this.gcRecyTime.FieldName = "RecyTime";
            this.gcRecyTime.Name = "gcRecyTime";
            this.gcRecyTime.Visible = true;
            this.gcRecyTime.VisibleIndex = 3;
            this.gcRecyTime.Width = 321;
            // 
            // gcRecyRecoveryBtn
            // 
            this.gcRecyRecoveryBtn.AutoHeight = false;
            this.gcRecyRecoveryBtn.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gcRecyRecoveryBtn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Redo)});
            this.gcRecyRecoveryBtn.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gcRecyRecoveryBtn.Name = "gcRecyRecoveryBtn";
            // 
            // gridDeleteCompleteBtn
            // 
            this.gridDeleteCompleteBtn.AutoHeight = false;
            this.gridDeleteCompleteBtn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.gridDeleteCompleteBtn.Name = "gridDeleteCompleteBtn";
            // 
            // rICheckEdit
            // 
            this.rICheckEdit.AutoHeight = false;
            this.rICheckEdit.Name = "rICheckEdit";
            // 
            // RecycleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlRecycle);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RecycleControl";
            this.Size = new System.Drawing.Size(648, 307);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRecycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRecycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecyRecoveryBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDeleteCompleteBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rICheckEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnDeleteSelected;
        private DevExpress.XtraEditors.SimpleButton btnRecoverySelected;
        private DevExpress.XtraEditors.SimpleButton btnRecoveryRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gridControlRecycle;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewRecycle;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecyName;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecyFileType;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecySize;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecySite;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecyTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit gcRecyRecoveryBtn;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit gridDeleteCompleteBtn;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rICheckEdit;
    }
}
