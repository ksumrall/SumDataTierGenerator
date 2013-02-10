using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SumDataTierGenerator
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(new MainForm());
        }

        #region exceptions
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if ((e.ExceptionObject is Exception))
            {
                MessageBox.Show(((Exception)e.ExceptionObject).Message);
            }
        }

        static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
        #endregion

    }
}
