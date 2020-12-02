using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPD_Compiler.LexiconHandler;

namespace LPD_Compiler.SemanticHandler
{
    public class Postfix
    {
        public List<String> expression = new List<String>();
        public List<String> convertedExpression = new List<String>();

        public Stack<String> stackSymbols = new Stack<String>();

        public Postfix()
        {

        }

        public Postfix(List<String> expression)
        {
            this.expression = expression;
        }

        public void clearExpression()
        {
            expression.Clear();
            convertedExpression.Clear();
            stackSymbols.Clear();
        }

        public void convertExpression()
        {
            int stackPrecedencia, currentPrecedencia;

            string caracter;
            for (var i = 0; i < expression.Count; i++)
            {
                caracter = expression[i];
                if (expression[i] == "+" || expression[i] == "-")
                {
                    if (i == 0 || (checkUnary(expression[i - 1])))
                        caracter += "u";
                }

                switch(caracter)
                {
                    case "+":
                    case "-":
                    case "nao":
                    case "*":
                    case "div":
                    case "<":
                    case "<=":
                    case ">":
                    case ">=":
                    case "=":
                    case "!=":
                    case "e":
                    case "ou":
                    case "+u":
                    case "-u":
                        if (stackSymbols.Count != 0)
                            if (menorPrecedenciaAB(caracter, stackSymbols.Peek()))
                            {
                                if(!(caracter == "nao") && !(stackSymbols.Peek() == "nao"))
                                {
                                    while (stackSymbols.Count != 0 && menorPrecedenciaAB(caracter, stackSymbols.Peek()))
                                    {
                                        convertedExpression.Add(stackSymbols.Pop());
                                    }
                                }
                                //desempilhaSimbolo(caracter);
                                stackSymbols.Push(caracter);
                            }
                            else
                                empilhaSimbolo(caracter);
                        else
                            if (menorPrecedenciaAB(caracter, null))
                                desempilhaSimbolo(caracter);
                            else
                                empilhaSimbolo(caracter);
                        break;
                    case "(":
                        stackSymbols.Push(caracter);
                        break;
                    case ")":
                        while(stackSymbols.Peek() != "(")
                        {
                            convertedExpression.Add(stackSymbols.Pop());
                        }
                        stackSymbols.Pop();
                        break;
                    default: convertedExpression.Add(caracter);
                        break;
                }
            }
            desempilhaTudo();
        }

        public void test()
        {
            List<String> test = new List<String>();

            test.Add("(");
            test.Add("a");
            test.Add("+");
            test.Add("b");
            test.Add(">");
            test.Add("3");
            test.Add(")");
            test.Add("e");
            test.Add("(");
            test.Add("c");
            test.Add(">");
            test.Add("d");
            test.Add(")");

            /*test.Add("(");
            test.Add("a");
            test.Add("+");
            test.Add("b");
            test.Add("+");
            test.Add("c");
            test.Add(")");
            test.Add("*");
            test.Add("d");*/

            /*test.Add("-");
            test.Add("3");
            test.Add("*");
            test.Add("4");*/

            this.expression = test;
            convertExpression();
        }

        public bool menorPrecedenciaAB(String caracterA, String caracterB)
        {
            int precedenciaA, precedenciaB;

            precedenciaA = checkPrecedencia(caracterA);
            precedenciaB = checkPrecedencia(caracterB);

            if (precedenciaB == -1)
                return false;
            if (precedenciaA <= precedenciaB)
                return true;
            return false;
        }

        public int checkPrecedencia(String caracter)
        {
            if (caracter == null) return -1;
            switch (caracter)
            {
                case "-u":
                case "+u":
                case "nao": return 7;

                case "*":
                case "div": return 6;

                case "-":
                case "+": return 5;

                case "<":
                case "<=":
                case ">=":
                case ">": return 4;

                case "=":
                case "!=": return 3;

                case "e": return 2;

                case "ou": return 1;
            }

            return -1;
        }

        public void desempilhaSimbolo(String caracter)
        {
            if(stackSymbols.Count != 0)
            {
                convertedExpression.Add(stackSymbols.Pop());
            }

            stackSymbols.Push(caracter);
        }

        public void empilhaSimbolo(String caracter)
        {
            stackSymbols.Push(caracter);
        }

        public void desempilhaTudo()
        {
            while(stackSymbols.Count != 0)
            {
                convertedExpression.Add(stackSymbols.Pop());
            }
        }

        public bool checkUnary(String caracter)
        {
            switch (caracter)
            {
                case "+":
                case "-":
                case "nao":
                case "*":
                case "div":
                case "<":
                case "<=":
                case ">":
                case ">=":
                case "=":
                case "!=":
                case "e":
                case "(":
                case ")":
                case "ou": return true;

                default: return false;
            }
        }

    }
}
