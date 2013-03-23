using System;
using NUnit.Framework;
using BusinessControlSolutionsTest;

namespace UnitTests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void IntergerTest()
		{
			Assert.AreEqual("1", MainClass.devisibleByThree(1));
		}

		[Test()]
		public void MultipleOfThreeTest ()
		{
			Assert.AreEqual("Fizz", MainClass.devisibleByThree(3));
		}

		[Test()]
		public void MultipleOfFiveTest ()
		{
			Assert.AreEqual("Buzz", MainClass.devisibleByThree(55));
		}

		[Test()]
		public void MultipleOfBothTest ()
		{
			Assert.AreEqual("FizzBuzz", MainClass.devisibleByThree(15));
		}
	}
}

