namespace xxkUI.Form
{
    partial class BakupDbFrm
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
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.textEditDbName = new DevExpress.XtraEditors.TextEdit();
            this.textEditUserName = new DevExpress.XtraEditors.TextEdit();
            this.textEditPsd = new DevExpress.XtraEditors.TextEdit();
            this.textEditPort = new DevExpress.XtraEditors.TextEdit();
            this.textEditSaveto = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnSaveto = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDbName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPsd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSaveto.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(60, 214);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "执行";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(141, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textEditDbName
            // 
            this.textEditDbName.Location = new System.Drawing.Point(81, 22);
            this.textEditDbName.Name = "textEditDbName";
            this.textEditDbName.Size = new System.Drawing.Size(169, 20);
            this.textEditDbName.TabIndex = 2;
            // 
            // textEditUserName
            // 
            this.textEditUserName.Location = new System.Drawing.Point(81, 60);
            this.textEditUserName.Name = "textEditUserName";
            this.textEditUserName.Size = new System.Drawing.Size(169, 20);
            this.textEditUserName.TabIndex = 3;
            // 
            // textEditPsd
            // 
            this.textEditPsd.Location = new System.Drawing.Point(81, 94);
            this.textEditPsd.Name = "textEditPsd";
            this.textEditPsd.Size = new System.Drawing.Size(169, 20);
            this.textEditPsd.TabIndex = 4;
            // 
            // textEditPort
            // 
            this.textEditPort.Location = new System.Drawing.Point(81, 129);
            this.textEditPort.Name = "textEditPort";
            this.textEditPort.Size = new System.Drawing.Size(169, 20);
            this.textEditPort.TabIndex = 5;
            // 
            // textEditSaveto
            // 
            this.textEditSaveto.Location = new System.Drawing.Point(81, 159);
            this.textEditSaveto.Name = "textEditSaveto";
            this.textEditSaveto.Size = new System.Drawing.Size(125, 20);
            this.textEditSaveto.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "数据库名:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(23, 63);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "用 户 名:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(23, 97);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "密     码:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(23, 132);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "端 口 号:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(23, 162);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 11;
            this.labelControl5.Text = "备 份 至:";
            // 
            // btnSaveto
            // 
            this.btnSaveto.Location = new System.Drawing.Point(212, 158);
            this.btnSaveto.Name = "btnSaveto";
            this.btnSaveto.Size = new System.Drawing.Size(38, 23);
            this.btnSaveto.TabIndex = 12;
            this.btnSaveto.Text = "...";
            this.btnSaveto.Click += new System.EventHandler(this.btnSaveto_Click);
            // 
            // BakupDbFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 249);
            this.Controls.Add(this.btnSaveto);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.textEditSaveto);
            this.Controls.Add(this.textEditPort);
            this.Controls.Add(this.textEditPsd);
            this.Controls.Add(this.textEditUserName);
            this.Controls.Add(this.textEditDbName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.LookAndFeel.SkinName = "Office 2013";
            this.Name = "BakupDbFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "备份数据库";
            ((System.ComponentModel.ISupportInitialize)(this.textEditDbName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPsd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSaveto.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit textEditDbName;
        private DevExpress.XtraEditors.TextEdit textEditUserName;
        private DevExpress.XtraEditors.TextEdit textEditPsd;
        private DevExpress.XtraEditors.TextEdit textEditPort;
        private DevExpress.XtraEditors.TextEdit textEditSaveto;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnSaveto;
    }
}