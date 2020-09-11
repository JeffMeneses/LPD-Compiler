using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPD_Compiler.FileHandler
{
    class LpdFile
    {
        public string name;
        public string content;
        public int i = 0;

        public void openLpdFile()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text File Only|*.txt";
            string fname = "";

            if (fd.ShowDialog().Equals(DialogResult.OK))
            {
                fname = fd.FileName;
                name = fname;
            }
        }

        public void readFile(string fname)
        {
            content = File.ReadAllText(fname);
        }

        public char getCharacter()
        {
            char character = content[i];
            i++;
            return character;
        }
    }
}
