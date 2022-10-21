using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public static class CultureLanguage
    {

        public static string languageProd = "eng";

        public static void ChangeLanguageControlCollection<T>(Control.ControlCollection controlCollection)
        {
            try
            {
                foreach (Control c in controlCollection)
                {
                    foreach (Control d in c.Controls)
                    {
                        ComponentResourceManager resources2 = new ComponentResourceManager(typeof(T));
                        resources2.ApplyResources(d, d.Name, new CultureInfo(languageProd));
                    }
                    ComponentResourceManager resources = new ComponentResourceManager(typeof(T));
                    resources.ApplyResources(c, c.Name, new CultureInfo(languageProd));
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        public static void ChangeLanguageForm<T>(Form form)
        {
            try
            {
                languageProd = Properties.Settings.Default.LanguageProp;
                ChangeLanguageControlCollection<T>(form.Controls);
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        public static string ChangeLanguageName(string name)
        {
            try
            {
                return new ResourceManager("TibiaPixelBot.ResourceLanguage", typeof(Program).Assembly).GetString(name, CultureInfo.CreateSpecificCulture(languageProd));
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return string.Empty;
            }
            //CultureInfo newCulture = new CultureInfo(languageProd);
            //Thread.CurrentThread.CurrentCulture = newCulture;
            //Thread.CurrentThread.CurrentUICulture = newCulture;
            //ResourceManager rm = new ResourceManager("ResourceLanguage", typeof(Program).Assembly);
            //string greeting = String.Format("The current culture is {0}.\n{1}",
            //                                Thread.CurrentThread.CurrentUICulture.Name,
            //                                rm.GetString("HelloString"));
        }

    }
}
