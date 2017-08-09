namespace xxkUI.Form
{
    partial class DataAdmin
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataAdmin));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUserUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcSiteName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcObsValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcObsDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCommitDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcNote = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcChecked = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnAgreeInsertToDb = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gcReject = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnReject = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAgreeInsertToDb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.btnAgreeInsertToDb,
            this.repositoryItemComboBox1,
            this.btnReject});
            this.gridControl.Size = new System.Drawing.Size(649, 310);
            this.gridControl.TabIndex = 0;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcUserName,
            this.gcUserUnit,
            this.gcSiteName,
            this.gcLineName,
            this.gcObsValue,
            this.gcObsDate,
            this.gcCommitDate,
            this.gcNote,
            this.gcChecked,
            this.gcReject});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // gcUserName
            // 
            this.gcUserName.Caption = "用户名";
            this.gcUserName.FieldName = "username";
            this.gcUserName.Name = "gcUserName";
            this.gcUserName.Visible = true;
            this.gcUserName.VisibleIndex = 0;
            this.gcUserName.Width = 73;
            // 
            // gcUserUnit
            // 
            this.gcUserUnit.Caption = "单位名称";
            this.gcUserUnit.FieldName = "userunit";
            this.gcUserUnit.Name = "gcUserUnit";
            this.gcUserUnit.Visible = true;
            this.gcUserUnit.VisibleIndex = 1;
            this.gcUserUnit.Width = 73;
            // 
            // gcSiteName
            // 
            this.gcSiteName.Caption = "场地名称";
            this.gcSiteName.FieldName = "sitename";
            this.gcSiteName.Name = "gcSiteName";
            this.gcSiteName.Visible = true;
            this.gcSiteName.VisibleIndex = 2;
            this.gcSiteName.Width = 73;
            // 
            // gcLineName
            // 
            this.gcLineName.Caption = "测线名称";
            this.gcLineName.FieldName = "obslinename";
            this.gcLineName.Name = "gcLineName";
            this.gcLineName.Visible = true;
            this.gcLineName.VisibleIndex = 3;
            this.gcLineName.Width = 73;
            // 
            // gcObsValue
            // 
            this.gcObsValue.Caption = "观测值(mm)";
            this.gcObsValue.FieldName = "obsvalue";
            this.gcObsValue.Name = "gcObsValue";
            this.gcObsValue.Visible = true;
            this.gcObsValue.VisibleIndex = 4;
            this.gcObsValue.Width = 83;
            // 
            // gcObsDate
            // 
            this.gcObsDate.Caption = "观测时间";
            this.gcObsDate.FieldName = "obsdate";
            this.gcObsDate.Name = "gcObsDate";
            this.gcObsDate.Visible = true;
            this.gcObsDate.VisibleIndex = 5;
            this.gcObsDate.Width = 69;
            // 
            // gcCommitDate
            // 
            this.gcCommitDate.Caption = "提交时间";
            this.gcCommitDate.FieldName = "cmitdate";
            this.gcCommitDate.Name = "gcCommitDate";
            this.gcCommitDate.Visible = true;
            this.gcCommitDate.VisibleIndex = 6;
            this.gcCommitDate.Width = 69;
            // 
            // gcNote
            // 
            this.gcNote.Caption = "备注";
            this.gcNote.FieldName = "note";
            this.gcNote.Name = "gcNote";
            this.gcNote.Visible = true;
            this.gcNote.VisibleIndex = 7;
            this.gcNote.Width = 73;
            // 
            // gcChecked
            // 
            this.gcChecked.Caption = "接收";
            this.gcChecked.ColumnEdit = this.btnAgreeInsertToDb;
            this.gcChecked.Name = "gcChecked";
            this.gcChecked.Visible = true;
            this.gcChecked.VisibleIndex = 8;
            // 
            // btnAgreeInsertToDb
            // 
            this.btnAgreeInsertToDb.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("btnAgreeInsertToDb.Appearance.Image")));
            this.btnAgreeInsertToDb.Appearance.Options.UseImage = true;
            this.btnAgreeInsertToDb.AutoHeight = false;
            this.btnAgreeInsertToDb.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnAgreeInsertToDb.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.btnAgreeInsertToDb.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnAgreeInsertToDb.Name = "btnAgreeInsertToDb";
            this.btnAgreeInsertToDb.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // gcReject
            // 
            this.gcReject.Caption = "拒绝";
            this.gcReject.ColumnEdit = this.btnReject;
            this.gcReject.Name = "gcReject";
            this.gcReject.Visible = true;
            this.gcReject.VisibleIndex = 9;
            // 
            // btnReject
            // 
            this.btnReject.AutoHeight = false;
            this.btnReject.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnReject.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.btnReject.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnReject.Name = "btnReject";
            this.btnReject.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // DataAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 310);
            this.Controls.Add(this.gridControl);
            this.Name = "DataAdmin";
            this.Text = "上报数据管理";
            this.Load += new System.EventHandler(this.DataAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAgreeInsertToDb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gcUserName;
        private DevExpress.XtraGrid.Columns.GridColumn gcUserUnit;
        private DevExpress.XtraGrid.Columns.GridColumn gcSiteName;
        private DevExpress.XtraGrid.Columns.GridColumn gcLineName;
        private DevExpress.XtraGrid.Columns.GridColumn gcObsValue;
        private DevExpress.XtraGrid.Columns.GridColumn gcObsDate;
        private DevExpress.XtraGrid.Columns.GridColumn gcCommitDate;
        private DevExpress.XtraGrid.Columns.GridColumn gcNote;
        private DevExpress.XtraGrid.Columns.GridColumn gcChecked;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnAgreeInsertToDb;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn gcReject;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnReject;
    }
}