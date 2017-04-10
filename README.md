# SimplifyEquation

This program takes an equation that is defined using EBNF grammar as follows
 
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
              
Then it simplifies the equation into canonical form. 
