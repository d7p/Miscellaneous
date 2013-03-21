using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gestures;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using graphicP2_test;

namespace graphicP2_test
{
	[Activity (Label = "Soft333",
				ConfigurationChanges=ConfigChanges.Orientation | ConfigChanges.KeyboardHidden,
				MainLauncher = true)]
	public class Activity1 : Activity, GestureDetector.IOnGestureListener
	{
		GLView1 view;
		private GestureDetector _gestureDetector;

		public override bool OnTouchEvent(MotionEvent e)
		{
			_gestureDetector.OnTouchEvent(e);
			return false;
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_gestureDetector = new GestureDetector(this);
			view = new GLView1 (this);
			SetContentView (view);
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			view.Pause ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			view.Resume ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();
		}

		public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			if (e2.PointerCount>1)
			{
				float distance = (float)distanceX/100;
				view.Zoom(distance);
			}
			else
			{
				view.move (distanceX, distanceY);
			}
			return true;
		}

		public override bool OnCreateOptionsMenu(IMenu menu) {
			MenuInflater inflater = new MenuInflater(view.GetContext());
			inflater.Inflate(Resource.Layout.Menu,menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.Cube:
				return view.LoadNewMatrix("Cube");
			case Resource.Id.Dolphin:
				return view.LoadNewMatrix("Dolphin");
				case Resource.Id.teapot:
				return view.LoadNewMatrix("teapot");
			case Resource.Id.TRex:
				return view.LoadNewMatrix("T-Rex");
			case Resource.Id.tiger:
				return view.LoadNewMatrix("tiger");
			case Resource.Id.fill:
				return view.fill = view.fill?false:true;
			default:
				return false;
			}
		}

		//####### Unused user Inputs needed to compleate interface ############################################
		public bool OnDown(MotionEvent e){return true;}
		
		public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY){return true;}
		
		public void OnLongPress(MotionEvent e) {}
		
		public void OnShowPress(MotionEvent e) {}
		
		public bool OnSingleTapUp(MotionEvent e){return true;}

	}
}


