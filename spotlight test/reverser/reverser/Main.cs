using System;
using System.IO;

namespace reverser
{
	public class MainClass
	{
		public static void Main (string[] args)
		{
			string openFileName, saveFileName;
			//TODO split the Args into file names
			saveFileName = "";
			openFileName = "";

			//call methods to make magic happen
			SaveFile (reverse (LoadFlie (openFileName)), saveFileName);
			
		}

		public static string reverse (string text)
		{
			//List<string> linesArray = text.Split("\n\r");
			//return linesArray.Order().ToString();
			return text;
		}

		public static string LoadFlie (string filename)
		{
			try {
				using (var file = new FileStream(filename,FileMode.Open,FileAccess.Read)) {
					return file.ReadLine();
				}
			}
			catch (IOException e) {
				return "sorry cant open file";
			}

		}

		public static bool SaveFile (string text, string filenName)
		{
			return true;
		}
	}
}
