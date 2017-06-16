namespace xxkUI.Form
{
    partial class ObslineMergeFrm
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
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.treeListData = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(60, 544);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 35);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "处理";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(154, 544);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioGroup);
            this.groupBox2.Location = new System.Drawing.Point(3, 465);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(305, 70);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "是否移动到平均值处？";
            // 
            // radioGroup
            // 
            this.radioGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup.EditValue = true;
            this.radioGroup.Location = new System.Drawing.Point(3, 22);
            this.radioGroup.Name = "radioGroup";
            this.radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "是", true, "移动到平均值处"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "否", true, "不移动到平均值处")});
            this.radioGroup.Size = new System.Drawing.Size(299, 45);
            this.radioGroup.TabIndex = 0;
            // 
            // treeListData
            // 
            this.treeListData.AppearancePrint.OddRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.treeListData.AppearancePrint.OddRow.Options.UseBackColor = true;
            this.treeListData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.treeListData.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeListData.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListData.KeyFieldName = "";
            this.treeListData.Location = new System.Drawing.Point(3, 3);
            this.treeListData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeListData.Name = "treeListData";
            this.treeListData.OptionsSelection.MultiSelectMode = DevExpress.XtraTreeList.TreeListMultiSelectMode.CellSelect;
            this.treeListData.OptionsView.ShowHorzLines = false;
            this.treeListData.ParentFieldName = "";
            this.treeListData.Size = new System.Drawing.Size(305, 455);
            this.treeListData.TabIndex = 6;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "场地";
            this.treeListColumn1.FieldName = "Caption";
            this.treeListColumn1.MinWidth = 52;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.OptionsColumn.FixedWidth = true;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 190;
            // 
            // ObslineMergeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 591);
            this.Controls.Add(this.treeListData);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObslineMergeFrm";
            this.ShowIcon = false;
            this.Text = "选择要合并的测项";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
        private DevExpress.XtraTreeList.TreeList treeListData;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    }
}