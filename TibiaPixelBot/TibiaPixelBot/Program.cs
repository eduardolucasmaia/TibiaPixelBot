using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (args != null && args.Length > 0)
                {
                    string fileName = args[0];

                    if (File.Exists(fileName))
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FormLogin(fileName));
                    }
                    else
                    {
                        FormMessageBox.Show("O arquivo não existe.", "Error!");
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FormLogin());
                    }
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormLogin());
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }
    }
}
