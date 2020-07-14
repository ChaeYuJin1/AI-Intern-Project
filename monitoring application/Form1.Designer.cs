namespace helmet_manager
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbi1 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pbi2 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pbi3 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.pbi4 = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.bgW = new System.ComponentModel.BackgroundWorker();
            this.lb_state1 = new System.Windows.Forms.Label();
            this.lb_state2 = new System.Windows.Forms.Label();
            this.lb_state3 = new System.Windows.Forms.Label();
            this.lb_state4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbi1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbi2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbi3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbi4)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbi1
            // 
            this.pbi1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pbi1.Location = new System.Drawing.Point(12, 20);
            this.pbi1.Name = "pbi1";
            this.pbi1.Size = new System.Drawing.Size(640, 360);
            this.pbi1.TabIndex = 0;
            this.pbi1.TabStop = false;
            // 
            // pbi2
            // 
            this.pbi2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pbi2.Location = new System.Drawing.Point(658, 20);
            this.pbi2.Name = "pbi2";
            this.pbi2.Size = new System.Drawing.Size(640, 360);
            this.pbi2.TabIndex = 1;
            this.pbi2.TabStop = false;
            // 
            // pbi3
            // 
            this.pbi3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pbi3.Location = new System.Drawing.Point(12, 386);
            this.pbi3.Name = "pbi3";
            this.pbi3.Size = new System.Drawing.Size(640, 360);
            this.pbi3.TabIndex = 0;
            this.pbi3.TabStop = false;
            // 
            // pbi4
            // 
            this.pbi4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pbi4.Location = new System.Drawing.Point(658, 386);
            this.pbi4.Name = "pbi4";
            this.pbi4.Size = new System.Drawing.Size(640, 360);
            this.pbi4.TabIndex = 0;
            this.pbi4.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_clear);
            this.groupBox1.Controls.Add(this.btn_save);
            this.groupBox1.Controls.Add(this.tbOutput);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(1304, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 734);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "알람 로그";
            // 
            // btn_clear
            // 
            this.btn_clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_clear.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_clear.Location = new System.Drawing.Point(151, 675);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(101, 47);
            this.btn_clear.TabIndex = 2;
            this.btn_clear.Text = "로그 삭제";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_save
            // 
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_save.Location = new System.Drawing.Point(17, 675);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(101, 47);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "로그 저장";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.BackColor = System.Drawing.Color.Black;
            this.tbOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbOutput.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbOutput.ForeColor = System.Drawing.Color.White;
            this.tbOutput.Location = new System.Drawing.Point(6, 20);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.Size = new System.Drawing.Size(256, 643);
            this.tbOutput.TabIndex = 0;
            this.tbOutput.TabStop = false;
            // 
            // lb_state1
            // 
            this.lb_state1.AutoSize = true;
            this.lb_state1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lb_state1.Font = new System.Drawing.Font("휴먼둥근헤드라인", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_state1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lb_state1.Location = new System.Drawing.Point(230, 180);
            this.lb_state1.Name = "lb_state1";
            this.lb_state1.Size = new System.Drawing.Size(198, 41);
            this.lb_state1.TabIndex = 3;
            this.lb_state1.Text = "이상 없음";
            // 
            // lb_state2
            // 
            this.lb_state2.AutoSize = true;
            this.lb_state2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lb_state2.Font = new System.Drawing.Font("휴먼둥근헤드라인", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_state2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lb_state2.Location = new System.Drawing.Point(891, 180);
            this.lb_state2.Name = "lb_state2";
            this.lb_state2.Size = new System.Drawing.Size(198, 41);
            this.lb_state2.TabIndex = 3;
            this.lb_state2.Text = "이상 없음";
            // 
            // lb_state3
            // 
            this.lb_state3.AutoSize = true;
            this.lb_state3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lb_state3.Font = new System.Drawing.Font("휴먼둥근헤드라인", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_state3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lb_state3.Location = new System.Drawing.Point(230, 534);
            this.lb_state3.Name = "lb_state3";
            this.lb_state3.Size = new System.Drawing.Size(198, 41);
            this.lb_state3.TabIndex = 3;
            this.lb_state3.Text = "이상 없음";
            // 
            // lb_state4
            // 
            this.lb_state4.AutoSize = true;
            this.lb_state4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lb_state4.Font = new System.Drawing.Font("휴먼둥근헤드라인", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_state4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lb_state4.Location = new System.Drawing.Point(891, 534);
            this.lb_state4.Name = "lb_state4";
            this.lb_state4.Size = new System.Drawing.Size(198, 41);
            this.lb_state4.TabIndex = 3;
            this.lb_state4.Text = "이상 없음";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.lb_state2);
            this.Controls.Add(this.lb_state4);
            this.Controls.Add(this.lb_state3);
            this.Controls.Add(this.lb_state1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbi2);
            this.Controls.Add(this.pbi4);
            this.Controls.Add(this.pbi3);
            this.Controls.Add(this.pbi1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.pbi1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbi2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbi3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbi4)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenCvSharp.UserInterface.PictureBoxIpl pbi1;
        private OpenCvSharp.UserInterface.PictureBoxIpl pbi2;
        private OpenCvSharp.UserInterface.PictureBoxIpl pbi3;
        private OpenCvSharp.UserInterface.PictureBoxIpl pbi4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker bgW;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Label lb_state1;
        private System.Windows.Forms.Label lb_state2;
        private System.Windows.Forms.Label lb_state3;
        private System.Windows.Forms.Label lb_state4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.SaveFileDialog sfd;
    }
}

