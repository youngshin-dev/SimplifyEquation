using System;

//Reference:http://blog.roboblob.com/2014/12/12/introduction-to-recursive-descent-parsers-with-csharp/

namespace SimplifyEquation
{
	public class StringReader
	{
		private string str;
		//private List<char> characters = new List<char>();
		private int currentIndex = -1;
		public StringReader(string value)
		{
			this.str = value;
			//characters = expression.ToList();
		}
		public bool ThereAreMoreChar
		{
			get { return currentIndex < str.Length - 1; }
		}

		public char Read()
		{
			ReadEndCheck();
			currentIndex = currentIndex + 1;
			return str[currentIndex];
			//return str[++currentIndex];
		}
		public char Peek()
		{
			PeekEndCheck();
			return str[currentIndex + 1];
		}

		private void ReadEndCheck()
		{
			//if (!(currentIndex <= (str.Length - 2)))
			//throw new Exception("Cannot read pass the end");
			if (!ThereAreMoreChar)
				throw new Exception("Cannot read pass the end");
		}
		private void PeekEndCheck()
		{
			//if (!(currentIndex <= str.Length-2))
			//	throw new Exception("Cannot peek pass the end");


			var weCanPeek = (currentIndex + 1 < str.Length);
			if (!weCanPeek)
				throw new Exception("Cannot peek pass the end of tokens list");
		}
	}
}
