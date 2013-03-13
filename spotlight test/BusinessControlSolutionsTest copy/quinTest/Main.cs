using System;

namespace BusinessControlSolutionsTest
{
	public static class MainClass
	{
		public static void Main (string[] args)
		{
			for (int temp =0; temp <=100; temp++) 
			{
				Console.WriteLine(devisibleByThree(temp));
			}
		}

		public static string devisibleByThree(int number)
		{
			string result="";
			// check if temp is devisable by three
			if ((number%3)== 0 && number!=0)
			{
				result+="Fizz";

			}
			if ((number%5)==0 && number!=0)
			{
				result+="Buzz";
			}
			if(result == "")
			{
				result = number.ToString();
			}
			return result;
		}
}
}