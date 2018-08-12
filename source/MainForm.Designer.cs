namespace ECHONET_Lite
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblNumber = new System.Windows.Forms.Label();
            this.nudNumber = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnON = new System.Windows.Forms.Button();
            this.btnOFF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "一覧の取得";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 41);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(260, 172);
            this.textBox1.TabIndex = 1;
            this.textBox1.TabStop = false;
            // 
            // lblNumber
            // 
            this.lblNumber.AutoSize = true;
            this.lblNumber.Location = new System.Drawing.Point(12, 223);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(32, 12);
            this.lblNumber.TabIndex = 11;
            this.lblNumber.Text = "ノード";
            // 
            // nudNumber
            // 
            this.nudNumber.Location = new System.Drawing.Point(91, 220);
            this.nudNumber.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudNumber.Name = "nudNumber";
            this.nudNumber.Size = new System.Drawing.Size(42, 19);
            this.nudNumber.TabIndex = 3;
            this.nudNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox2.Location = new System.Drawing.Point(50, 219);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(35, 19);
            this.textBox2.TabIndex = 2;
            this.textBox2.TabStop = false;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnON
            // 
            this.btnON.Location = new System.Drawing.Point(139, 218);
            this.btnON.Name = "btnON";
            this.btnON.Size = new System.Drawing.Size(65, 23);
            this.btnON.TabIndex = 4;
            this.btnON.Text = "オ　ン";
            this.btnON.UseVisualStyleBackColor = true;
            this.btnON.Click += new System.EventHandler(this.btnON_Click);
            // 
            // btnOFF
            // 
            this.btnOFF.Location = new System.Drawing.Point(207, 218);
            this.btnOFF.Name = "btnOFF";
            this.btnOFF.Size = new System.Drawing.Size(65, 23);
            this.btnOFF.TabIndex = 5;
            this.btnOFF.Text = "オ　フ";
            this.btnOFF.UseVisualStyleBackColor = true;
            this.btnOFF.Click += new System.EventHandler(this.btnOFF_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 247);
            this.Controls.Add(this.btnOFF);
            this.Controls.Add(this.btnON);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.nudNumber);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "ECHONET Lite テスト";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.NumericUpDown nudNumber;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnON;
        private System.Windows.Forms.Button btnOFF;
    }
}

