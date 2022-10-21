using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace TibiaPixelBot
{
    internal class BotLogs
    {
        public static List<string> ListMsgs = new List<string>();

        public static void add(string msg, Exception ex)
        {
            try
            {
                string item = "";

                if (FormPrincipal.IsDebug && ex != null)
                    item = DateTime.Now + ": " + ex.Message;

                if (!string.IsNullOrEmpty(msg))
                    item = DateTime.Now + ": " + msg;

                if (!string.IsNullOrEmpty(item))
                    BotLogs.ListMsgs.Add(item);
            }
            catch (Exception) { }
        }

        public static void SaveLog(Exception ex, string metodo, string theadId)
        {
            add(null, ex);

            try
            {
                var m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\logs";

                if (!System.IO.Directory.Exists(m_exePath))
                    System.IO.Directory.CreateDirectory(m_exePath);

                m_exePath += "\\logFile (" + DateTime.Now.ToShortDateString().Replace("/", "-") + ").txt";

                StringBuilder messege = new StringBuilder();
                messege.Append(DateTime.Now.ToString());
                messege.Append("  @@@  Método: " + metodo);
                messege.Append("  @@@  Thread: " + (!string.IsNullOrEmpty(theadId) ? theadId : string.Empty));
                messege.Append("  @@@  Exception: " + (!string.IsNullOrEmpty(ex.Message) ? ex.Message : string.Empty));
                messege.Append("  @@@  Inner: " + (ex.InnerException != null ? (!string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : string.Empty) : string.Empty));

                using (StreamWriter txtWriter = File.AppendText(m_exePath))
                {
                    txtWriter.WriteLine(messege);
                }
            }
            catch (Exception e)
            {
                add(null, e);
            }
        }

        public static bool DebugMode()
        {
            try
            {
                object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(DebuggableAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length == 1))
                {
                    DebuggableAttribute attribute = customAttributes[0] as DebuggableAttribute;
                    return (attribute.IsJITOptimizerDisabled && attribute.IsJITTrackingEnabled);
                }
                return false;
            }
            catch (Exception ex)
            {
                add(null, ex);
                return false;
            }
        }
    }
}
