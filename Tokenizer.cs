using System;
using System.Collections.Generic;

/*
 Equation   := Expresssion "=" Expresssion

Expression := [ "-" ] Term { ("+" | "-") Term }
Term       := RealNumber | RealNumber Variable | "(" Expression ")"
Variable   := Letter [exponent] {Letter [exponent]} 
exponent   := "^" Integer
RealNumber := Digit {Digit} | {Digit} "." {Digit}
Integer    := ["-"] Digit {Digit}
Digit      := "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
Letter     := "A" | "B" | "C" | "D" | "E" | "F" | "G"
              | "H" | "I" | "J" | "K" | "L" | "M" | "N"
              | "O" | "P" | "Q" | "R" | "S" | "T" | "U"
              | "V" | "W" | "X" | "Y" | "Z" | "a" | "b"
              | "c" | "d" | "e" | "f" | "g" | "h" | "i"
              | "j" | "k" | "l" | "m" | "n" | "o" | "p"
              | "q" | "r" | "s" | "t" | "u" | "v" | "w"
              | "x" | "y" | "z" 
 */

namespace SimplifyEquation
{
	public class Tokenizer
	{
		private StringReader reader;

		public Tokenizer(string input)
		{
			reader = new StringReader(input);
		}

		public List<Token> Scan()
		{
			var tokens = new List<Token>();
			while (reader.ThereAreMoreChar)
			{
				char c = reader.Peek();
				if (Char.IsWhiteSpace(c))
				{
					reader.Read();
					continue;
				}
				if (Char.IsDigit(c) | char.IsLetter(c))
				{
					var tr = TokenizeTerm();
					tokens.Add(new TermToken(tr.Item1, tr.Item2));
				}
				else if (c == '-')
				{
					tokens.Add(new MinusToken());
					reader.Read();
				}
				else if (c == '+')
				{
					tokens.Add(new PlusToken());
					reader.Read();
				}
				else if (c == '(')
				{
					tokens.Add(new OpenParenthesisToken());
					reader.Read();
				}
				else if (c == ')')
				{
					tokens.Add(new ClosedParenthesisToken());
					reader.Read();
				}
				else if (c == '=')
				{
					tokens.Add(new EqualToken());
					reader.Read();
				}
				else
					throw new Exception("Unknown character in expression: " + c);

			}
			return tokens;
		}

		private Tuple<float, string> TokenizeTerm()
		{
			string realNum = null;
			var variable = new List<Tuple<char, int>>();
			var exponent = "";

			// We tokenize the coefficient 
			while (reader.ThereAreMoreChar && (char.IsNumber(reader.Peek()) | reader.Peek() == '.'))
			{
				var item = reader.Read();
				realNum += item;
			}
			while (reader.ThereAreMoreChar && (char.IsWhiteSpace(reader.Peek())))
			{
				reader.Read();
			}

			// We expect a letter for a variable or a plus or minus or equal operator after coefficient
			if (reader.ThereAreMoreChar && (!(char.IsLetter(reader.Peek())) && reader.Peek() != '+' && reader.Peek() != '-' && reader.Peek() != '=' && reader.Peek() != ')'))
				throw new Exception(" Not a valid term ");
			
			// Now we tokenize the variable
			while (reader.ThereAreMoreChar &&(char.IsLetter(reader.Peek())))
			{
				var letter = reader.Read();
				if (reader.ThereAreMoreChar&& reader.Peek() == '^')
				{
					reader.Read();
					if (reader.ThereAreMoreChar && !char.IsNumber(reader.Peek()) && reader.Peek() != '-')
						throw new Exception(" Expected integer exponent but found:" + reader.Peek());
					else if (reader.ThereAreMoreChar && reader.Peek() == '-')
						exponent += reader.Read();
					while (reader.ThereAreMoreChar && char.IsNumber(reader.Peek()))
					{
						exponent += reader.Read();
					}

					variable.Add(Tuple.Create(letter, Int32.Parse(exponent)));
				}
				//else if (char.IsLetter(reader.Peek()))
				else
					variable.Add(Tuple.Create(letter, 1));
			}

			string strVar = SortVariableAndConvertToString(variable);

			if (realNum == null)
				return Tuple.Create(float.Parse("1"), strVar);

			else
				return Tuple.Create(float.Parse(realNum), strVar);
		}


		private string SortVariableAndConvertToString(List<Tuple<char, int>> var)
		{
			string strVar;
			var.Sort(
			(t1, t2) =>
			{
				int res = t1.Item1.CompareTo(t2.Item1);
				return res != 0 ? res : t2.Item2.CompareTo(t1.Item2);
			});

			strVar = ConverToString(var);
			return strVar;
		}

		private string ConverToString(List<Tuple<char, int>> var)
		{
			string strVar = "";
			foreach (Tuple<char, int> item in var)
			{
				strVar += item.Item1;
				if (item.Item2 != 1)
				{
					strVar += '^';
					strVar += item.Item2.ToString();
				}
			}
			return strVar;
		}


	}

}