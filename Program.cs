// Developer :Youngshin Oh
// Last update: 2017, Apr 3
// Reference for EBNF tutorial and helper functions:
// http://blog.roboblob.com/2014/12/12/introduction-to-recursive-descent-parsers-with-csharp/

/*
 We will define the  EBNF Grammar first in order to tokenize the input.
 
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

using System;
using System.IO;



namespace SimplifyEquation
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			//Interactive mode
			if (args.Length == 0)
			{
				Console.WriteLine("Press CTRL+C to exit the program");
				while (true)
				{
					Console.WriteLine();
					Console.Write("Enter the equation: ");

					string input = Console.ReadLine();

					if (String.IsNullOrEmpty(input))
						continue;

					Console.WriteLine("Transformed Canonical Form: ");

					try
					{
						var tokenizer = new Tokenizer(input);
						var tokens = tokenizer.Scan();
						var parser = new Parser(tokens);
						var res = parser.ParseEquation();

						Console.Write(res);
						Console.WriteLine();
					}
					catch (EqException)
					{
						Console.WriteLine("=> Enter a valid equation <=");
					}
				}

			}

			//File input mode
			else
			{
				string filename = args[0];
				string input;

				StreamReader inputFile = new StreamReader(filename);
				StreamWriter outputFile = new StreamWriter(filename + ".out");

				while ((input = inputFile.ReadLine()) != null)
				{
					try
					{
						var tokenizer = new Tokenizer(input);
						var tokens = tokenizer.Scan();
						var parser = new Parser(tokens);
						var res = parser.ParseEquation();

						outputFile.WriteLine(res);
					}
					catch (EqException)
					{
						Console.WriteLine("=> Enter a valid equation <=");
					}

				}
				inputFile.Close();
				outputFile.Flush();
				outputFile.Close();

				Console.WriteLine("File processing finished");
			}

		}
	}




}

