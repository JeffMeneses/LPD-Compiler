using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
