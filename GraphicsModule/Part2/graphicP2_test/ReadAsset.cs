using System;
using Java.IO;
using System.IO;
using Android.App;
using Android.OS;

namespace graphicP2_test
{
	public class ReadAsset : Activity
	{
		public ReadAsset(){
		}

		public StreamReader Load(string fileName){
			Stream file = Assets.Open("Cube.txt");//fileName);
			StreamReader fileStreamReader = new StreamReader(file);
			return fileStreamReader;
		}
	}
}