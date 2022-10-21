using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace TibiaPixelBot
{
    public static class TibiaFunction
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static string Version = "1.0.0";

        public static string Email { get; internal set; }

        public static string Password { get; internal set; }

        public static bool ProcurarTibiaAberto()
        {
            try
            {
                if (Process.GetProcessesByName("client").Length > 0 || Process.GetProcessesByName("Tibia").Length > 0)
                {
                    return true;
                }
                else if (FormPrincipal.IsDebug)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                if (FormPrincipal.IsDebug)
                {
                    return true;
                }
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return false;
            }
        }

        public static bool ProcurarCharOnline(string nomeChar)
        {
            try
            {
                return FindWindow(null, "Tibia - " + nomeChar) != IntPtr.Zero && ProcurarTibiaAberto();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return false;
            }
        }

        public static bool TibiaPrimeiraJanela()
        {
            try
            {
                var processoClient = Process.GetProcessesByName("client");

                if (processoClient.Length > 0)
                {
                    if (processoClient[0].MainWindowHandle == GetForegroundWindow())
                    {
                        FormPrincipal.VersaoAtualTibia = true;
                        return true;
                    }
                }
                else
                {
                    var processoTibia = Process.GetProcessesByName("Tibia");

                    if (processoTibia.Length > 0)
                    {
                        if (processoTibia[0].MainWindowHandle == GetForegroundWindow())
                        {
                            FormPrincipal.VersaoAtualTibia = false;
                            return true;
                        }
                    }
                }

                if (FormPrincipal.IsDebug)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                if (FormPrincipal.IsDebug)
                {
                    return true;
                }
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return false;
            }
        }

        public static string GetHDDSerialNo()
        {
            try
            {
                ManagementObjectCollection management = new ManagementClass("Win32_DiskDrive").GetInstances();
                string text = "";
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = management.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        if (Convert.ToString(managementObject["MediaType"]).ToUpper().Contains("FIXED"))
                        {
                            text += Convert.ToString(managementObject["SerialNumber"]);
                        }
                    }
                }
                return text.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
