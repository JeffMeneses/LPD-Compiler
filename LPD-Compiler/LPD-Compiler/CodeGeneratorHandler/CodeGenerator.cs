using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPD_Compiler.SemanticHandler;

namespace LPD_Compiler.CodeGeneratorHandler
{

    public class CodeGenerator
    {
        public List<string> outputCode = new List<string>();

        public void generate(string rotulo, string comando, string param1, string param2)
        {
            switch(comando)
            {
                case "ALLOC":
                case "DALLOC": generateTwoParamCommand(comando, param1, param2);
                    break;

                case "LDC":
                case "LDV":
                case "STR": generateOneParamCommand(comando, param1);
                    break;

                case "JMP":
                case "JMPF":
                case "CALL": generateJumpCallCommand(comando, param1);
                    break;

                case "ADD":
                case "SUB":
                case "MULT":
                case "DIVI":
                case "INV":
                case "AND":
                case "OR":
                case "NEG":
                case "CME":
                case "CMA":
                case "CEQ":
                case "CDIF":
                case "CMEQ":
                case "CMAQ":
                case "START":
                case "HLT":
                case "RD":
                case "PRN":
                case "RETURN": generateNoParamCommand(comando);
                    break;

                case "NULL": generateNullCommand(comando, rotulo);
                    break;
                default:
                    break;
            }
        }

        public void generateNoParamCommand(string command)
        {
            outputCode.Add(command);
        }

        public void generateOneParamCommand(string command, string param1)
        {
            outputCode.Add(command + " " + param1);
        }

        public void generateTwoParamCommand(string command, string param1, string param2)
        {
            outputCode.Add(command + " " + param1 + "," + param2);
        }

        public void generateNullCommand(string command, string rotulo)
        {
            outputCode.Add("L" + rotulo + " " + command);
        }

        public void generateJumpCallCommand(string command, string param1)
        {
            outputCode.Add(command + " L" + param1);
        }

        public void test()
        {
            foreach (var item in outputCode)
            {
                Console.WriteLine(item);
            }
        }

        public void generateExpression(List<string> postFixExpression, Semantic semantic)
        {
            foreach(var item in postFixExpression)
            {
                switch(item)
                {
                    case "nao": generate("", "NEG", "", "");
                        break;
                    case "*":
                        generate("", "MULT", "", "");
                        break;
                    case "div":
                        generate("", "DIVI", "", "");
                        break;
                    case "-":
                        generate("", "SUB", "", "");
                        break;
                    case "+":
                        generate("", "ADD", "", "");
                        break;
                    case "<":
                        generate("", "CME", "", "");
                        break;
                    case "<=":
                        generate("", "CMEQ", "", "");
                        break;
                    case ">=":
                        generate("", "CMAQ", "", "");
                        break;
                    case ">":
                        generate("", "CMA", "", "");
                        break;
                    case "=":
                        generate("", "CEQ", "", "");
                        break;
                    case "!=":
                        generate("", "CDIF", "", "");
                        break;
                    case "e":
                        generate("", "AND", "", "");
                        break;
                    case "ou":
                        generate("", "OR", "", "");
                        break;
                    case "+u":
                        break;
                    case "-u": generate("", "INV", "", "");
                        break;
                    case "verdadeiro":
                        generate("", "LDC", "1", "");
                        break;
                    case "falso":
                        generate("", "LDC", "0", "");
                        break;
                    default:
                        if(int.TryParse(item, out _))
                        {
                            generate("", "LDC", item, "");
                        }
                        else
                        {
                            Item itemTable;
                            itemTable = semantic.getPesquisaTabela(item);

                            if(itemTable.tipo == "funcInteiro" || itemTable.tipo == "funcBooleano")
                            {
                                generate("", "CALL", itemTable.simbolo, "");
                                generate("", "LDV", "0", ""); // retorno da func DEVE estar na posicao 0
                            }
                            else
                            {
                                generate("", "LDV", itemTable.rotulo.ToString(), "");
                            }
                        }
                        break;
                }
            }
        }
    }
}
