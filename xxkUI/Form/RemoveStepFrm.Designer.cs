namespace xxkUI.Form
{
    partial class RemoveStepFrm
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.spinEditAvgSimpling = new DevExpress.XtraEditors.SpinEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbDataCount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTxtboxNote = new System.Windows.Forms.RichTextBox();
            this.checkAddNote = new DevExpress.XtraEditors.CheckEdit();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbExpectOffsetV = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAvgSimpling.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkAddNote.Properties)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.spinEditAvgSimpling);
            this.groupBox4.Location = new System.Drawing.Point(12, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 76);
            this.groupBox4.TabIndex = 18;
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
            this.spinEditAvgSimpling.Location = new System.Drawing.Point(6, 32);
            this.spinEditAvgSimpling.Name = "spinEditAvgSimpling";
            this.spinEditAvgSimpling.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditAvgSimpling.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.spinEditAvgSimpling.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.spinEditAvgSimpling.Size = new System.Drawing.Size(167, 24);
            this.spinEditAvgSimpling.TabIndex = 4;
            this.spinEditAvgSimpling.EditValueChanged += new System.EventHandler(this.spinEditAvgSimpling_EditValueChanged);
            this.spinEditAvgSimpling.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.spinEditAvgSimpling_EditValueChanging);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioGroup);
            this.groupBox3.Location = new System.Drawing.Point(203, 69);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(255, 106);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作方法";
            // 
            // radioGroup
            // 
            this.radioGroup.EditValue = "left";
            this.radioGroup.Location = new System.Drawing.Point(6, 24);
            this.radioGroup.Name = "radioGroup";
            this.radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("left", "消除左侧(平均值)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("right", "消除右侧(平均值)")});
            this.radioGroup.Size = new System.Drawing.Size(243, 76);
            this.radioGroup.TabIndex = 2;
            this.radioGroup.SelectedIndexChanged += new System.EventHandler(this.radioGroup_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(238, 329);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.Location = new System.Drawing.Point(153, 329);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbDataCount);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 72);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "待处理目标个数";
            // 
            // lbDataCount
            // 
            this.lbDataCount.AutoSize = true;
            this.lbDataCount.Location = new System.Drawing.Point(10, 33);
            this.lbDataCount.Name = "lbDataCount";
            this.lbDataCount.Size = new System.Drawing.Size(47, 18);
            this.lbDataCount.TabIndex = 0;
            this.lbDataCount.Text = "100个";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkAddNote);
            this.groupBox1.Controls.Add(this.richTxtboxNote);
            this.groupBox1.Location = new System.Drawing.Point(12, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 128);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // richTxtboxNote
            // 
            this.richTxtboxNote.Location = new System.Drawing.Point(5, 25);
            this.richTxtboxNote.Name = "richTxtboxNote";
            this.richTxtboxNote.Size = new System.Drawing.Size(434, 94);
            this.richTxtboxNote.TabIndex = 0;
            this.richTxtboxNote.Text = "[台阶消除@2017-05-27]\n偏移值：-0.482498";
            // 
            // checkAddNote
            // 
            this.checkAddNote.Location = new System.Drawing.Point(13, -3);
            this.checkAddNote.Name = "checkAddNote";
            this.checkAddNote.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.checkAddNote.Properties.Appearance.Options.UseBackColor = true;
            this.checkAddNote.Properties.Caption = "添加备注信息";
            this.checkAddNote.Size = new System.Drawing.Size(119, 22);
            this.checkAddNote.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbExpectOffsetV);
            this.groupBox5.Location = new System.Drawing.Point(203, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(255, 51);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "预期偏移值";
            // 
            // lbExpectOffsetV
            // 
            this.lbExpectOffsetV.AutoSize = true;
            this.lbExpectOffsetV.Location = new System.Drawing.Point(6, 22);
            this.lbExpectOffsetV.Name = "lbExpectOffsetV";
            this.lbExpectOffsetV.Size = new System.Drawing.Size(45, 18);
            this.lbExpectOffsetV.TabIndex = 0;
            this.lbExpectOffsetV.Text = "0.998";
            // 
            // RemoveStepFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 371);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RemoveStepFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "消台阶";
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAvgSimpling.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkAddNote.Properties)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.SpinEdit spinEditAvgSimpling;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbDataCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.CheckEdit checkAddNote;
        private System.Windows.Forms.RichTextBox richTxtboxNote;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lbExpectOffsetV;
    }
}