using System;


//Reference:http://blog.roboblob.com/2014/12/12/introduction-to-recursive-descent-parsers-with-csharp/


namespace SimplifyEquation
{
	public abstract class Token
	{
	}
	public class OperatorToken : Token
	{
	}
	public class PlusToken : OperatorToken
	{
	}
	public class MinusToken : OperatorToken
	{
	}
	public class EqualToken : OperatorToken
	{
	}
	public class ParenthesisToken : Token
	{
	}
	public class OpenParenthesisToken : ParenthesisToken
	{
	}
	public class ClosedParenthesisToken : ParenthesisToken
	{
	}
	public class TermToken : Token
	{
		private float coefficient;
		private string variable;

		public TermToken(float coef, string vari)
		{
			this.coefficient = coef;
			this.variable = vari;
		}


		public float Coefficient
		{
			get { return coefficient; }
		
		}
		public string Variable
		{
			get { return variable; }

		}
	}

}
