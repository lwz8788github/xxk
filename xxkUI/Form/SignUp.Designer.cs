namespace xxkUI.Form
{
    partial class SignUp
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
            this.btnSignCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSignUp = new DevExpress.XtraEditors.SimpleButton();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.labelPwd = new DevExpress.XtraEditors.LabelControl();
            this.labelUserName = new DevExpress.XtraEditors.LabelControl();
            this.txtPsd1 = new DevExpress.XtraEditors.TextEdit();
            this.labelPwdConf = new DevExpress.XtraEditors.LabelControl();
            this.txtPsd2 = new DevExpress.XtraEditors.TextEdit();
            this.barFault = new DevExpress.XtraBars.BarEditItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.cbeAuth = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbeUnit = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPsd1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPsd2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeAuth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeUnit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSignCancel
            // 
            this.btnSignCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSignCancel.Location = new System.Drawing.Point(227, 183);
            this.btnSignCancel.Name = "btnSignCancel";
            this.btnSignCancel.Size = new System.Drawing.Size(75, 23);
            this.btnSignCancel.TabIndex = 18;
            this.btnSignCancel.Text = "取消";
            this.btnSignCancel.Click += new System.EventHandler(this.btnSignCancel_Click);
            // 
            // btnSignUp
            // 
            this.btnSignUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignUp.Location = new System.Drawing.Point(129, 183);
            this.btnSignUp.Name = "btnSignUp";
            this.btnSignUp.Size = new System.Drawing.Size(75, 23);
            this.btnSignUp.TabIndex = 17;
            this.btnSignUp.Text = "注册";
            this.btnSignUp.Click += new System.EventHandler(this.btnSignUp_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.EditValue = "superadmin";
            this.txtUsername.Location = new System.Drawing.Point(118, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(207, 20);
            this.txtUsername.TabIndex = 15;
            // 
            // labelPwd
            // 
            this.labelPwd.Location = new System.Drawing.Point(58, 51);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.Size = new System.Drawing.Size(55, 14);
            this.labelPwd.TabIndex = 14;
            this.labelPwd.Text = "*密   码：";
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(57, 22);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(55, 14);
            this.labelUserName.TabIndex = 13;
            this.labelUserName.Text = "*用户名：";
            // 
            // txtPsd1
            // 
            this.txtPsd1.EditValue = "jced";
            this.txtPsd1.Location = new System.Drawing.Point(118, 48);
            this.txtPsd1.Name = "txtPsd1";
            this.txtPsd1.Properties.AutoHeight = false;
            this.txtPsd1.Properties.PasswordChar = '*';
            this.txtPsd1.Properties.UseSystemPasswordChar = true;
            this.txtPsd1.Size = new System.Drawing.Size(207, 20);
            this.txtPsd1.TabIndex = 16;
            this.txtPsd1.EditValueChanged += new System.EventHandler(this.txtPsd1_EditValueChanged);
            // 
            // labelPwdConf
            // 
            this.labelPwdConf.Location = new System.Drawing.Point(46, 78);
            this.labelPwdConf.Name = "labelPwdConf";
            this.labelPwdConf.Size = new System.Drawing.Size(67, 14);
            this.labelPwdConf.TabIndex = 19;
            this.labelPwdConf.Text = "*确认密码：";
            // 
            // txtPsd2
            // 
            this.txtPsd2.EditValue = "jced";
            this.txtPsd2.Location = new System.Drawing.Point(118, 75);
            this.txtPsd2.Name = "txtPsd2";
            this.txtPsd2.Properties.AutoHeight = false;
            this.txtPsd2.Properties.PasswordChar = '*';
            this.txtPsd2.Properties.UseSystemPasswordChar = true;
            this.txtPsd2.Size = new System.Drawing.Size(207, 20);
            this.txtPsd2.TabIndex = 20;
            // 
            // barFault
            // 
            this.barFault.Caption = "断层数据：";
            this.barFault.Edit = null;
            this.barFault.EditWidth = 120;
            this.barFault.Id = 51;
            this.barFault.Name = "barFault";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "断层数据：";
            this.barEditItem1.Edit = null;
            this.barEditItem1.EditWidth = 120;
            this.barEditItem1.Id = 51;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // cbeAuth
            // 
            this.cbeAuth.EditValue = "";
            this.cbeAuth.Location = new System.Drawing.Point(118, 101);
            this.cbeAuth.Name = "cbeAuth";
            this.cbeAuth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeAuth.Size = new System.Drawing.Size(207, 20);
            this.cbeAuth.TabIndex = 21;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(45, 104);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(67, 14);
            this.labelControl1.TabIndex = 22;
            this.labelControl1.Text = "*权限申请：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(46, 131);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(67, 14);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "*用户单位：";
            // 
            // cbeUnit
            // 
            this.cbeUnit.Location = new System.Drawing.Point(118, 128);
            this.cbeUnit.Name = "cbeUnit";
            this.cbeUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeUnit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbeUnit.Size = new System.Drawing.Size(207, 20);
            this.cbeUnit.TabIndex = 26;
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 218);
            this.Controls.Add(this.cbeUnit);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cbeAuth);
            this.Controls.Add(this.labelPwdConf);
            this.Controls.Add(this.txtPsd2);
            this.Controls.Add(this.btnSignCancel);
            this.Controls.Add(this.btnSignUp);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.labelPwd);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.txtPsd1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户注册";
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPsd1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPsd2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeAuth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeUnit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSignCancel;
        private DevExpress.XtraEditors.SimpleButton btnSignUp;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.LabelControl labelPwd;
        private DevExpress.XtraEditors.LabelControl labelUserName;
        private DevExpress.XtraEditors.TextEdit txtPsd1;
        private DevExpress.XtraEditors.LabelControl labelPwdConf;
        private DevExpress.XtraEditors.TextEdit txtPsd2;
        private DevExpress.XtraBars.BarEditItem barFault;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbeAuth;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cbeUnit;
    }
}