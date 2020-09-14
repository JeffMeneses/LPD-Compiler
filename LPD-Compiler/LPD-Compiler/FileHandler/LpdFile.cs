using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPD_Compiler.FileHandler
{
    public class LpdFile
    {
        public string name;
        public string[] content;
        public int i = 0;
        public int currentLine = 0;
        public int countLines = 0;

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
            content = File.ReadAllLines(fname);
            countLines = File.ReadAllLines(fname).Count();
        }

        public char getCharacter()
        {
            char character = ' ';
            if (isIndexOutofBounds() == true)
            {
                i = 0;
                currentLine++;
            }
            if (!isEndOfFile()) character = content[currentLine][i];

            i++;
            return character;
        }

        public bool isIndexOutofBounds()
        {
            if (i >= content[currentLine].Length) return true;
            return false;
        }

        public bool isEndOfFile()
        {
            if (currentLine >= countLines) return true;
            return false;
        }
    }
}
