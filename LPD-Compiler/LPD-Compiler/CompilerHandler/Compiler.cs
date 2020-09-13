using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.FileHandler;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LPD_Compiler.CompilerHandler
{
    public class Compiler
    {
        public Lexicon lexicon = new Lexicon();
        public LpdFile lpdFile = new LpdFile();

        public void runCompiler()
        {
            lpdFile.openLpdFile();
            lpdFile.readFile(lpdFile.name);

            try
            {
                lexicon.lexicalAnalyser(lpdFile);
            }
            catch (LexiconException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
