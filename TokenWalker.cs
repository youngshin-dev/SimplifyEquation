using System;
using System.Collections.Generic;

//Reference:http://blog.roboblob.com/2014/12/12/introduction-to-recursive-descent-parsers-with-csharp/

namespace SimplifyEquation
{
	public class TokenWalker
	{
		private readonly List<Token> _tokens = new List<Token>();
		private int _currentIndex = -1;

		public TokenWalker(List<Token> input)
		{
			this._tokens = input; 
		}
		public bool ThereAreMoreTokens
		{
			get { return _currentIndex < _tokens.Count - 1; }
		}

		public Token GetNext()
		{
			MakeSureWeDontGoPastTheEnd();
			return _tokens[++_currentIndex];
		}
		private void MakeSureWeDontGoPastTheEnd()
		{
			if (!(_currentIndex < _tokens.Count - 1))
				throw new Exception("Cannot read pass the end of tokens list");
		}
		public Token PeekNext()
		{
			MakeSureWeDontPeekPastTheEnd();
			return _tokens[_currentIndex + 1];
		}

		private void MakeSureWeDontPeekPastTheEnd()
		{
			//if (!(_currentIndex + 1 >= _tokens.Count))
			//	throw new Exception("Cannot peek pass the end of tokens list");
			var weCanPeek = (_currentIndex + 1 < _tokens.Count);
			if (!weCanPeek)
				throw new Exception("Cannot peek pass the end of tokens list");
		}
		public bool IsNextOfType(Type type)
		{
			return PeekNext().GetType() == type;
		}


	}
}
