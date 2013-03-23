namespace The_Famous_Perspective_Cube
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.scrollBarFocalLength = new System.Windows.Forms.VScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.scrollBarZ = new System.Windows.Forms.VScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelFvalue = new System.Windows.Forms.Label();
            this.labelXvalue = new System.Windows.Forms.Label();
            this.labelZvalue = new System.Windows.Forms.Label();
            this.buttonShowPoints = new System.Windows.Forms.Button();
            this.scrollBarY = new System.Windows.Forms.VScrollBar();
            this.label4 = new System.Windows.Forms.Label();
            this.labelYValue = new System.Windows.Forms.Label();
            this.scrollBarX = new System.Windows.Forms.HScrollBar();
            this.showPointsBox = new System.Windows.Forms.TextBox();
            this.XRotateBar = new System.Windows.Forms.HScrollBar();
            this.labelYRotate = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.YRotateBar = new System.Windows.Forms.VScrollBar();
            this.lableZRotate = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ZRotateBar = new System.Windows.Forms.VScrollBar();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.CubeButton = new System.Windows.Forms.Button();
            this.TiggerButton = new System.Windows.Forms.Button();
            this.DolphinButton = new System.Windows.Forms.Button();
            this.TRexButton = new System.Windows.Forms.Button();
            this.TeapotButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxG = new System.Windows.Forms.TextBox();
            this.textBoxB = new System.Windows.Forms.TextBox();
            this.textBoxR = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.FillcheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // scrollBarFocalLength
            // 
            this.scrollBarFocalLength.Location = new System.Drawing.Point(1180, 104);
            this.scrollBarFocalLength.Maximum = 750;
            this.scrollBarFocalLength.Minimum = 50;
            this.scrollBarFocalLength.Name = "scrollBarFocalLength";
            this.scrollBarFocalLength.Size = new System.Drawing.Size(25, 426);
            this.scrollBarFocalLength.TabIndex = 0;
            this.scrollBarFocalLength.Value = 200;
            this.scrollBarFocalLength.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarFocalLength_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1176, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "F";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 547);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "X";
            // 
            // scrollBarZ
            // 
            this.scrollBarZ.Location = new System.Drawing.Point(1205, 104);
            this.scrollBarZ.Maximum = 200;
            this.scrollBarZ.Name = "scrollBarZ";
            this.scrollBarZ.Size = new System.Drawing.Size(25, 426);
            this.scrollBarZ.TabIndex = 4;
            this.scrollBarZ.Value = 1;
            this.scrollBarZ.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarZ_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1201, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Z";
            // 
            // labelFvalue
            // 
            this.labelFvalue.AutoSize = true;
            this.labelFvalue.Location = new System.Drawing.Point(1177, 79);
            this.labelFvalue.Name = "labelFvalue";
            this.labelFvalue.Size = new System.Drawing.Size(13, 13);
            this.labelFvalue.TabIndex = 6;
            this.labelFvalue.Text = "0";
            // 
            // labelXvalue
            // 
            this.labelXvalue.AutoSize = true;
            this.labelXvalue.Location = new System.Drawing.Point(8, 567);
            this.labelXvalue.Name = "labelXvalue";
            this.labelXvalue.Size = new System.Drawing.Size(13, 13);
            this.labelXvalue.TabIndex = 7;
            this.labelXvalue.Text = "0";
            // 
            // labelZvalue
            // 
            this.labelZvalue.AutoSize = true;
            this.labelZvalue.Location = new System.Drawing.Point(1202, 79);
            this.labelZvalue.Name = "labelZvalue";
            this.labelZvalue.Size = new System.Drawing.Size(13, 13);
            this.labelZvalue.TabIndex = 8;
            this.labelZvalue.Text = "0";
            // 
            // buttonShowPoints
            // 
            this.buttonShowPoints.Location = new System.Drawing.Point(1241, 558);
            this.buttonShowPoints.Name = "buttonShowPoints";
            this.buttonShowPoints.Size = new System.Drawing.Size(75, 23);
            this.buttonShowPoints.TabIndex = 9;
            this.buttonShowPoints.Text = "Show Points";
            this.buttonShowPoints.UseVisualStyleBackColor = true;
            this.buttonShowPoints.Click += new System.EventHandler(this.buttonShowPoints_Click);
            // 
            // scrollBarY
            // 
            this.scrollBarY.Location = new System.Drawing.Point(1155, 104);
            this.scrollBarY.Maximum = 200;
            this.scrollBarY.Name = "scrollBarY";
            this.scrollBarY.Size = new System.Drawing.Size(25, 426);
            this.scrollBarY.TabIndex = 2;
            this.scrollBarY.Value = 1;
            this.scrollBarY.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarY_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1151, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Y";
            // 
            // labelYValue
            // 
            this.labelYValue.AutoSize = true;
            this.labelYValue.Location = new System.Drawing.Point(1152, 79);
            this.labelYValue.Name = "labelYValue";
            this.labelYValue.Size = new System.Drawing.Size(13, 13);
            this.labelYValue.TabIndex = 8;
            this.labelYValue.Text = "0";
            // 
            // scrollBarX
            // 
            this.scrollBarX.Location = new System.Drawing.Point(44, 559);
            this.scrollBarX.Maximum = 200;
            this.scrollBarX.Name = "scrollBarX";
            this.scrollBarX.Size = new System.Drawing.Size(1186, 23);
            this.scrollBarX.TabIndex = 2;
            this.scrollBarX.Value = 42;
            this.scrollBarX.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarX_Scroll);
            // 
            // showPointsBox
            // 
            this.showPointsBox.Location = new System.Drawing.Point(8, 585);
            this.showPointsBox.Name = "showPointsBox";
            this.showPointsBox.Size = new System.Drawing.Size(1315, 20);
            this.showPointsBox.TabIndex = 12;
            // 
            // XRotateBar
            // 
            this.XRotateBar.Location = new System.Drawing.Point(44, 536);
            this.XRotateBar.Maximum = 360;
            this.XRotateBar.Name = "XRotateBar";
            this.XRotateBar.Size = new System.Drawing.Size(1186, 23);
            this.XRotateBar.TabIndex = 15;
            this.XRotateBar.Value = 180;
            this.XRotateBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.XRotate);
            // 
            // labelYRotate
            // 
            this.labelYRotate.AutoSize = true;
            this.labelYRotate.Location = new System.Drawing.Point(1248, 79);
            this.labelYRotate.Name = "labelYRotate";
            this.labelYRotate.Size = new System.Drawing.Size(13, 13);
            this.labelYRotate.TabIndex = 20;
            this.labelYRotate.Text = "0";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelY.Location = new System.Drawing.Point(1247, 59);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(20, 20);
            this.labelY.TabIndex = 23;
            this.labelY.Text = "Y";
            // 
            // YRotateBar
            // 
            this.YRotateBar.Location = new System.Drawing.Point(1251, 104);
            this.YRotateBar.Maximum = 360;
            this.YRotateBar.Name = "YRotateBar";
            this.YRotateBar.Size = new System.Drawing.Size(25, 426);
            this.YRotateBar.TabIndex = 16;
            this.YRotateBar.Value = 1;
            this.YRotateBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.YRotate);
            // 
            // lableZRotate
            // 
            this.lableZRotate.AutoSize = true;
            this.lableZRotate.Location = new System.Drawing.Point(1298, 79);
            this.lableZRotate.Name = "lableZRotate";
            this.lableZRotate.Size = new System.Drawing.Size(13, 13);
            this.lableZRotate.TabIndex = 21;
            this.lableZRotate.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1297, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 20);
            this.label9.TabIndex = 18;
            this.label9.Text = "Z";
            // 
            // ZRotateBar
            // 
            this.ZRotateBar.Location = new System.Drawing.Point(1301, 104);
            this.ZRotateBar.Maximum = 360;
            this.ZRotateBar.Minimum = 1;
            this.ZRotateBar.Name = "ZRotateBar";
            this.ZRotateBar.Size = new System.Drawing.Size(25, 426);
            this.ZRotateBar.TabIndex = 17;
            this.ZRotateBar.Value = 1;
            this.ZRotateBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ZRotate);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(1151, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 20);
            this.label11.TabIndex = 24;
            this.label11.Text = "Move";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(1247, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 20);
            this.label12.TabIndex = 25;
            this.label12.Text = "Rotate";
            // 
            // CubeButton
            // 
            this.CubeButton.Location = new System.Drawing.Point(8, 12);
            this.CubeButton.Name = "CubeButton";
            this.CubeButton.Size = new System.Drawing.Size(75, 23);
            this.CubeButton.TabIndex = 26;
            this.CubeButton.Text = "Cube";
            this.CubeButton.UseVisualStyleBackColor = true;
            this.CubeButton.Click += new System.EventHandler(this.LoadCube);
            // 
            // TiggerButton
            // 
            this.TiggerButton.Location = new System.Drawing.Point(89, 12);
            this.TiggerButton.Name = "TiggerButton";
            this.TiggerButton.Size = new System.Drawing.Size(75, 23);
            this.TiggerButton.TabIndex = 27;
            this.TiggerButton.Text = "Tigger";
            this.TiggerButton.UseVisualStyleBackColor = true;
            this.TiggerButton.Click += new System.EventHandler(this.LoadTigger);
            // 
            // DolphinButton
            // 
            this.DolphinButton.Location = new System.Drawing.Point(170, 13);
            this.DolphinButton.Name = "DolphinButton";
            this.DolphinButton.Size = new System.Drawing.Size(75, 23);
            this.DolphinButton.TabIndex = 28;
            this.DolphinButton.Text = "Dolphin";
            this.DolphinButton.UseVisualStyleBackColor = true;
            this.DolphinButton.Click += new System.EventHandler(this.LoadDolphin);
            // 
            // TRexButton
            // 
            this.TRexButton.Location = new System.Drawing.Point(251, 13);
            this.TRexButton.Name = "TRexButton";
            this.TRexButton.Size = new System.Drawing.Size(75, 23);
            this.TRexButton.TabIndex = 29;
            this.TRexButton.Text = "T Rex";
            this.TRexButton.UseVisualStyleBackColor = true;
            this.TRexButton.Click += new System.EventHandler(this.LoadTRex);
            // 
            // TeapotButton
            // 
            this.TeapotButton.Location = new System.Drawing.Point(332, 12);
            this.TeapotButton.Name = "TeapotButton";
            this.TeapotButton.Size = new System.Drawing.Size(75, 23);
            this.TeapotButton.TabIndex = 30;
            this.TeapotButton.Text = "Teapot";
            this.TeapotButton.UseVisualStyleBackColor = true;
            this.TeapotButton.Click += new System.EventHandler(this.LoadTeapot);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(413, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 20);
            this.label8.TabIndex = 31;
            this.label8.Text = "Color";
            // 
            // textBoxG
            // 
            this.textBoxG.Location = new System.Drawing.Point(541, 12);
            this.textBoxG.Name = "textBoxG";
            this.textBoxG.Size = new System.Drawing.Size(63, 20);
            this.textBoxG.TabIndex = 33;
            this.textBoxG.Text = "100";
            // 
            // textBoxB
            // 
            this.textBoxB.Location = new System.Drawing.Point(610, 12);
            this.textBoxB.Name = "textBoxB";
            this.textBoxB.Size = new System.Drawing.Size(63, 20);
            this.textBoxB.TabIndex = 34;
            this.textBoxB.Text = "100";
            // 
            // textBoxR
            // 
            this.textBoxR.Location = new System.Drawing.Point(472, 12);
            this.textBoxR.Name = "textBoxR";
            this.textBoxR.Size = new System.Drawing.Size(63, 20);
            this.textBoxR.TabIndex = 35;
            this.textBoxR.Text = "100";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(679, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 36;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.setcolor);
            // 
            // FillcheckBox
            // 
            this.FillcheckBox.AutoSize = true;
            this.FillcheckBox.Checked = true;
            this.FillcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FillcheckBox.Location = new System.Drawing.Point(771, 15);
            this.FillcheckBox.Name = "FillcheckBox";
            this.FillcheckBox.Size = new System.Drawing.Size(38, 17);
            this.FillcheckBox.TabIndex = 37;
            this.FillcheckBox.Text = "Fill";
            this.FillcheckBox.UseVisualStyleBackColor = true;
            this.FillcheckBox.CheckedChanged += new System.EventHandler(this.toggleFill);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 617);
            this.Controls.Add(this.FillcheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxR);
            this.Controls.Add(this.textBoxB);
            this.Controls.Add(this.textBoxG);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TeapotButton);
            this.Controls.Add(this.TRexButton);
            this.Controls.Add(this.DolphinButton);
            this.Controls.Add(this.TiggerButton);
            this.Controls.Add(this.CubeButton);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.XRotateBar);
            this.Controls.Add(this.labelYRotate);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.YRotateBar);
            this.Controls.Add(this.lableZRotate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ZRotateBar);
            this.Controls.Add(this.showPointsBox);
            this.Controls.Add(this.scrollBarX);
            this.Controls.Add(this.labelYValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.scrollBarY);
            this.Controls.Add(this.buttonShowPoints);
            this.Controls.Add(this.labelZvalue);
            this.Controls.Add(this.labelXvalue);
            this.Controls.Add(this.labelFvalue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.scrollBarZ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scrollBarFocalLength);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar scrollBarFocalLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.VScrollBar scrollBarZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelFvalue;
        private System.Windows.Forms.Label labelXvalue;
        private System.Windows.Forms.Label labelZvalue;
        private System.Windows.Forms.Button buttonShowPoints;
        private System.Windows.Forms.VScrollBar scrollBarY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelYValue;
        private System.Windows.Forms.HScrollBar scrollBarX;
        private System.Windows.Forms.TextBox showPointsBox;
        private System.Windows.Forms.HScrollBar XRotateBar;
        private System.Windows.Forms.Label labelYRotate;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.VScrollBar YRotateBar;
        private System.Windows.Forms.Label lableZRotate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.VScrollBar ZRotateBar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button CubeButton;
        private System.Windows.Forms.Button TiggerButton;
        private System.Windows.Forms.Button DolphinButton;
        private System.Windows.Forms.Button TRexButton;
        private System.Windows.Forms.Button TeapotButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxG;
        private System.Windows.Forms.TextBox textBoxB;
        private System.Windows.Forms.TextBox textBoxR;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox FillcheckBox;
    }
}

