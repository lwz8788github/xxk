namespace xxkUI.Form
{
    partial class RemoveJumpFrm
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.spinEditAvgSimpling = new DevExpress.XtraEditors.SpinEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbDataCount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.richTxtboxNote = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbExpectOffsetV = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAvgSimpling.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(163, 323);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(248, 323);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.spinEditAvgSimpling);
            this.groupBox4.Location = new System.Drawing.Point(12, 124);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(165, 58);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "平均值采样";
            // 
            // spinEditAvgSimpling
            // 
            this.spinEditAvgSimpling.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinEditAvgSimpling.Location = new System.Drawing.Point(12, 25);
            this.spinEditAvgSimpling.Name = "spinEditAvgSimpling";
            this.spinEditAvgSimpling.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditAvgSimpling.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.spinEditAvgSimpling.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.spinEditAvgSimpling.Size = new System.Drawing.Size(147, 24);
            this.spinEditAvgSimpling.TabIndex = 4;
            this.spinEditAvgSimpling.EditValueChanged += new System.EventHandler(this.textValue_EditValueChanged);
            this.spinEditAvgSimpling.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.textValue_EditValueChanging);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioGroup);
            this.groupBox3.Location = new System.Drawing.Point(183, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(302, 170);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作方法";
            // 
            // radioGroup
            // 
            this.radioGroup.EditValue = "both";
            this.radioGroup.Location = new System.Drawing.Point(6, 24);
            this.radioGroup.Name = "radioGroup";
            this.radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("both", "两侧采样平均值()"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("left", "左侧采样平均值()"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("right", "右侧采样平均值()")});
            this.radioGroup.Size = new System.Drawing.Size(290, 137);
            this.radioGroup.TabIndex = 2;
            this.radioGroup.SelectedIndexChanged += new System.EventHandler(this.radioGroup_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbDataCount);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 50);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "待处理目标个数";
            // 
            // lbDataCount
            // 
            this.lbDataCount.AutoSize = true;
            this.lbDataCount.Location = new System.Drawing.Point(9, 22);
            this.lbDataCount.Name = "lbDataCount";
            this.lbDataCount.Size = new System.Drawing.Size(47, 18);
            this.lbDataCount.TabIndex = 0;
            this.lbDataCount.Text = "100个";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkEdit1);
            this.groupBox1.Controls.Add(this.richTxtboxNote);
            this.groupBox1.Location = new System.Drawing.Point(10, 188);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 125);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(12, -1);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.checkEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.checkEdit1.Properties.Caption = "添加备注信息";
            this.checkEdit1.Size = new System.Drawing.Size(119, 22);
            this.checkEdit1.TabIndex = 1;
            // 
            // richTxtboxNote
            // 
            this.richTxtboxNote.Location = new System.Drawing.Point(6, 25);
            this.richTxtboxNote.Name = "richTxtboxNote";
            this.richTxtboxNote.Size = new System.Drawing.Size(463, 94);
            this.richTxtboxNote.TabIndex = 0;
            this.richTxtboxNote.Text = "[突跳消除@2017-05-27]\n偏移值：-0.482498\n偏移区间：1982-07-13至1982-08-25";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbExpectOffsetV);
            this.groupBox5.Location = new System.Drawing.Point(10, 68);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(167, 50);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "预期偏移值";
            // 
            // lbExpectOffsetV
            // 
            this.lbExpectOffsetV.AutoSize = true;
            this.lbExpectOffsetV.Location = new System.Drawing.Point(9, 22);
            this.lbExpectOffsetV.Name = "lbExpectOffsetV";
            this.lbExpectOffsetV.Size = new System.Drawing.Size(45, 18);
            this.lbExpectOffsetV.TabIndex = 0;
            this.lbExpectOffsetV.Text = "0.998";
            // 
            // RemoveJumpFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 366);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RemoveJumpFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "消突跳";
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAvgSimpling.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbDataCount;
        private DevExpress.XtraEditors.SpinEdit spinEditAvgSimpling;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private System.Windows.Forms.RichTextBox richTxtboxNote;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lbExpectOffsetV;
    }
}