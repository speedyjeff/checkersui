using System;

namespace CheckersUi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetCallBack(new MyCheckersCallBack());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CheckersUiWf.CheckersFrm());
        }
    }
}
