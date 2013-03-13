using System;
using NUnit.Framework;
using reverser;

namespace Tests
{
	[TestFixture()]
	public class Test
	{
		/*##########################
		Tests cant write to file system
		##########################*/
		[Test()]
		public void ReverseTest ()
		{
			Assert.AreSame("somthing secondline\n something firstline\n",reverser.MainClass.reverse("something firstline\n  something secondline\n"));
		}

		[Test()]
		public void LoadFileTest()
		{
			//TODO make test file
			Assert.AreSame("something firstline\n  something secondline\n",reverser.MainClass.LoadFlie("testfile.txt"));
			//TODO destroy test file
		}

		[Test()]
		public void LoadFileExptionHandelingTest ()
		{
			Assert.Catch(reverser.MainClass.LoadFlie("NonexistantFile"));
		}

		[Test()]
		public void SaveFileTest()
		{
			//TODO test if file saved to the right name
			//TODO destroy file from above
		}

		public void SaveFileTestExecptionTest ()
		{

		}
	}
}

