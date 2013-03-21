using System;

using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Views;
using Android.Content;
using Android.Util;

using graphicP2_test;

namespace graphicP2_test
{
	class GLView1 : AndroidGameView
	{
		private CustomMatrix DisplayObject;
		public bool fill {get;set;}
		float[] PointsArray;
		int[] Triangles;
		float[] ColorsArray;
		public float RX =0.0f;
		public float RY =0.0f;
		public float Scale =0.0f;

		public GLView1 (Context context) : base (context)
		{
			DisplayObject = new CustomMatrix ();
			DisplayObject.LoadMatrix(Context.Assets.Open(@"Projections/Dolphin.txt"));
			setupObject ("Dolphin");
			PointsArray = DisplayObject.CreateVertex ();
			Triangles = DisplayObject.CreateFaceArray();
			ColorsArray = new float[]{1.0f, 0.0f, 0.0f};
		}

		// This gets called when the drawing surface is ready
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			Run ();
		}

		protected override void CreateFrameBuffer ()
		{
			try {
				Log.Verbose ("GLCube", "Loading with default settings");

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer ();
				return;
			} catch (Exception ex) {
				Log.Verbose ("GLCube", "{0}", ex);
			}

			// this is a graphics setting that sets everything to the lowest mode possible so
			// the device returns a reliable graphics setting.
			try {
				Log.Verbose ("GLCube", "Loading with custom Android settings (low mode)");
				GraphicsMode = new AndroidGraphicsMode (0, 0, 0, 0, 0, false);

				// if you don't call this, the context won't be created
				base.CreateFrameBuffer ();
				return;
			} catch (Exception ex) {
				Log.Verbose ("GLCube", "{0}", ex);
			}
			throw new Exception ("Can't load egl, aborting");
		}

		public bool move(float X,float Y)
		{
			RX += X;
			RY += Y;
			return true;
		}

		private void setupObject(string name)
		{
			switch (name) {
			case "Cube":
				Scale =0.5f;
				break;
			case "Dolphin":
				Scale =0.003f;
				break;
			case "teapot":
				Scale =0.005f;
				break;
			case "T-Rex":
				Scale =1f;
				break;
			case "tiger":
				Scale =0.5f;
				break;
			}
		}

		public void Zoom(float scale)
		{
			Scale+=scale;
		}

		// This gets called on each frame render
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			GL.LoadIdentity ();
			GL.Enable(All.DepthTest);
			GL.FrontFace(All.Cw);
			GL.Enable(All.CullFace);

			//set up object size 

			GL.Scale(Scale,Scale,Scale);

			// rotation
			GL.Rotate (RX, 0f, 1f, 0f);
			GL.Rotate (RY, 1f, 0f, 0f);

			//clear the back ground
			GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);
			GL.Clear (ClearBufferMask.ColorBufferBit);
			GL.Clear(ClearBufferMask.DepthBufferBit);

			//enable 
			GL.EnableClientState (All.VertexArray);
			GL.EnableClientState (All.ColorArray);
			GL.ShadeModel(All.Smooth);//Smooth or Flat

			GL.VertexPointer(3, All.Float, 0, PointsArray);
			GL.ColorPointer(4, All.Float, 0,ColorsArray);

			//Toggle between wire frame and filled polygon
			if (fill)
			{
				GL.DrawElements(All.Triangles,Triangles.Length, All.UnsignedInt,Triangles);
			}else{
				GL.DrawElements(All.LineStrip,Triangles.Length, All.UnsignedInt,Triangles);
			}

			//disable
			GL.DisableClientState(All.VertexArray);
			GL.DisableClientState(All.ColorArray);

			SwapBuffers ();
		}

		public bool LoadNewMatrix(string name)
		{
			DisplayObject.LoadMatrix(Context.Assets.Open(@"Projections/"+name+".txt"));
			setupObject(name);
			PointsArray = DisplayObject.CreateVertex ();
			Triangles = DisplayObject.CreateFaceArray();
			return true;
		}

		public Context GetContext()
		{
			return Context;
		}
	}
}

