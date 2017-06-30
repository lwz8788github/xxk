namespace xxkUI.Form
{
    partial class IntervalFrm
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
            this.spinInterval = new DevExpress.XtraEditors.SpinEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Canel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.spinInterval.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // spinInterval
            // 
            this.spinInterval.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spinInterval.Location = new System.Drawing.Point(118, 43);
            this.spinInterval.Name = "spinInterval";
            this.spinInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinInterval.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.spinInterval.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.spinInterval.Size = new System.Drawing.Size(93, 24);
            this.spinInterval.TabIndex = 30;
            this.spinInterval.EditValueChanged += new System.EventHandler(this.spinInterval_EditValueChanged);
            this.spinInterval.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.spinInterval_EditValueChanging);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.spinInterval);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox2.Font = new System.Drawing.Font("楷体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(12, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 89);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "请选择数据处理间隔";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("华文仿宋", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(26, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "间隔数：";
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK.ForeColor = System.Drawing.Color.Green;
            this.OK.Location = new System.Drawing.Point(28, 141);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 33);
            this.OK.TabIndex = 21;
            this.OK.Text = "确定";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Canel
            // 
            this.Canel.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Canel.ForeColor = System.Drawing.Color.Green;
            this.Canel.Location = new System.Drawing.Point(151, 141);
            this.Canel.Name = "Canel";
            this.Canel.Size = new System.Drawing.Size(75, 33);
            this.Canel.TabIndex = 22;
            this.Canel.Text = "取消";
            this.Canel.UseVisualStyleBackColor = true;
            this.Canel.Click += new System.EventHandler(this.Canel_Click);
            // 
            // IntervalFrm
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Red;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 186);
            this.Controls.Add(this.Canel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.SkinName = "Metropolis";
            this.Name = "IntervalFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntervalFrm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IntervalFrm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.IntervalFrm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IntervalFrm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.spinInterval.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit spinInterval;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Canel;
    }
}