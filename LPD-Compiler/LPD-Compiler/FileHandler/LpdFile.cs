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

    public List<string> readFile(string fname)
    {
        List<string> lines = File.ReadAllLines(fname).ToList();
        return lines;
    }
}
}
