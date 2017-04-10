﻿// Developer :Youngshin Oh
// Last update: 2017, Apr 3

using System;
using System.Collections.Generic;


namespace SimplifyEquation
{
	public class Parser
	{
		//private List<Token> tokens=new List<Token>();
		private TokenWalker walker;
		private Dictionary<string, List<float>> right_dict = new Dictionary<string, List<float>>();
		private Dictionary<string, List<float>> left_dict = new Dictionary<string, List<float>>();
		private int leftHand = 1;

		public Parser(List<Token> input)
		{
			this.walker = new TokenWalker(input);
		}

		//Equation   := Expresssion "=" Expresssion
		public string ParseEquation()
		{
			string result = "";
			ParseExpression();
			if (walker.ThereAreMoreTokens && !walker.IsNextOfType(typeof(EqualToken)))
				throw new Exception("Expected an equal sign");
			walker.GetNext();
			leftHand = -1;
			ParseExpression();

			result = SumUpCoefficients();
			return result;
		}

		//Expression := [ "-" ] Term { ("+" | "-") Term }
		//temp stores Tuples of variable and coefficient in a term until we see closing bracket.
		//temp is passed by reference to ParseTerm and updated there.
		//Then at the end of ParseExpression, each variable key is updated with the coefficients collected in the temp
		//Then the coefficients are updated according to the sign infront of the opening barcket. 
		public void ParseExpression()
		{
			bool nextIsNegative = walker.ThereAreMoreTokens && walker.IsNextOfType(typeof(MinusToken));
			List<Tuple<string, float>> temp = new List<Tuple<string, float>>();
			//we assume that the first term is positive unless we see negative sign
			int neg = 1;
			if (nextIsNegative)
			{
				neg = -1;
				walker.GetNext();
			}
			ParseTerm(neg, temp);

			while (walker.ThereAreMoreTokens && (walker.IsNextOfType(typeof(MinusToken)) || walker.IsNextOfType(typeof(PlusToken))))
			{
				var op = walker.GetNext();
				if (op is PlusToken)
					ParseTerm(1, temp);
				else
					ParseTerm(-1, temp);
			}
			if (leftHand == 1)
			{
				foreach (Tuple<string, float> item in temp)
				{
					if (right_dict.ContainsKey(item.Item1))
						right_dict[item.Item1].Add(item.Item2);
					else
						right_dict.Add(item.Item1, new List<float> { item.Item2 });
				}
			}
			else
			{
				foreach (Tuple<string, float> item in temp)
				{
					if (left_dict.ContainsKey(item.Item1))
						left_dict[item.Item1].Add(item.Item2);
					else
						left_dict.Add(item.Item1, new List<float> { item.Item2 });
				}
			}
			if (leftHand == 1)
				DistributeTheSign(1, right_dict);
			else
				DistributeTheSign(-1, left_dict);

		}

		//Term       := RealNumber | RealNumber Variable | "(" Expression ")"
		public void ParseTerm(int op, List<Tuple<string, float>> temp)
		{
			if (walker.ThereAreMoreTokens && walker.IsNextOfType(typeof(TermToken)))
			{
				var tr = walker.GetNext();
				var term = tr as TermToken;
				//temp.Add(Tuple.Create(term.Variable, term.Coefficient * op * leftHand));
				temp.Add(Tuple.Create(term.Variable, term.Coefficient * op));
				return;
			}
			if (walker.ThereAreMoreTokens && !walker.IsNextOfType(typeof(OpenParenthesisToken)))
				throw new Exception("Expected a term or opening bracket");
			walker.GetNext();
			ParseExpression();
			//DistributeTheSign(op, leftHand);
			/*
			if (leftHand == 1)
				DistributeTheSign(op, right_dict);
			else
				DistributeTheSign(op, left_dict);
				*/
			if (walker.ThereAreMoreTokens && !walker.IsNextOfType(typeof(ClosedParenthesisToken)))
				throw new Exception("Expected a closing bracket");
			walker.GetNext();

			return;
		}


		/*private void DistributeTheSign(int neg, int left)
		{
			List<string> keys = new List<string>(dict.Keys);
			foreach (string key in keys)
			{
				for (int i = 0; i < dict[key].Count; i++)
				{
					float temp = dict[key][i];
					float newCoef = temp * neg * left;
					dict[key][i] = newCoef;
				}

			}
		}
		*/


		private void DistributeTheSign(int neg, Dictionary<string, List<float>> dictionary)
		{
			List<string> keys = new List<string>(dictionary.Keys);
			foreach (string key in keys)
			{
				for (int i = 0; i < dictionary[key].Count; i++)
				{
					float temp = dictionary[key][i];
					float newCoef = temp * neg;
					dictionary[key][i] = newCoef;
				}

			}
		}

		private string SumUpCoefficients()
		{
			string result = "";
			bool firstTerm = true;
			string str_sum = "";
			List<string> coeffs = new List<string>(); //This list is simply for checking if all coeffs sup up to 0 
			SortedSet<string> keys = new SortedSet<string>(right_dict.Keys); //This is a set of all keys from dict and left_dict
			keys.UnionWith(left_dict.Keys);
			foreach (string key in keys)
			{
				float sum = 0;

				if (right_dict.ContainsKey(key) && !left_dict.ContainsKey(key))
				{
					foreach (float coef in right_dict[key])
					{
						sum = sum + coef;
					}
				}
				else if (!right_dict.ContainsKey(key) && left_dict.ContainsKey(key))
				{
					foreach (float coef in left_dict[key])
					{
						sum = sum - coef;
					}
				}
				else if (right_dict.ContainsKey(key) && left_dict.ContainsKey(key))
				{
					foreach (float coef in right_dict[key])
					{
						sum = sum + coef;
					}
					foreach (float coef in left_dict[key])
					{
						sum = sum - coef;
					}
				}
				else
					throw new Exception("Key missing ");

				str_sum = Convert.ToString(sum);
				coeffs.Add(str_sum);
				if (firstTerm && str_sum == "0")  // if the first term is 0 then ignore.
					continue;
				else if (firstTerm && str_sum != "0")
				{
					MakeFirstTerm(key, str_sum, ref result);
					firstTerm = false;
				}
				else
					AttachStringTermToResult(key, str_sum, ref result);
				firstTerm = false;
			}

			if (CheckIfEveryThingCancels(coeffs))
				result = "0=0";
			else
				result += "=0";

			return result;
		}

		private bool CheckIfEveryThingCancels(List<string> coeffs)
		{
			bool everythingCancel = true;
			foreach (string coef in coeffs)
			{
				if (coef != "0")
					everythingCancel = false;
			}

			return everythingCancel;
		}


		private void MakeFirstTerm(string key, string str_sum, ref string result)
		{
			// Check the coefficient is not zero 
			if (str_sum != "0")
			{
				// Check that it is not a constant term
				if (key != "")
				{

					if (str_sum != "-1" && str_sum != "1")
						result += str_sum;
					else if (str_sum == "-1")
						result += "-";
					result += key;
				}
				else if (key == "")
					result += str_sum;
			}
		}

		private void AttachStringTermToResult(string key, string str_sum, ref string result)
		{

			// Check the coefficient is not zero 
			if (str_sum != "0")
			{
				// Check that it is not a constant term
				if (key != "")
				{
					// Check if the coefficient is  -1
					if (str_sum == "-1")
						result += '-';

					// Check if the coefficient is  1
					else if (str_sum == "1")
						result += '+';
					else
					{
						if (Convert.ToDouble(str_sum) >= 0.000000)
							result += '+';
						result += str_sum;
					}

					result += key;
				}

				else if (key == "")
				{
					// Check if the constant is 1
					if (Convert.ToDouble(str_sum) >= 0.000000)
						result += '+';
					result += str_sum;
				}
			}
		}



	}

}

