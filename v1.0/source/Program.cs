using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prototype_Token_Interface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //try
            //{
                Application.Run(new Token_Toolbar());
            //}
            //catch
            //{
            //    return;
            //}
        }
    }
}
