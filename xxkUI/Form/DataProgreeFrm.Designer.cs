namespace xxkUI.Form
{
    partial class DataProgreeFrm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbProgressExplain = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textValue = new DevExpress.XtraEditors.TextEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbDataCount = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textValue.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(68, 283);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(153, 283);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbProgressExplain);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 55);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // lbProgressExplain
            // 
            this.lbProgressExplain.AutoSize = true;
            this.lbProgressExplain.Location = new System.Drawing.Point(9, 22);
            this.lbProgressExplain.Name = "lbProgressExplain";
            this.lbProgressExplain.Size = new System.Drawing.Size(270, 18);
            this.lbProgressExplain.TabIndex = 1;
            this.lbProgressExplain.Text = "操作说明：处理数据=原始数据+操作值";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textValue);
            this.groupBox4.Location = new System.Drawing.Point(12, 201);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(283, 58);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "操作数值";
            // 
            // textValue
            // 
            this.textValue.Location = new System.Drawing.Point(12, 25);
            this.textValue.Name = "textValue";
            this.textValue.Size = new System.Drawing.Size(265, 24);
            this.textValue.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioGroup);
            this.groupBox3.Location = new System.Drawing.Point(12, 120);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(283, 75);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作方法";
            // 
            // radioGroup
            // 
            this.radioGroup.Location = new System.Drawing.Point(12, 24);
            this.radioGroup.Name = "radioGroup";
            this.radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("plus", "＋"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("minus", "－"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("multiply", "×"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("divide", "÷")});
            this.radioGroup.Size = new System.Drawing.Size(265, 43);
            this.radioGroup.TabIndex = 2;
            this.radioGroup.SelectedIndexChanged += new System.EventHandler(this.radioGroup_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbDataCount);
            this.groupBox2.Location = new System.Drawing.Point(12, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 49);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // lbDataCount
            // 
            this.lbDataCount.AutoSize = true;
            this.lbDataCount.Location = new System.Drawing.Point(9, 22);
            this.lbDataCount.Name = "lbDataCount";
            this.lbDataCount.Size = new System.Drawing.Size(167, 18);
            this.lbDataCount.TabIndex = 0;
            this.lbDataCount.Text = "待处理目标数量：100个";
            // 
            // DataProgreeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 326);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DataProgreeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "四则运算";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textValue.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbProgressExplain;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.TextEdit textValue;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbDataCount;
    }
}