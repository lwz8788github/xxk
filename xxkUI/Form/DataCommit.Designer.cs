using DevExpress.XtraEditors;

namespace xxkUI.Form
{
    partial class DataCommit:XtraForm 
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
            this.btnCommit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtObsvalue = new DevExpress.XtraEditors.TextEdit();
            this.dateEdit = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cbeSite = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbeLine = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelSite = new System.Windows.Forms.Label();
            this.labelLine = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtObsvalue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeLine.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(106, 133);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(75, 23);
            this.btnCommit.TabIndex = 27;
            this.btnCommit.Text = "提交";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 14);
            this.label2.TabIndex = 26;
            this.label2.Text = "观测值(mm)：";
            // 
            // txtObsvalue
            // 
            this.txtObsvalue.EditValue = "";
            this.txtObsvalue.Location = new System.Drawing.Point(104, 91);
            this.txtObsvalue.Name = "txtObsvalue";
            this.txtObsvalue.Properties.Mask.EditMask = "\\d+(\\.\\d{2})?";
            this.txtObsvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtObsvalue.Size = new System.Drawing.Size(250, 20);
            this.txtObsvalue.TabIndex = 25;
            // 
            // dateEdit
            // 
            this.dateEdit.EditValue = null;
            this.dateEdit.Location = new System.Drawing.Point(104, 65);
            this.dateEdit.Name = "dateEdit";
            this.dateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit.Size = new System.Drawing.Size(250, 20);
            this.dateEdit.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 21;
            this.label1.Text = "观测日期：";
            // 
            // cbeSite
            // 
            this.cbeSite.Location = new System.Drawing.Point(104, 12);
            this.cbeSite.Name = "cbeSite";
            this.cbeSite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeSite.Size = new System.Drawing.Size(250, 20);
            this.cbeSite.TabIndex = 22;
            this.cbeSite.SelectedValueChanged += new System.EventHandler(this.cbeSite_SelectedValueChanged);
            // 
            // cbeLine
            // 
            this.cbeLine.Location = new System.Drawing.Point(104, 39);
            this.cbeLine.Name = "cbeLine";
            this.cbeLine.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeLine.Size = new System.Drawing.Size(250, 20);
            this.cbeLine.TabIndex = 23;
            // 
            // labelSite
            // 
            this.labelSite.AutoSize = true;
            this.labelSite.Location = new System.Drawing.Point(54, 15);
            this.labelSite.Name = "labelSite";
            this.labelSite.Size = new System.Drawing.Size(43, 14);
            this.labelSite.TabIndex = 19;
            this.labelSite.Text = "场地：";
            // 
            // labelLine
            // 
            this.labelLine.AutoSize = true;
            this.labelLine.Location = new System.Drawing.Point(54, 42);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(43, 14);
            this.labelLine.TabIndex = 20;
            this.labelLine.Text = "测线：";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(187, 133);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DataCommit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 168);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtObsvalue);
            this.Controls.Add(this.dateEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbeSite);
            this.Controls.Add(this.cbeLine);
            this.Controls.Add(this.labelSite);
            this.Controls.Add(this.labelLine);
            this.Name = "DataCommit";
            this.Text = "数据提交";
            ((System.ComponentModel.ISupportInitialize)(this.txtObsvalue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeLine.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Label label2;
        private TextEdit txtObsvalue;
        private DateEdit dateEdit;
        private System.Windows.Forms.Label label1;
        private ComboBoxEdit cbeSite;
        private ComboBoxEdit cbeLine;
        private System.Windows.Forms.Label labelSite;
        private System.Windows.Forms.Label labelLine;
        private System.Windows.Forms.Button btnCancel;
    }
}