using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public class HandlerFunction
    {

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);


        public static List<FunctionRulePoint> RulesPoint = new List<FunctionRulePoint>();
        public static List<Thread> threadListPoint = new List<Thread>();
        public static List<ControlePrioridade> ControlePrioridadeListPoint = new List<ControlePrioridade>();

        public static List<FunctionRulePorcent> RulesPorcent = new List<FunctionRulePorcent>();
        public static List<Thread> threadListPorcent = new List<Thread>();
        public static List<ControlePrioridade> ControlePrioridadeListPorcent = new List<ControlePrioridade>();

        public static void Start()
        {
            try
            {
                new Thread(new ThreadStart(HandlerFunction.run))
                {
                    IsBackground = true,
                    Name = "IDHAN"
                }.Start();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDHAN");
            }
        }

        private static void run()
        {
            BotLogs.add(CultureLanguage.ChangeLanguageName("StartingAutoAction"), null);

            while (true)
            {
                HealhManaRulesPoint();

                HealhManaRulesPorcent();

                Thread.Sleep(1000);
            }
        }

        private static void HealhManaRulesPoint()
        {
            try
            {
                foreach (var loop in RulesPoint)
                {
                    if (loop.Active)
                    {
                        if (ControlePrioridadeListPoint.Where(x => x.Id == loop.Id).ToList().Count == 0)
                        {
                            CreateThreadPoint(loop);
                            BotLogs.add(null, new Exception("ADD THREAD: IDPOI" + loop.Id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private static void HealhManaRulesPorcent()
        {
            try
            {
                foreach (var loop in HandlerFunction.RulesPorcent)
                {
                    if (loop.Active)
                    {
                        if (ControlePrioridadeListPorcent.Where(x => x.Id == loop.Id).ToList().Count == 0)
                        {
                            CreateThreadPorcent(loop);

                            BotLogs.add(null, new Exception("ADD THREAD: IDPOR" + loop.Id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        public static void CreateThreadPoint(FunctionRulePoint loop)
        {
            var newThread = new System.Threading.Thread(() => { LoopGenericoPoint(loop); });
            newThread.IsBackground = true;
            newThread.Name = "IDPOI" + loop.Id;
            newThread.Start();
            threadListPoint.Add(newThread);

            ControlePrioridadeListPoint.Add(new ControlePrioridade()
            {
                Id = loop.Id,
                IsExecutado = false,
                Prioridade = loop.Prioridade,
                IsLife = loop.Health
            });
        }

        public static void CreateThreadPorcent(FunctionRulePorcent loop)
        {
            var newThread = new System.Threading.Thread(() => { LoopGenericoPorcent(loop); });
            newThread.IsBackground = true;
            newThread.Name = "IDPOR" + loop.Id;
            newThread.Start();
            threadListPorcent.Add(newThread);

            ControlePrioridadeListPorcent.Add(new ControlePrioridade()
            {
                Id = loop.Id,
                IsExecutado = false,
                Prioridade = loop.Prioridade,
                IsLife = false
            });
        }

        private static void LoopGenericoPoint(FunctionRulePoint functionRule)
        {
            var pararLoop = 0;

            while (true)
            {
                try
                {
                    ControlePrioridadeListPoint.Where(w => w.Id == functionRule.Id).FirstOrDefault((f => f.IsExecutado = false));

                    Thread.Sleep(functionRule.TimerInterval / 2);

                    do
                    {
                        //if (functionRule.Prioridade == 1)
                        //{
                        //    var aaa = ControlePrioridadeListPoint.Where(x => x.Prioridade < functionRule.Prioridade && !x.IsExecutado && x.IsLife == functionRule.Life && x.Prioridade != 0).ToList();
                        //}
                        //Esperar o Proximo com mias prioridade
                    } while (ControlePrioridadeListPoint.Where(x => x.Prioridade < functionRule.Prioridade && !x.IsExecutado && x.IsLife == functionRule.Health && x.Prioridade != 0).Count() > 0);

                    if (FormPrincipal.TibiaPrimeiraJanela)
                    {
                        if (functionRule.Active)
                        {
                            if (functionRule.PosX > 0 && functionRule.PoxY > 0)
                            {
                                bool executeBool = false;

                                if (functionRule.Health)
                                {
                                    executeBool = ComparaVida(GetPixelColor(functionRule.PosX, functionRule.PoxY));
                                }
                                else
                                {
                                    executeBool = ComparaMana(GetPixelColor(functionRule.PosX, functionRule.PoxY));
                                }

                                executeBool = functionRule.Ausent ? !executeBool : executeBool;

                                if (executeBool)
                                {
                                    if (functionRule.Hotkey)
                                    {
                                        if (functionRule.Hotkey01.Trim().Equals("Shift +"))
                                        {
                                            SendKeys.SendWait("+" + "{" + functionRule.Hotkey02.Trim() + "}");
                                        }
                                        else if (functionRule.Hotkey01.Trim().Equals("Ctrl +"))
                                        {
                                            SendKeys.SendWait("^" + "{" + functionRule.Hotkey02.Trim() + "}");
                                        }
                                        else if (functionRule.Hotkey01.Trim().Equals("Empty") || functionRule.Hotkey01.Trim().Equals(""))
                                        {
                                            SendKeys.SendWait("{" + functionRule.Hotkey02.Trim() + "}");
                                        }
                                    }
                                    else
                                    {
                                        SendKeys.SendWait(functionRule.Spell.Trim());
                                        Thread.Sleep(20);
                                        SendKeys.SendWait("{ENTER}");
                                    }
                                    pararLoop++;
                                }
                                else
                                {
                                    pararLoop = 0;
                                }
                            }
                        }
                    }

                    ControlePrioridadeListPoint.Where(w => w.Id == functionRule.Id).ToList().ForEach(f => f.IsExecutado = true);

                    Thread.Sleep(functionRule.TimerInterval / 2);

                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDPOI" + functionRule.Id.ToString());
                }

                if (pararLoop >= Properties.Settings.Default.RulesAttempts)
                {
                    break;
                }
            }
            if (pararLoop >= Properties.Settings.Default.RulesAttempts)
            {
                FormPrincipal.idThreadsStopPoint.Add(functionRule.Id);
                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("MotivoSeguranca"));
            }
        }

        private static void LoopGenericoPorcent(FunctionRulePorcent functionRule)
        {
            var pararLoop = 0;

            while (true)
            {
                try
                {
                    ControlePrioridadeListPorcent.Where(w => w.Id == functionRule.Id).FirstOrDefault((f => f.IsExecutado = false));

                    Thread.Sleep(functionRule.TimerInterval / 2);

                    do
                    {
                        //Esperar o Proximo com mias prioridade
                    } while (ControlePrioridadeListPorcent.Where(x => x.Prioridade < functionRule.Prioridade && !x.IsExecutado && x.Prioridade != 0).Count() > 0);

                    if (FormPrincipal.TibiaPrimeiraJanela)
                    {
                        if (functionRule.Active)
                        {
                            if (FormPrincipal.PosMinHealth != new Point() && FormPrincipal.PosMaxHealth != new Point() &&
                                FormPrincipal.PosMinMana != new Point() && FormPrincipal.PosMaxMana != new Point())
                            {

                                int distancePixelsHealth = FormPrincipal.PosMaxHealth.X - FormPrincipal.PosMinHealth.X;
                                int distancePixelsMana = FormPrincipal.PosMaxMana.X - FormPrincipal.PosMinMana.X;

                                bool executeMinHealth = true;
                                bool executeMaxHealth = false;

                                bool executeMinMana = true;
                                bool executeMaxMana = false;

                                if (functionRule.EnabledHealth)
                                {
                                    if (functionRule.PorcentMinHealth >= 0 && functionRule.PorcentMaxHealth != 0 && functionRule.PorcentMinHealth < functionRule.PorcentMaxHealth)
                                    {
                                        int posMinHealth = FormPrincipal.PosMinHealth.X + (int)Math.Round(((decimal)functionRule.PorcentMinHealth / 100) * distancePixelsHealth);
                                        int posMaxHealth = FormPrincipal.PosMinHealth.X + (int)Math.Round(((decimal)functionRule.PorcentMaxHealth / 100) * distancePixelsHealth);

                                        if (functionRule.PorcentMinHealth > 0)
                                        {
                                            executeMinHealth = functionRule.AusentHealth ? !ComparaVida(GetPixelColor(posMinHealth, FormPrincipal.PosMinHealth.Y)) : ComparaVida(GetPixelColor(posMinHealth, FormPrincipal.PosMinHealth.Y));
                                        }
                                        executeMaxHealth = functionRule.AusentHealth ? !ComparaVida(GetPixelColor(posMaxHealth, FormPrincipal.PosMaxHealth.Y)) : ComparaVida(GetPixelColor(posMaxHealth, FormPrincipal.PosMaxHealth.Y));

                                        if (functionRule.PorcentMinHealth > 0)
                                        {
                                            if (functionRule.AusentHealth)
                                            {
                                                if (!executeMinHealth && executeMaxHealth)
                                                {
                                                    executeMinHealth = true;
                                                }
                                                else if (executeMinHealth && executeMaxHealth)
                                                {
                                                    executeMinHealth = false;
                                                }
                                            }
                                            else
                                            {
                                                if (executeMinHealth && !executeMaxHealth)
                                                {
                                                    executeMaxHealth = true;
                                                }
                                            }
                                        }
                                    }
                                    else if (functionRule.PorcentMinHealth == 0 && functionRule.PorcentMaxHealth == 0)
                                    {
                                        executeMaxHealth = true;
                                    }
                                }
                                else
                                {
                                    executeMaxHealth = true;
                                }

                                if (functionRule.EnabledMana)
                                {
                                    if (functionRule.PorcentMinMana >= 0 && functionRule.PorcentMaxMana != 0 && functionRule.PorcentMinMana < functionRule.PorcentMaxMana && executeMinHealth && executeMaxHealth)
                                    {
                                        int posMinMana = FormPrincipal.PosMinMana.X + (int)Math.Round(((decimal)functionRule.PorcentMinMana / 100) * distancePixelsMana);
                                        int posMaxMana = FormPrincipal.PosMinMana.X + (int)Math.Round(((decimal)functionRule.PorcentMaxMana / 100) * distancePixelsMana);

                                        if (functionRule.PorcentMinMana > 0)
                                        {
                                            executeMinMana = functionRule.AusentMana ? !ComparaMana(GetPixelColor(posMinMana, FormPrincipal.PosMinMana.Y)) : ComparaMana(GetPixelColor(posMinMana, FormPrincipal.PosMinMana.Y));
                                        }
                                        executeMaxMana = functionRule.AusentMana ? !ComparaMana(GetPixelColor(posMaxMana, FormPrincipal.PosMaxMana.Y)) : ComparaMana(GetPixelColor(posMaxMana, FormPrincipal.PosMaxMana.Y));

                                        if (functionRule.PorcentMinMana > 0)
                                        {
                                            if (functionRule.AusentMana)
                                            {
                                                if (!executeMinMana && executeMaxMana)
                                                {
                                                    executeMinMana = true;
                                                }
                                                else if (executeMinMana && executeMaxMana)
                                                {
                                                    executeMinMana = false;
                                                }
                                            }
                                            else
                                            {
                                                if (executeMinMana && !executeMaxMana)
                                                {
                                                    executeMaxMana = true;
                                                }
                                            }
                                        }
                                    }
                                    else if (functionRule.PorcentMinMana == 0 && functionRule.PorcentMaxMana == 0)
                                    {
                                        executeMaxMana = true;
                                    }
                                }
                                else
                                {
                                    executeMaxMana = true;
                                }

                                if (executeMinHealth && executeMaxHealth && executeMinMana && executeMaxMana)
                                {
                                    if (functionRule.Hotkey)
                                    {
                                        if (functionRule.Hotkey01.Trim().Equals("Shift +"))
                                        {
                                            SendKeys.SendWait("+" + "{" + functionRule.Hotkey02.Trim() + "}");
                                        }
                                        else if (functionRule.Hotkey01.Trim().Equals("Ctrl +"))
                                        {
                                            SendKeys.SendWait("^" + "{" + functionRule.Hotkey02.Trim() + "}");
                                        }
                                        else if (functionRule.Hotkey01.Trim().Equals("Empty") || functionRule.Hotkey01.Trim().Equals(""))
                                        {
                                            SendKeys.SendWait("{" + functionRule.Hotkey02.Trim() + "}");
                                        }
                                    }
                                    else
                                    {
                                        SendKeys.SendWait(functionRule.Spell.Trim());
                                        Thread.Sleep(20);
                                        SendKeys.SendWait("{ENTER}");
                                    }
                                    pararLoop++;
                                }
                                else
                                {
                                    pararLoop = 0;
                                }
                            }
                        }
                    }

                    ControlePrioridadeListPorcent.Where(w => w.Id == functionRule.Id).ToList().ForEach(f => f.IsExecutado = true);

                    Thread.Sleep(functionRule.TimerInterval / 2);

                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDPOR" + functionRule.Id.ToString());
                }

                if (pararLoop >= Properties.Settings.Default.RulesAttempts)
                {
                    break;
                }
            }
            if (pararLoop >= Properties.Settings.Default.RulesAttempts)
            {
                FormPrincipal.idThreadsStopPoint.Add(functionRule.Id);
                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("MotivoSeguranca"));
            }
        }

        private static Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        private static bool ComparaVida(Color cor)
        {
            int red = cor.R;
            int green = cor.G;
            int blue = cor.B;
            if ((red == 131 && green == 65 && blue == 65) ||
                (red == 144 && green == 46 && blue == 46) ||
                (red == 167 && green == 51 && blue == 51) ||
                (red == 192 && green == 64 && blue == 64) ||
                (red == 219 && green == 79 && blue == 79) ||
                (red == 211 && green == 79 && blue == 79) ||
                (red == 241 && green == 97 && blue == 97) ||
                (red == 192 && green == 96 && blue == 96) ||
                (red == 156 && green == 54 && blue == 54) ||
                (red == 255 && green == 113 && blue == 113) ||
                (red == 255 && green == 125 && blue == 125) ||
                (red == 219 && green == 91 && blue == 91) ||
                (red == 192 && green == 80 && blue == 80) ||
                (red == 220 && green == 113 && blue == 113))
            {
                return true;
            }
            return false;
        }

        private static bool ComparaMana(Color cor)
        {
            int red = cor.R;
            int green = cor.G;
            int blue = cor.B;
            if ((red == 67 && green == 65 && blue == 131) ||
                (red == 47 && green == 45 && blue == 145) ||
                (red == 54 && green == 51 && blue == 167) ||
                (red == 67 && green == 64 && blue == 192) ||
                (red == 83 && green == 80 && blue == 218) ||
                (red == 101 && green == 98 && blue == 240) ||
                (red == 116 && green == 113 && blue == 255) ||
                (red == 128 && green == 125 && blue == 255) ||
                (red == 82 && green == 79 && blue == 211) ||
                (red == 95 && green == 92 && blue == 218) ||
                (red == 91 && green == 88 && blue == 193) ||
                (red == 45 && green == 45 && blue == 105) ||
                (red == 38 && green == 39 && blue == 84) ||
                (red == 83 && green == 80 && blue == 192) ||
                (red == 116 && green == 113 && blue == 220))
            {
                return true;
            }
            return false;
        }

    }
}

