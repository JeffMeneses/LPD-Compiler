using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.FileHandler;

namespace LPD_Compiler.CompilerHandler
{
    public class Compiler
    {
        Lexicon lexicon = new Lexicon();
        LpdFile lpdFile = new LpdFile();

        public void runCompiler()
        {
            lpdFile.openLpdFile();
            lpdFile.readFile(lpdFile.name);

            lexicon.lexicalAnalyser(lpdFile);
        }
    }
}
