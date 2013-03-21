using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace The_Famous_Perspective_Cube
{
    public partial class Form1 : Form
    {
        public CustomMatrix DisplayObject;
        public string filename = @"objects\Cube.txt";
        public double[,] scalingMatrix; // since the animals are all diff sizes this is set on load;

        public Form1()
        {
            DisplayObject = new CustomMatrix() { 
                                                lineColor = Color.Black
                                                ,fillColor = Color.FromArgb(100,100,100)
                                                ,penThickness = 1 
                                               };
            DisplayObject.LoadMatrix(filename);
            InitializeComponent();
            scalingMatrix = Dmatrix.ScalingMatrix(1, 1, 1);
            transformAndDrawAll();
        }

        void updateLabels()
        {
            labelFvalue.Text = Convert.ToString(scrollBarFocalLength.Value);
            labelXvalue.Text = Convert.ToString(scrollBarX.Value);
            labelZvalue.Text = Convert.ToString(scrollBarZ.Value);
            labelYValue.Text = Convert.ToString(scrollBarY.Value);
            labelYRotate.Text = Convert.ToString(YRotateBar.Value);
            lableZRotate.Text = Convert.ToString(ZRotateBar.Value);
        }

        void transformCube()
        {
            DisplayObject.ResetPoints();
            DisplayObject.Transform(Dmatrix.TranslationMatrix(scrollBarX.Value, scrollBarY.Value, scrollBarZ.Value));
            DisplayObject.Transform(Dmatrix.PerspectiveMatrix(scrollBarFocalLength.Value));
            DisplayObject.Transform(scalingMatrix);
            DisplayObject.Transform(Dmatrix.RotateXmatrix(XRotateBar.Value));
            DisplayObject.Transform(Dmatrix.RotateYmatrix(YRotateBar.Value));
            DisplayObject.Transform(Dmatrix.RotateZmatrix(ZRotateBar.Value));
        }

        void drawCube()
        {
            Graphics g = CreateGraphics();
            g.Clear(Color.Tan);
           if(FillcheckBox.Checked)
            DisplayObject.drawAsFilled(g);

            DisplayObject.draw(g);
        }

        void transformAndDrawAll()
        {
            transformCube();
            drawCube();
            updateLabels();
        }
		private CustomMatrix NewCustomMatrix()
		{
			return new CustomMatrix()
			{
				fillColor=Color.FromArgb(Convert.ToInt32(textBoxR.Text), Convert.ToInt32(textBoxG.Text), Convert.ToInt32(textBoxB.Text))
					,lineColor=Color.Black
						,penThickness =1
			};
		}

        #region user inputs

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            transformAndDrawAll();
        }

        private void scrollBarFocalLength_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate(); //Invalidate forces a Form Paint event.
        }

        private void scrollBarX_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate(); //Invalidate forces a Form Paint event.
        }

        private void scrollBarY_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void scrollBarZ_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate(); //Invalidate forces a Form Paint event.
        }

        private void buttonShowPoints_Click(object sender, EventArgs e)
        {
            showPointsBox.Text = DisplayObject.ToString();
        }

        private void YRotate(object sender, ScrollEventArgs e)
        {
            Invalidate(); //Invalidate forces a Form Paint event.
        }

        private void ZRotate(object sender, ScrollEventArgs e)
        {
            Invalidate(); //Invalidate forces a Form Paint event.
        }

        private void XRotate(object sender, ScrollEventArgs e)
        {
            Invalidate(); //Invalidate forces a Form Paint event.
        }

        private void LoadCube(object sender, EventArgs e)
        {
			DisplayObject = NewCustomMatrix();
            DisplayObject.LoadMatrix(@"objects\Cube.txt");
            scalingMatrix= Dmatrix.ScalingMatrix(1,1,1);
            transformAndDrawAll();
        }

        private void LoadTigger(object sender, EventArgs e)
        {
			DisplayObject = NewCustomMatrix();
            DisplayObject.LoadMatrix(@"objects\tigger.txt");
            scalingMatrix = Dmatrix.ScalingMatrix(1, 1, 1);
            transformAndDrawAll();
        }

        private void LoadDolphin(object sender, EventArgs e)
        {
			DisplayObject =NewCustomMatrix();
            DisplayObject.LoadMatrix(@"objects\Dolphin.txt");
            scalingMatrix = Dmatrix.ScalingMatrix(1, 1, 1);
            transformAndDrawAll();
        }

        private void LoadTRex(object sender, EventArgs e)
        {
			DisplayObject = NewCustomMatrix();
            DisplayObject.LoadMatrix(@"objects\T-Rex.txt");
            scalingMatrix = Dmatrix.ScalingMatrix(10, 10, 10);
        }

        private void LoadTeapot(object sender, EventArgs e)
        {
			DisplayObject = NewCustomMatrix();
            DisplayObject.LoadMatrix(@"objects\teapot.txt");
            scalingMatrix = Dmatrix.ScalingMatrix(10, 10, 10);
            transformAndDrawAll();
        }

        private void setcolor(object sender, EventArgs e)
        {
            DisplayObject.fillColor = Color.FromArgb(Convert.ToInt32(textBoxR.Text), Convert.ToInt32(textBoxG.Text), Convert.ToInt32(textBoxB.Text));
            transformAndDrawAll();
        }

        private void toggleFill(object sender, EventArgs e)
        {
            transformAndDrawAll();
        }
        #endregion
    }
}