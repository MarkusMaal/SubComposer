using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SubComposer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Form1 f1;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new Form1();
            if (args.Length > 0)
            {
                foreach (string element in args)
                {
                    f1.filename += element + " ";
                }
            }
            Application.Run(new Form1());
        }
    }
}
