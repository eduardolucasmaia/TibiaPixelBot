using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public partial class FormPrincipal : MaterialForm
    {
        #region DLLs
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);
        #endregion

        #region SISTEM

        #region VARIAVEIS

        private Form formLogin = null;

        public static bool AplicandoLinguagem = false;

        private Timer TimerVerificarTibia = new Timer();

        private Timer TimerKeepAlive = new Timer();

        public static bool IsDebug = false;

        private bool forceExit = false;

        private string FileName = string.Empty;

        public static bool TibiaPrimeiraJanela = false;

        public static bool VersaoAtualTibia = false;

        KeyboardHook keyboardHook = new KeyboardHook();

        #endregion

        public FormPrincipal(Form login)
        {
            InicializarComponentes(login);
        }

        public FormPrincipal(Form login, string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                this.FileName = file;
            }
            InicializarComponentes(login);
        }

        private void InicializarComponentes(Form login)
        {
            InitializeComponent();

            try
            {
                this.formLogin = login;

                var skinManager = MaterialSkinManager.Instance;
                skinManager.AddFormToManage(this);
                skinManager.Theme = MaterialSkinManager.Themes.DARK;
                skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            InicializarDataGridViewPoint();
            InicializarDataGridViewPorcent();
            HandlerFunction.Start();

            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(CapturarKeyDown);
            keyboardHook.KeyUp += new KeyboardHook.KeyboardHookCallback(CapturarKeyUp);

        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            try
            {
                CultureLanguage.ChangeLanguageForm<FormPrincipal>(this);

                IsDebug = BotLogs.DebugMode();

                this.Icon = Properties.Resources.favicon;

                BitmapMagicShield = Properties.Resources.ms_icon;
                BitmapMagicShieldOld = Properties.Resources.ms_icon;
                BitmapHast = Properties.Resources.hast_icon;
                BitmapHastOld = Properties.Resources.hast_icon;
                BitmapParalize = Properties.Resources.paralize_icon;
                BitmapParalizeOld = Properties.Resources.paralize_icon;
                BitmapCurePoison = Properties.Resources.poison_icon;
                BitmapCurePoisonOld = Properties.Resources.poison_icon_old;
                BitmapCureFire = Properties.Resources.burn_icon;
                BitmapCureFireOld = Properties.Resources.burn_icon;
                BitmapHungry = Properties.Resources.hungry_icon;
                BitmapHungryOld = Properties.Resources.hungry_icon;
                BitmapHeart = Properties.Resources.heart_icon;
                BitmapHeartOld = Properties.Resources.heart_icon;
                BitmapBleeding = Properties.Resources.bleeding_icon;
                BitmapBleedingOld = Properties.Resources.bleeding_icon;
                BitmapEletric = Properties.Resources.electrification_icon;
                BitmapEletricOld = Properties.Resources.electrification_icon;
                BitmapCurse = Properties.Resources.curse_icon;
                BitmapCurseOld = Properties.Resources.curse_icon;


                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem1 = new MenuItem();
                MenuItem menuItem2 = new MenuItem();
                MenuItem menuItem3 = new MenuItem();
                MenuItem menuItem4 = new MenuItem();
                MenuItem menuItem5 = new MenuItem();
                MenuItem menuItem6 = new MenuItem();

                contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem1 });
                contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem2 });
                contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem3 });
                contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem4 });
                contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem5 });
                contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem6 });


                menuItem4.Index = 0;
                menuItem4.Text = "Import";
                menuItem4.Click += new System.EventHandler(this.btnImport_Click);

                menuItem5.Index = 1;
                menuItem5.Text = "Export";
                menuItem5.Click += new System.EventHandler(this.btnExport_Click);

                menuItem6.Index = 2;
                menuItem6.Text = "-";

                menuItem1.Index = 3;
                menuItem1.Text = "Settings";
                menuItem1.Click += new System.EventHandler(this.btnSettings_Click);

                menuItem2.Index = 4;
                menuItem2.Text = "-";

                menuItem3.Index = 5;
                menuItem3.Text = "Exit";
                menuItem3.Click += new System.EventHandler(this.btnExit_Click);

                this.myNotifyIcon.ContextMenu = contextMenu;

            }
            catch (Exception ex)
            {
                BotLogs.add(CultureLanguage.ChangeLanguageName("ErrorLoadingResources"), ex);
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            try
            {
                TimerVerificarTibia.Tick += new EventHandler(LoopParaVerificacoes);
                TimerVerificarTibia.Interval = 1000;
                TimerVerificarTibia.Enabled = true;
                TimerVerificarTibia.Start();
                RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'P');
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, string.Empty);
            }

            try
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(() => { LoopCapturaImagens(); }))
                {
                    IsBackground = true,
                    Name = "IDIMG"
                }.Start();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDIMG");
            }

            try
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(() => { LoopTibiaPrimeiraJanela(); }))
                {
                    IsBackground = true,
                    Name = "IDTIB"
                }.Start();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDTIB");
            }


            try
            {
                TimerKeepAlive.Tick += new EventHandler(KeepAlive);
                TimerKeepAlive.Interval = 5000;
                //TimerVerificarTibia.Interval = 150000;
                TimerKeepAlive.Enabled = true;
                TimerKeepAlive.Start();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, string.Empty);
            }

            try
            {
                if (!string.IsNullOrEmpty(this.FileName))
                {
                    ImportarPorNomeFile(this.FileName);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDIMG");
            }
        }

        private void KeepAlive(object sender, EventArgs e)
        {
            try
            {
                TimerKeepAlive.Stop();

                using (WebClient webClient = new WebClient())
                {
                    string result = webClient.DownloadString(string.Concat(new string[]
                    {
                                    "http://localhost:53099/api/Account/KeepAlive/",
                                    TibiaFunction.Email,
                                    "/",
                                    TibiaFunction.Password,
                                    "/",
                                    TibiaFunction.GetHDDSerialNo()
                    }));


                    result = result.Replace("\\", "");
                    result = result.Replace("\"{", "{");
                    result = result.Replace("}\"", "} ");

                    var objectSerialize = JsonConvert.DeserializeObject<ObjetoRetornoAplicativo>(result);

                    if (!objectSerialize.Valido)
                    {
                        if (!string.IsNullOrEmpty(objectSerialize.Mensagem))
                        {
                            using (var form = new FormMessageBox(objectSerialize.Mensagem))
                            {
                                var formResult = form.ShowDialog();
                                formLogin.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
            finally
            {
                TimerKeepAlive.Start();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            forceExit = true;
            this.Close();
            formLogin.Close();
        }

        private void LoopParaVerificacoes(object sender, EventArgs e)
        {
            if (idThreadsStopPoint.Count > 0)
            {
                try
                {
                    var tempRemoveidThreadsStopPoint = new List<int>();
                    foreach (int loop in idThreadsStopPoint)
                    {
                        StopFunctionPointById(loop);
                        tempRemoveidThreadsStopPoint.Add(loop);
                    }
                    foreach (int loop in tempRemoveidThreadsStopPoint)
                    {
                        idThreadsStopPoint.Remove(loop);
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
            }

            if (idThreadsStopPorcent.Count > 0)
            {
                try
                {
                    var tempRemoveidThreadsStopPorcent = new List<int>();
                    foreach (int loop in idThreadsStopPorcent)
                    {
                        StopFunctionPorcentById(loop);
                        tempRemoveidThreadsStopPorcent.Add(loop);
                    }
                    foreach (int loop in tempRemoveidThreadsStopPorcent)
                    {
                        idThreadsStopPorcent.Remove(loop);
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
            }

            if (AplicandoLinguagem)
            {
                AplicandoLinguagem = false;
            }

            if (!TibiaPrimeiraJanela)
            {
                this.Text = CultureLanguage.ChangeLanguageName("FormStop");
            }
            else
            {
                this.Text = CultureLanguage.ChangeLanguageName("FormWorking");
            }

            if (this.dgvFunctionPoint.RowCount > 0)
            {
                this.btnEditPoint.Enabled = true;
                this.btnDeletePoint.Enabled = true;
                this.btnStartPoint.Enabled = true;
                this.btnStopPoint.Enabled = true;
            }
            else
            {
                this.btnDeletePoint.Enabled = false;
                this.btnEditPoint.Enabled = false;
                this.btnStartPoint.Enabled = false;
                this.btnStopPoint.Enabled = false;
            }

            if (this.dgvFunctionPorcent.RowCount > 0)
            {
                this.btnEditPorcent.Enabled = true;
                this.btnDeletePorcent.Enabled = true;
                this.btnStartPorcent.Enabled = true;
                this.btnStopPorcent.Enabled = true;
            }
            else
            {
                this.btnDeletePorcent.Enabled = false;
                this.btnEditPorcent.Enabled = false;
                this.btnStartPorcent.Enabled = false;
                this.btnStopPorcent.Enabled = false;
            }

            try
            {
                if (this.cbRecLogs.Checked)
                {
                    BotLogs.ListMsgs.ForEach(delegate (string msg)
                    {
                        this.tbLog.AppendText(Environment.NewLine + msg);
                        this.tbLog.ScrollToCaret();
                    });
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
            finally
            {
                BotLogs.ListMsgs.Clear();
            }
        }

        private void LoopTibiaPrimeiraJanela()
        {
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(500);
                    TibiaPrimeiraJanela = TibiaFunction.TibiaPrimeiraJanela();
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }

            }
        }

        private void LoopCapturaImagens()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(500);

                if (TibiaPrimeiraJanela)
                {

                    try
                    {
                        BitmapExtensions.getToolAreaNew();
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }

                    try
                    {
                        if (PosAction.X != 0 && PosAction.Y != 0)
                        {
                            if (PosAction.X > 0 && PosAction.Y > 0)
                            {
                                if (VerificarAlgumAutoFunctionLigado())
                                {
                                    bitmapQuadranteCaputrado = BitmapExtensions.RetornaQuadranteImagem((PosAction.X - 20), (PosAction.Y - 20), 140, 40);
                                    this.pbCaptured.Image = bitmapQuadranteCaputrado;
                                }
                            }
                            else
                            {
                                BotLogs.add(CultureLanguage.ChangeLanguageName("CaptureImage"), null);
                                throw new Exception(CultureLanguage.ChangeLanguageName("LocationNegativeNum"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }

                    CapturarPosicaoVidaManaPorcentagem();
                }

                //if (TibiaFunction.ProcurarTibiaAberto())
                //{
                //    CapturarPosicaoVidaManaPorcentagem();
                //}
            }
        }

        private bool TratarCombox(string combo, List<string> list)
        {
            foreach (var loop in list)
            {
                if (combo.Equals(loop))
                {
                    return true;
                }
            }
            return false;
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.CloseToTray)
            {
                if (e.CloseReason == CloseReason.UserClosing && !forceExit)
                {
                    MinimizeToTray();
                    e.Cancel = true;
                }
            }
            else
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        string result = webClient.DownloadString(string.Concat(new string[]
                        {
                                    "http://localhost:53099/api/Account/LogOut/",
                                    TibiaFunction.Email,
                                    "/",
                                   TibiaFunction.Password
                        }));
                    }
                }
                catch (Exception) { }

                formLogin.Close();
            }
        }

        private void myNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            myNotifyIcon.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MinimizeToTray()
        {
            this.myNotifyIcon.Visible = true;
            this.Hide();
            this.myNotifyIcon.BalloonTipText = "Minimized";
            this.myNotifyIcon.BalloonTipTitle = "Pixel is running in background";
            this.myNotifyIcon.ShowBalloonTip(500);
        }

        private void FormPrincipal_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                if (Properties.Settings.Default.MinimizeToTray)
                {
                    MinimizeToTray();
                }
            }
        }

        private void FormPrincipal_Move(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                if (Properties.Settings.Default.MinimizeToTray)
                {
                    MinimizeToTray();
                }
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        protected override void WndProc(ref Message msg)
        {
            try
            {
                if (msg.Msg == 0x0312)
                {
                    if (TibiaFunction.ProcurarTibiaAberto())
                    {
                        if (this.cbCaptureMinHelPorc.Checked)
                        {
                            this.cbCaptureMinHelPorc.Checked = false;
                            this.tbPosXHelMinPorc.Text = Cursor.Position.X.ToString();
                            this.tbPosYHelMinPorc.Text = Cursor.Position.Y.ToString();
                            PosMinHealth = Cursor.Position;
                            AtivarBtnAddPorcent();
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCaptureMaxHelPorc.Checked)
                        {
                            this.cbCaptureMaxHelPorc.Checked = false;
                            this.tbPosXHelMaxPorc.Text = Cursor.Position.X.ToString();
                            this.tbPosYHelMaxPorc.Text = Cursor.Position.Y.ToString();
                            PosMaxHealth = Cursor.Position;
                            AtivarBtnAddPorcent();
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCaptureMinManPorc.Checked)
                        {
                            this.cbCaptureMinManPorc.Checked = false;
                            this.tbPosXManMinPorc.Text = Cursor.Position.X.ToString();
                            this.tbPosYManMinPorc.Text = Cursor.Position.Y.ToString();
                            PosMinMana = Cursor.Position;
                            AtivarBtnAddPorcent();
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCaptureMaxManPorc.Checked)
                        {
                            this.cbCaptureMaxManPorc.Checked = false;
                            this.tbPosXManMaxPorc.Text = Cursor.Position.X.ToString();
                            this.tbPosYManMaxPorc.Text = Cursor.Position.Y.ToString();
                            PosMaxMana = Cursor.Position;
                            AtivarBtnAddPorcent();
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCapturePoint.Checked)
                        {
                            this.cbCapturePoint.Checked = false;
                            this.tbPosXPoint.Text = Cursor.Position.X.ToString();
                            this.tbPosYPoint.Text = Cursor.Position.Y.ToString();
                            AtivarBtnAddPoint();
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCaptureAction.Checked)
                        {
                            this.cbCaptureAction.Checked = false;
                            this.tbPosActionX.Text = Cursor.Position.X.ToString();
                            this.tbPosActionY.Text = Cursor.Position.Y.ToString();
                            PosAction = Cursor.Position;
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCapturePlayer.Checked)
                        {
                            this.cbCapturePlayer.Checked = false;
                            this.tbPosPlayerX.Text = Cursor.Position.X.ToString();
                            this.tbPosPlayerY.Text = Cursor.Position.Y.ToString();
                            PosPlayer = Cursor.Position;
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                        else if (this.cbCaptureLoot.Checked)
                        {
                            this.cbCaptureLoot.Checked = false;
                            this.tbPosLootX.Text = Cursor.Position.X.ToString();
                            this.tbPosLootY.Text = Cursor.Position.Y.ToString();
                            PosLoot = Cursor.Position;
                            FormMessageBox.Show(CultureLanguage.ChangeLanguageName("PositionSaved"), "Ctrl + P");
                        }
                    }
                    else
                    {
                        FormMessageBox.Show(CultureLanguage.ChangeLanguageName("TibiaNotOpen"), CultureLanguage.ChangeLanguageName("Problems"));
                    }

                }
                base.WndProc(ref msg);
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        #region BUTTONS

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            this.cbAutoBleeding.Checked = false;
            this.cbAutoCureEletric.Checked = false;
            this.cbAutoCureFire.Checked = false;
            this.cbAutoCurePoison.Checked = false;
            this.cbAutoCurse.Checked = false;
            this.cbAutoFood.Checked = false;
            this.cbAutoHaste.Checked = false;
            this.cbAutoParalize.Checked = false;
            this.cbAutoManaShield.Checked = false;

            #region STOP Point
            try
            {
                foreach (var loop in HandlerFunction.RulesPoint)
                {
                    StopFunctionPointById(loop.Id);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            #endregion

            #region STOP Porcent
            try
            {
                foreach (var loop in HandlerFunction.RulesPorcent)
                {
                    StopFunctionPorcentById(loop.Id);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            #endregion

            BotLogs.add(CultureLanguage.ChangeLanguageName("StopAll"), null);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FormSettings settings = new FormSettings(this.Controls);
            settings.ShowDialog();
        }

        private void btnCleanLogs_Click(object sender, EventArgs e)
        {
            this.tbLog.Text = string.Empty;
            BotLogs.add(CultureLanguage.ChangeLanguageName("CleanedLogs"), null);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = CultureLanguage.ChangeLanguageName("TPBFiles") + " (*.tpb)|*.tpb|All files (*.*)|*.*";
                openFileDialog.Title = CultureLanguage.ChangeLanguageName("OpenTPB");
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if ((openFileDialog.OpenFile()) != null)
                    {
                        ImportarPorNomeFile(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void ImportarPorNomeFile(string file)
        {
            try
            {
                string result = string.Join("", File.ReadAllLines(file));

                var objectSerialize = XmlConvert.DeserializeObject<ObjectSerialize>(result);

                if (objectSerialize != null)
                {

                    #region POINT
                    DeleteAllFunctionPoint();

                    if (objectSerialize.HealthManaFunctionPointList.Count > 0)
                    {
                        foreach (var loop in objectSerialize.HealthManaFunctionPointList)
                        {
                            this.tbPosXPoint.Text = loop.PosX.ToString();
                            this.tbPosYPoint.Text = loop.PosY.ToString();

                            this.rbHotkeyPoint.Checked = loop.HotKeyOrSpell;
                            this.rbSpellPoint.Checked = !loop.HotKeyOrSpell;

                            this.cbHotkey01Point.Text = string.IsNullOrEmpty(loop.HotKey01) ? "Empty" : loop.HotKey01;
                            this.cbHotkey02Point.Text = string.IsNullOrEmpty(loop.HotKey02) ? "F1" : loop.HotKey02;

                            this.tbSpellPoint.Text = loop.Spell;

                            this.nudPrioridadePoint.Value = loop.Priority;
                            this.nudIntervaloPoint.Value = loop.Interval;

                            this.rbAusentePoint.Checked = loop.AbsentOrPresent;
                            this.rbPresentePoint.Checked = !loop.AbsentOrPresent;

                            this.rbLifePoint.Checked = loop.HealthOrMana;
                            this.rbManaPoint.Checked = !loop.HealthOrMana;

                            AddFunctionPoint(loop.Enabled);
                        }
                    }

                    #endregion

                    #region PORCENT

                    DeleteAllFunctionPorcent();

                    if (objectSerialize.HealthManaFunctionPorcentList.Count > 0)
                    {
                        foreach (var loop in objectSerialize.HealthManaFunctionPorcentList)
                        {
                            this.rbHotkeyPorcent.Checked = loop.HotKeyOrSpell;
                            this.rbSpellPorcent.Checked = !loop.HotKeyOrSpell;

                            this.cbHotkey01Porcent.Text = string.IsNullOrEmpty(loop.HotKey01) ? "Empty" : loop.HotKey01;
                            this.cbHotkey02Porcent.Text = string.IsNullOrEmpty(loop.HotKey02) ? "F1" : loop.HotKey02;

                            this.tbSpellPorcent.Text = loop.Spell;

                            this.nudPrioridadePorcent.Value = loop.Priority;
                            this.nudIntervaloPorcent.Value = loop.Interval;

                            this.rbAusentePorcentHealth.Checked = loop.AbsentOrPresentHealth;
                            this.rbPresentePorcentHealth.Checked = !loop.AbsentOrPresentHealth;

                            this.rbAusentePorcentMana.Checked = loop.AbsentOrPresentMana;
                            this.rbPresentePorcentMana.Checked = !loop.AbsentOrPresentMana;

                            this.nudPorcentMinHealth.Value = loop.MinHealth;
                            this.nudPorcentMaxHealth.Value = loop.MaxHealth;

                            this.nudPorcentMinMana.Value = loop.MinMana;
                            this.nudPorcentMaxMana.Value = loop.MaxMana;

                            this.cbEnableHealthPorcent.Checked = loop.EnabledHealth;
                            this.cbEnableManaPorcent.Checked = loop.EnabledMana;

                            AddFunctionPorcent(loop.Enabled);
                        }
                    }

                    PosMinHealth = new Point(objectSerialize.FunctionPorcentPosXMinHealth, objectSerialize.FunctionPorcentPosYMinHealth);
                    PosMaxHealth = new Point(objectSerialize.FunctionPorcentPosXMaxHealth, objectSerialize.FunctionPorcentPosYMaxHealth);
                    PosMinMana = new Point(objectSerialize.FunctionPorcentPosXMinMana, objectSerialize.FunctionPorcentPosYMinMana);
                    PosMaxMana = new Point(objectSerialize.FunctionPorcentPosXMaxMana, objectSerialize.FunctionPorcentPosYMaxMana);

                    this.tbPosXHelMinPorc.Text = PosMinHealth.X.ToString();
                    this.tbPosYHelMinPorc.Text = PosMinHealth.Y.ToString();

                    this.tbPosXHelMaxPorc.Text = PosMaxHealth.X.ToString();
                    this.tbPosYHelMaxPorc.Text = PosMaxHealth.Y.ToString();

                    this.tbPosXManMinPorc.Text = PosMinMana.X.ToString();
                    this.tbPosYManMinPorc.Text = PosMinMana.Y.ToString();

                    this.tbPosXManMaxPorc.Text = PosMaxMana.X.ToString();
                    this.tbPosYManMaxPorc.Text = PosMaxMana.Y.ToString();

                    this.cbAutoScanPorcent.Checked = objectSerialize.ActiveAutoScan;

                    #endregion

                    #region AUTO ACTION

                    this.tbPosActionX.Text = objectSerialize.AutoActionPosX.ToString();
                    this.tbPosActionY.Text = objectSerialize.AutoActionPosY.ToString();

                    PosAction = new Point(objectSerialize.AutoActionPosX, objectSerialize.AutoActionPosY);

                    if (objectSerialize.AutoActionList.Count > 0)
                    {
                        foreach (var loop in objectSerialize.AutoActionList)
                        {
                            if (loop.Name.Equals("Poison"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoCurePoison, this.cbHotkeyCurePoison01, this.cbHotkeyCurePoison02, this.nudIntervaloCurePoison, loop);
                            }
                            else if (loop.Name.Equals("Bleeding"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoBleeding, this.cbHotkeyBleeding01, this.cbHotkeyBleeding02, this.nudIntervaloBleeding, loop);
                            }
                            else if (loop.Name.Equals("Curse"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoCurse, this.cbHotkeyCurse01, this.cbHotkeyCurse02, this.nudIntervaloCurse, loop);
                            }
                            else if (loop.Name.Equals("Haste"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoHaste, this.cbHotkeyHaste01, this.cbHotkeyHaste02, this.nudIntervaloHaste, loop);
                            }
                            else if (loop.Name.Equals("Burning"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoCureFire, this.cbHotkeyCureFire01, this.cbHotkeyCureFire02, this.nudIntervaloCureFire, loop);
                            }
                            else if (loop.Name.Equals("Food"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoFood, this.cbHotkeyFood01, this.cbHotkeyFood02, this.nudIntervaloFood, loop);
                            }
                            else if (loop.Name.Equals("Shield"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoManaShield, this.cbHotkeyManaShield01, this.cbHotkeyManaShield02, this.nudIntervaloManaShield, loop);
                            }
                            else if (loop.Name.Equals("Paralize"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoParalize, this.cbHotkeyParalize01, this.cbHotkeyParalize02, this.nudIntervaloParalize, loop);
                            }
                            else if (loop.Name.Equals("Electrification"))
                            {
                                ActivateAutoFunctionGeneric(this.cbAutoCureEletric, this.cbHotkeyCureEletric01, this.cbHotkeyCureEletric02, this.nudIntervaloCureEletric, loop);
                            }
                        }
                    }
                    #endregion

                    #region AUTO LOOT

                    HotkeyLoot01 = (KeyboardHook.VKeys)objectSerialize.VKeysLoot01;
                    HotkeyLoot02 = (KeyboardHook.VKeys)objectSerialize.VKeysLoot02;

                    if (HotkeyLoot02 != new KeyboardHook.VKeys())
                    {
                        this.labelHotkeyLoot.Text = "HOTKEY: (";
                        if (HotkeyLoot01 != new KeyboardHook.VKeys())
                        {
                            this.labelHotkeyLoot.Text += HotkeyLoot01.ToString() + " + ";
                        }
                        this.labelHotkeyLoot.Text += HotkeyLoot02.ToString() + ")";
                    }

                    nudIntervaloAutoLoot.Value = objectSerialize.AutoLootIntervalo;

                    this.tbPosLootX.Text = objectSerialize.AutoClickLootPosX.ToString();
                    this.tbPosLootY.Text = objectSerialize.AutoClickLootPosY.ToString();

                    this.tbPosPlayerX.Text = objectSerialize.AutoClickPlayerPosX.ToString();
                    this.tbPosPlayerY.Text = objectSerialize.AutoClickPlayerPosY.ToString();

                    PosLoot = new Point(objectSerialize.AutoClickLootPosX, objectSerialize.AutoClickLootPosY);
                    PosPlayer = new Point(objectSerialize.AutoClickPlayerPosX, objectSerialize.AutoClickPlayerPosY);

                    this.cbEnabledAutoLoot.Checked = objectSerialize.ActiveAutoLoot;

                    #endregion

                }

                BotLogs.add(CultureLanguage.ChangeLanguageName("ImportSucces"), null);
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ErroReadFile"), CultureLanguage.ChangeLanguageName("Problems"));
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                #region ACTION
                var autoActionList = new List<AutoAction>();

                #region Bleeding
                autoActionList.Add(new AutoAction()
                {
                    Name = "Bleeding",
                    Enable = this.cbAutoBleeding.Checked,
                    HotKey01 = this.cbHotkeyBleeding01.Text,
                    HotKey02 = this.cbHotkeyBleeding02.Text,
                    Interval = this.nudIntervaloBleeding.Value
                });
                #endregion

                #region Curse
                autoActionList.Add(new AutoAction()
                {
                    Name = "Curse",
                    Enable = this.cbAutoCurse.Checked,
                    HotKey01 = this.cbHotkeyCurse01.Text,
                    HotKey02 = this.cbHotkeyCurse02.Text,
                    Interval = this.nudIntervaloCurse.Value
                });
                #endregion

                #region Haste
                autoActionList.Add(new AutoAction()
                {
                    Name = "Haste",
                    Enable = this.cbAutoHaste.Checked,
                    HotKey01 = this.cbHotkeyHaste01.Text,
                    HotKey02 = this.cbHotkeyHaste02.Text,
                    Interval = this.nudIntervaloHaste.Value
                });
                #endregion

                #region Poison
                autoActionList.Add(new AutoAction()
                {
                    Name = "Poison",
                    Enable = this.cbAutoCurePoison.Checked,
                    HotKey01 = this.cbHotkeyCurePoison01.Text,
                    HotKey02 = this.cbHotkeyCurePoison02.Text,
                    Interval = this.nudIntervaloCurePoison.Value
                });
                #endregion

                #region Fire
                autoActionList.Add(new AutoAction()
                {
                    Name = "Burning",
                    Enable = this.cbAutoCureFire.Checked,
                    HotKey01 = this.cbHotkeyCureFire01.Text,
                    HotKey02 = this.cbHotkeyCureFire02.Text,
                    Interval = this.nudIntervaloCureFire.Value
                });
                #endregion

                #region Food
                autoActionList.Add(new AutoAction()
                {
                    Name = "Food",
                    Enable = this.cbAutoFood.Checked,
                    HotKey01 = this.cbHotkeyFood01.Text,
                    HotKey02 = this.cbHotkeyFood02.Text,
                    Interval = this.nudIntervaloFood.Value
                });
                #endregion

                #region Mana Shield
                autoActionList.Add(new AutoAction()
                {
                    Name = "Shield",
                    Enable = this.cbAutoManaShield.Checked,
                    HotKey01 = this.cbHotkeyManaShield01.Text,
                    HotKey02 = this.cbHotkeyManaShield02.Text,
                    Interval = this.nudIntervaloManaShield.Value
                });
                #endregion

                #region Paralize
                autoActionList.Add(new AutoAction()
                {
                    Name = "Paralize",
                    Enable = this.cbAutoParalize.Checked,
                    HotKey01 = this.cbHotkeyParalize01.Text,
                    HotKey02 = this.cbHotkeyParalize02.Text,
                    Interval = this.nudIntervaloParalize.Value
                });
                #endregion

                #region Electrification
                autoActionList.Add(new AutoAction()
                {
                    Name = "Electrification",
                    Enable = this.cbAutoCureEletric.Checked,
                    HotKey01 = this.cbHotkeyCureEletric01.Text,
                    HotKey02 = this.cbHotkeyCureEletric02.Text,
                    Interval = this.nudIntervaloCureEletric.Value
                });
                #endregion

                #endregion

                #region  POINT

                var healthManaFunctionPoint = new List<HealthManaFunctionPoint>();

                foreach (var loop in HandlerFunction.RulesPoint)
                {
                    try
                    {
                        healthManaFunctionPoint.Add(new HealthManaFunctionPoint()
                        {
                            Enabled = loop.Active,
                            AbsentOrPresent = loop.Ausent,
                            HealthOrMana = loop.Health,
                            HotKey01 = loop.Hotkey01,
                            HotKey02 = loop.Hotkey02,
                            HotKeyOrSpell = loop.Hotkey,
                            Interval = loop.TimerInterval,
                            Spell = loop.Spell,
                            Priority = loop.Prioridade,
                            PosX = loop.PosX,
                            PosY = loop.PoxY
                        });
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }

                #endregion

                #region  Porcent

                var healthManaFunctionPorcent = new List<HealthManaFunctionPorcent>();

                foreach (var loop in HandlerFunction.RulesPorcent)
                {
                    try
                    {
                        healthManaFunctionPorcent.Add(new HealthManaFunctionPorcent()
                        {
                            Enabled = loop.Active,
                            HotKey01 = loop.Hotkey01,
                            HotKey02 = loop.Hotkey02,
                            HotKeyOrSpell = loop.Hotkey,
                            Interval = loop.TimerInterval,
                            Spell = loop.Spell,
                            Priority = loop.Prioridade,
                            AbsentOrPresentHealth = loop.AusentHealth,
                            AbsentOrPresentMana = loop.AusentMana,
                            MaxHealth = loop.PorcentMaxHealth,
                            MaxMana = loop.PorcentMaxMana,
                            MinHealth = loop.PorcentMinHealth,
                            MinMana = loop.PorcentMinMana,
                            EnabledHealth = loop.EnabledHealth,
                            EnabledMana = loop.EnabledMana
                        });
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }

                #endregion

                XmlConvert.SerializeObject<ObjectSerialize>(new ObjectSerialize()
                {
                    Email = Properties.Settings.Default.EmailUser,
                    Password = Properties.Settings.Default.PasswordUser,
                    AutoActionList = autoActionList,
                    HealthManaFunctionPointList = healthManaFunctionPoint,
                    HealthManaFunctionPorcentList = healthManaFunctionPorcent,
                    FunctionPorcentPosXMinHealth = PosMinHealth.X,
                    FunctionPorcentPosYMinHealth = PosMinHealth.Y,
                    FunctionPorcentPosXMaxHealth = PosMaxHealth.X,
                    FunctionPorcentPosYMaxHealth = PosMaxHealth.Y,
                    FunctionPorcentPosXMinMana = PosMinMana.X,
                    FunctionPorcentPosYMinMana = PosMinMana.Y,
                    FunctionPorcentPosXMaxMana = PosMaxMana.X,
                    FunctionPorcentPosYMaxMana = PosMaxMana.Y,
                    ActiveAutoScan = this.cbAutoScanPorcent.Checked,
                    AutoActionPosX = int.Parse(string.IsNullOrEmpty(this.tbPosActionX.Text) ? "0" : this.tbPosActionX.Text),
                    AutoActionPosY = int.Parse(string.IsNullOrEmpty(this.tbPosActionY.Text) ? "0" : this.tbPosActionY.Text),
                    ActiveAutoLoot = this.cbEnabledAutoLoot.Checked,
                    AutoClickLootPosX = PosLoot.X,
                    AutoClickLootPosY = PosLoot.Y,
                    AutoClickPlayerPosX = PosPlayer.X,
                    AutoClickPlayerPosY = PosPlayer.Y,
                    AutoLootIntervalo = nudIntervaloAutoLoot.Value,
                    VKeysLoot01 = (int)HotkeyLoot01,
                    VKeysLoot02 = (int)HotkeyLoot02
                });

                BotLogs.add(CultureLanguage.ChangeLanguageName("SavedSucces"), null);
            }
            catch (Exception ex)
            {
                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("CantExport"), CultureLanguage.ChangeLanguageName("Problems"));
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

        }

        #endregion

        #endregion

        #region AUTO ACTIONS

        #region VARIAVEIS

        #region IMAGES FOR FUNCTION

        public Bitmap bitmapQuadranteCaputrado = null;

        private Bitmap BitmapMagicShield = null;
        private Bitmap BitmapMagicShieldOld = null;

        private Bitmap BitmapCurePoison = null;
        private Bitmap BitmapCurePoisonOld = null;

        private Bitmap BitmapCureFire = null;
        private Bitmap BitmapCureFireOld = null;

        private Bitmap BitmapHast = null;
        private Bitmap BitmapHastOld = null;

        private Bitmap BitmapParalize = null;
        private Bitmap BitmapParalizeOld = null;

        private Bitmap BitmapBleeding = null;
        private Bitmap BitmapBleedingOld = null;

        private Bitmap BitmapEletric = null;
        private Bitmap BitmapEletricOld = null;

        private Bitmap BitmapCurse = null;
        private Bitmap BitmapCurseOld = null;

        private Bitmap BitmapHungry = null;
        private Bitmap BitmapHungryOld = null;

        private Bitmap BitmapHeart = null;
        private Bitmap BitmapHeartOld = null;

        #endregion

        #region THREAD

        private System.Threading.Thread AutoFoodThread = null;

        private System.Threading.Thread AutoManaShieldThread = null;

        private System.Threading.Thread AutoCurePoisonThread = null;

        private System.Threading.Thread AutoCureFireThread = null;

        private System.Threading.Thread AutoHastThread = null;

        private System.Threading.Thread AutoParalizeThread = null;

        private System.Threading.Thread AutoBleedingThread = null;

        private System.Threading.Thread AutoEletricThread = null;

        private System.Threading.Thread AutoCurseThread = null;

        #endregion

        public static Point PosAction = new Point();

        #endregion

        #region VALUE CHANGED

        private void cbCaptureAction_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCaptureAction.Checked)
            {
                this.cbCaptureAction.Text = CultureLanguage.ChangeLanguageName("WaitingCapture");
            }
            else
            {
                this.cbCaptureAction.Text = CultureLanguage.ChangeLanguageName("NotCapturing");
            }
        }

        private void cbAutoFood_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoFood.Checked)
                {
                    var hotkeyFood01 = this.cbHotkeyFood01.Text;
                    var hotkeyFood02 = this.cbHotkeyFood02.Text;

                    AutoFoodThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyFood01, hotkeyFood02, this.nudIntervaloFood, this.cbAutoFood, this.cbAutoEatHungry.Checked, BitmapHungry, BitmapHungryOld, true, "Executed Auto Food!", "IDFOOD"); });
                    AutoFoodThread.IsBackground = true;
                    AutoFoodThread.Name = "IDFOOD";
                    AutoFoodThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoFoodEnabled"), null);
                }
                else
                {
                    if (AutoFoodThread != null)
                    {
                        AutoFoodThread.Interrupt();
                        AutoFoodThread.Abort();
                        AutoFoodThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoFoodDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDFOOD");
                this.cbAutoFood.Checked = false;
            }
        }

        private void cbAutoManaShield_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoManaShield.Checked)
                {
                    var hotkeyMana01 = this.cbHotkeyManaShield01.Text;
                    var hotkeyMana02 = this.cbHotkeyManaShield02.Text;

                    AutoManaShieldThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyMana01, hotkeyMana02, this.nudIntervaloManaShield, this.cbAutoManaShield, true, BitmapMagicShield, BitmapMagicShieldOld, false, "Executed Auto Mana Shield!", "IDMANA"); });
                    AutoManaShieldThread.IsBackground = true;
                    AutoManaShieldThread.Name = "IDMANA";
                    AutoManaShieldThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoManaShieldEnabled"), null);
                }
                else
                {
                    if (AutoManaShieldThread != null)
                    {
                        AutoManaShieldThread.Interrupt();
                        AutoManaShieldThread.Abort();
                        AutoManaShieldThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoManaShieldDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDMANA");
                this.cbAutoManaShield.Checked = false;
            }
        }

        private void cbAutoCurePoison_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoCurePoison.Checked)
                {
                    var hotkeyCure01 = this.cbHotkeyCurePoison01.Text;
                    var hotkeyCure02 = this.cbHotkeyCurePoison02.Text;

                    AutoCurePoisonThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyCure01, hotkeyCure02, this.nudIntervaloCurePoison, this.cbAutoCurePoison, true, BitmapCurePoison, BitmapCurePoisonOld, true, "Executed Auto Cure Poison!", "IDPOIS"); });
                    AutoCurePoisonThread.IsBackground = true;
                    AutoCurePoisonThread.Name = "IDPOIS";
                    AutoCurePoisonThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCurePoisonEnabled"), null);
                }
                else
                {
                    if (AutoCurePoisonThread != null)
                    {
                        AutoCurePoisonThread.Interrupt();
                        AutoCurePoisonThread.Abort();
                        AutoCurePoisonThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCurePoisonDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDCURE");
                this.cbAutoCurePoison.Checked = false;
            }
        }

        private void cbAutoCureFire_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoCureFire.Checked)
                {
                    var hotkey01 = this.cbHotkeyCureFire01.Text;
                    var hotkey02 = this.cbHotkeyCureFire02.Text;

                    AutoCureFireThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkey01, hotkey02, this.nudIntervaloCureFire, this.cbAutoCureFire, true, BitmapCureFire, BitmapCureFireOld, true, "Executed Auto Cure Fire!", "IDFIRE"); });
                    AutoCureFireThread.IsBackground = true;
                    AutoCureFireThread.Name = "IDFIRE";
                    AutoCureFireThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureBurningEnabled"), null);
                }
                else
                {
                    if (AutoCureFireThread != null)
                    {
                        AutoCureFireThread.Interrupt();
                        AutoCureFireThread.Abort();
                        AutoCureFireThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureBurningDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDFIRE");
                this.cbAutoCureFire.Checked = false;
            }
        }

        private void cbAutoHast_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoHaste.Checked)
                {
                    var hotkeyHast01 = this.cbHotkeyHaste01.Text;
                    var hotkeyHast02 = this.cbHotkeyHaste02.Text;

                    AutoHastThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyHast01, hotkeyHast02, this.nudIntervaloHaste, this.cbAutoHaste, true, BitmapHast, BitmapHastOld, false, "Executed Auto Hast!", "IDHAST"); });
                    AutoHastThread.IsBackground = true;
                    AutoHastThread.Name = "IDHAST";
                    AutoHastThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoHasteEnabled"), null);
                }
                else
                {
                    if (AutoHastThread != null)
                    {
                        AutoHastThread.Interrupt();
                        AutoHastThread.Abort();
                        AutoHastThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoHasteDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDHAST");
                this.cbAutoHaste.Checked = false;
            }
        }

        private void cbAutoParalize_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoParalize.Checked)
                {
                    var hotkeyParalize01 = this.cbHotkeyParalize01.Text;
                    var hotkeyParalize02 = this.cbHotkeyParalize02.Text;

                    AutoParalizeThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyParalize01, hotkeyParalize02, this.nudIntervaloParalize, this.cbAutoParalize, true, BitmapParalize, BitmapParalizeOld, true, "Executed Auto Remove Paralize!", "IDPARA"); });
                    AutoParalizeThread.IsBackground = true;
                    AutoParalizeThread.Name = "IDPARA";
                    AutoParalizeThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoParalizeEnabled"), null);
                }
                else
                {
                    if (AutoParalizeThread != null)
                    {
                        AutoParalizeThread.Interrupt();
                        AutoParalizeThread.Abort();
                        AutoParalizeThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoParalizeDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDPARA");
                this.cbAutoParalize.Checked = false;
            }
        }

        private void cbAutoBleeding_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoBleeding.Checked)
                {
                    var hotkeyBleeding01 = this.cbHotkeyBleeding01.Text;
                    var hotkeyBleeding02 = this.cbHotkeyBleeding02.Text;

                    AutoBleedingThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyBleeding01, hotkeyBleeding02, this.nudIntervaloBleeding, this.cbAutoBleeding, true, BitmapBleeding, BitmapParalizeOld, true, "Executed Auto Cure Bleeding!", "IDBLEE"); });
                    AutoBleedingThread.IsBackground = true;
                    AutoBleedingThread.Name = "IDBLEE";
                    AutoBleedingThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureBleedingEnabled"), null);
                }
                else
                {
                    if (AutoBleedingThread != null)
                    {
                        AutoBleedingThread.Interrupt();
                        AutoBleedingThread.Abort();
                        AutoBleedingThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureBleedingDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDBLEE");
                this.cbAutoBleeding.Checked = false;
            }
        }

        private void cbAutoEletric_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoCureEletric.Checked)
                {
                    var hotkeyEletric01 = this.cbHotkeyCureEletric01.Text;
                    var hotkeyEletric02 = this.cbHotkeyCureEletric02.Text;

                    AutoEletricThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyEletric01, hotkeyEletric02, this.nudIntervaloCureEletric, this.cbAutoCureEletric, true, BitmapEletric, BitmapEletricOld, true, "Executed Auto Cure Electrification!", "IDELET"); });
                    AutoEletricThread.IsBackground = true;
                    AutoEletricThread.Name = "IDELET";
                    AutoEletricThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureElectrificationEnabled"), null);
                }
                else
                {
                    if (AutoEletricThread != null)
                    {
                        AutoEletricThread.Interrupt();
                        AutoEletricThread.Abort();
                        AutoEletricThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureElectrificationDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDELET");
                this.cbAutoCureEletric.Checked = false;
            }
        }

        private void cbAutoCurse_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbAutoCurse.Checked)
                {
                    var hotkeyCurse01 = this.cbHotkeyCurse01.Text;
                    var hotkeyCurse02 = this.cbHotkeyCurse02.Text;

                    AutoCurseThread = new System.Threading.Thread(() => { LoopAutoActionGenerico(hotkeyCurse01, hotkeyCurse02, this.nudIntervaloCurse, this.cbAutoCurse, true, BitmapCurse, BitmapCurseOld, true, "Executed Auto Cure Curse!", "IDCURSE"); });
                    AutoCurseThread.IsBackground = true;
                    AutoCurseThread.Name = "IDCURSE";
                    AutoCurseThread.Start();

                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureCurseEnabled"), null);
                }
                else
                {
                    if (AutoCurseThread != null)
                    {
                        AutoCurseThread.Interrupt();
                        AutoCurseThread.Abort();
                        AutoCurseThread = null;
                    }
                    BotLogs.add(CultureLanguage.ChangeLanguageName("AutoCureCurseDisabled"), null);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "IDCURSE");
                this.cbAutoCurse.Checked = false;
            }
        }

        private void cbHotkeyFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoFood.Checked = false;
        }

        private void cbHotkeyParalize_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoParalize.Checked = false;
        }

        private void cbHotkeyHaste_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoHaste.Checked = false;
        }

        private void cbHotkeyCurePoison_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoCurePoison.Checked = false;
        }

        private void cbHotkeyCureFire_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoCureFire.Checked = false;
        }

        private void cbHotkeyCureEletric_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoCureEletric.Checked = false;
        }

        private void cbHotkeyBleeding_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoBleeding.Checked = false;
        }

        private void cbHotkeyCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoCurse.Checked = false;
        }

        private void cbHotkeyManaShield_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoManaShield.Checked = false;
        }

        #endregion

        private void ActivateAutoFunctionGeneric(CheckBox check, ComboBox hotkey01, ComboBox hotkey02, NumericUpDown interval, AutoAction autoAction)
        {
            check.Checked = autoAction.Enable;
            hotkey01.Text = autoAction.HotKey01.Trim();
            hotkey02.Text = autoAction.HotKey02.Trim();
            interval.Value = autoAction.Interval;
        }

        private void LoopAutoActionGenerico(string hotkey01, string hotkey02, NumericUpDown intervalo, CheckBox checkBox, bool compararImagem, Bitmap BitmapComparar, Bitmap BitmapCompararOld, bool presente, string log, string idThread)
        {
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(intervalo.Value));

                    if (TibiaPrimeiraJanela)
                    {
                        if ((PosAction.X > 0 && PosAction.Y > 0) || !compararImagem)
                        {
                            #region Tratar ComboBox
                            if (!TratarCombox(hotkey01, new List<string>() { "Empty", "Shift +", "Ctrl +" }))
                            {
                                checkBox.Checked = false;
                                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                                break;
                            }

                            if (!TratarCombox(hotkey02, new List<string>() { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" }))
                            {
                                checkBox.Checked = false;
                                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                                break;
                            }
                            #endregion

                            var executar = false;

                            if (compararImagem)
                            {
                                Bitmap squareAction = BitmapExtensions.CloneBitmap(bitmapQuadranteCaputrado);
                                Bitmap imageCompare = null;

                                if (VersaoAtualTibia)
                                {
                                    imageCompare = BitmapExtensions.CloneBitmap(BitmapComparar);
                                }
                                else
                                {
                                    imageCompare = BitmapExtensions.CloneBitmap(BitmapCompararOld);
                                }

                                if (BitmapExtensions.VerificarAcaoTela(squareAction, imageCompare))
                                {
                                    executar = presente ? true : false;
                                }
                                else
                                {
                                    executar = !presente ? true : false;
                                }
                            }
                            else
                            {
                                executar = true;
                            }

                            if (executar)
                            {
                                if (hotkey01.Trim().Equals("Shift +"))
                                {
                                    SendKeys.SendWait("+" + "{" + hotkey02.Trim() + "}");
                                }
                                else if (hotkey01.Trim().Equals("Ctrl +"))
                                {
                                    SendKeys.SendWait("^" + "{" + hotkey02.Trim() + "}");
                                }
                                else if (hotkey01.Trim().Equals("Empty") || hotkey01.Trim().Equals(""))
                                {
                                    SendKeys.SendWait("{" + hotkey02.Trim() + "}");
                                }

                                BotLogs.add(log, null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, idThread);
                }
            }
            try
            {
                checkBox.Checked = false;
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, idThread);
            }
        }

        private bool VerificarAlgumAutoFunctionLigado()
        {
            return this.cbAutoBleeding.Checked ||
            this.cbAutoCureEletric.Checked ||
            this.cbAutoCureFire.Checked ||
            this.cbAutoCurePoison.Checked ||
            this.cbAutoCurse.Checked ||
            this.cbAutoHaste.Checked ||
            this.cbAutoParalize.Checked ||
            this.cbAutoManaShield.Checked ||
            (this.cbAutoFood.Checked && this.cbAutoEatHungry.Checked) ? true : false;
        }

        #endregion

        #region POINT

        #region VARIAVEIS

        public int idThreadPoint = 0;

        public static List<int> idThreadsStopPoint = new List<int>();

        #endregion

        #region VALUES CHANGED

        private void rbSpellPoint_CheckedChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPoint();
            this.tbSpellPoint.Visible = true;
            this.cbHotkey01Point.Visible = false;
            this.cbHotkey02Point.Visible = false;
        }

        private void rbHotkeyPoint_CheckedChanged(object sender, EventArgs e)
        {
            this.tbSpellPoint.Visible = false;
            this.cbHotkey01Point.Visible = true;
            this.cbHotkey02Point.Visible = true;
        }

        private void cbCapturePoint_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCapturePoint.Checked)
            {
                this.cbCapturePoint.Text = CultureLanguage.ChangeLanguageName("WaitingCapture");
            }
            else
            {
                this.cbCapturePoint.Text = CultureLanguage.ChangeLanguageName("NotCapturing");
            }
        }

        private void cbHotkey01Point_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region Tratar ComboBox
                if (!TratarCombox(this.cbHotkey01Point.Text, new List<string>() { "Empty", "Shift +", "Ctrl +" }))
                {
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                }
                #endregion
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

        }

        private void cbHotkey02Point_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region Tratar ComboBox
                if (!TratarCombox(this.cbHotkey02Point.Text, new List<string>() { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" }))
                {
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                }
                #endregion
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void tbSpellPoint_TextChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPoint();
        }

        #endregion

        #region BUTTON

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            AddFunctionPoint(true);
        }

        private void AddFunctionPoint(bool functionActive)
        {
            try
            {
                idThreadPoint++;

                #region Tratar ComboBox
                if (!TratarCombox(this.cbHotkey01Point.Text, new List<string>() { "Empty", "Shift +", "Ctrl +" }))
                {
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                    throw new Exception();
                }

                if (!TratarCombox(this.cbHotkey02Point.Text, new List<string>() { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" }))
                {
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                    throw new Exception();
                }
                #endregion

                if (this.rbHotkeyPoint.Checked)
                {
                    this.dgvFunctionPoint.Rows.Add(new string[]
                    {
                functionActive.ToString(),
                this.tbPosXPoint.Text.Trim(),
                this.tbPosYPoint.Text.Trim(),
                this.rbHotkeyPoint.Checked.ToString(),
                string.Empty,
                this.cbHotkey01Point.Text.Trim(),
                this.cbHotkey02Point.Text.Trim(),
                this.rbAusentePoint.Checked.ToString(),
                this.rbLifePoint.Checked.ToString(),
                this.nudIntervaloPoint.Value.ToString(),
                this.nudPrioridadePoint.Value.ToString(),
                idThreadPoint.ToString()
                    });
                    HandlerFunction.RulesPoint.Add(new FunctionRulePoint(idThreadPoint, functionActive, int.Parse(this.tbPosXPoint.Text.Trim()), int.Parse(this.tbPosYPoint.Text.Trim()), this.rbHotkeyPoint.Checked, string.Empty, this.cbHotkey01Point.Text.Equals("Empty") ? string.Empty : this.cbHotkey01Point.Text.Trim(), this.cbHotkey02Point.Text.Trim(), this.rbAusentePoint.Checked, this.rbLifePoint.Checked, Convert.ToInt32(this.nudIntervaloPoint.Value), Convert.ToInt32(this.nudPrioridadePoint.Value)));

                }
                else
                {
                    this.dgvFunctionPoint.Rows.Add(new string[]
                    {
                functionActive.ToString(),
                this.tbPosXPoint.Text.Trim(),
                this.tbPosYPoint.Text.Trim(),
                this.rbHotkeyPoint.Checked.ToString(),
                this.tbSpellPoint.Text.Trim(),
                string.Empty,
                string.Empty,
                this.rbAusentePoint.Checked.ToString(),
                this.rbLifePoint.Checked.ToString(),
                this.nudIntervaloPoint.Value.ToString(),
                this.nudPrioridadePoint.Value.ToString(),
                idThreadPoint.ToString()
                    });
                    HandlerFunction.RulesPoint.Add(new FunctionRulePoint(idThreadPoint, functionActive, int.Parse(this.tbPosXPoint.Text.Trim()), int.Parse(this.tbPosYPoint.Text.Trim()), this.rbHotkeyPoint.Checked, this.tbSpellPoint.Text.Trim(), string.Empty, string.Empty, this.rbAusentePoint.Checked, this.rbLifePoint.Checked, Convert.ToInt32(this.nudIntervaloPoint.Value), Convert.ToInt32(this.nudPrioridadePoint.Value)));
                }

                LimparCamposPoint();

                AtivarBtnAddPoint();

                BotLogs.add(CultureLanguage.ChangeLanguageName("VerificationAdded"), null);

            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void btnEditPoint_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPoint.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        this.tbPosXPoint.Text = dataGridViewRow.Cells[1].Value.ToString();
                        this.tbPosYPoint.Text = dataGridViewRow.Cells[2].Value.ToString();
                        this.rbHotkeyPoint.Checked = Convert.ToBoolean(dataGridViewRow.Cells[3].Value.ToString());
                        this.rbSpellPoint.Checked = !Convert.ToBoolean(dataGridViewRow.Cells[3].Value.ToString());
                        this.tbSpellPoint.Text = dataGridViewRow.Cells[4].Value.ToString();
                        this.cbHotkey01Point.Text = dataGridViewRow.Cells[5].Value.ToString();
                        this.cbHotkey02Point.Text = dataGridViewRow.Cells[6].Value.ToString();
                        this.rbAusentePoint.Checked = Convert.ToBoolean(dataGridViewRow.Cells[7].Value.ToString());
                        this.rbPresentePoint.Checked = !Convert.ToBoolean(dataGridViewRow.Cells[7].Value.ToString());
                        this.rbLifePoint.Checked = Convert.ToBoolean(dataGridViewRow.Cells[8].Value.ToString());
                        this.rbManaPoint.Checked = !Convert.ToBoolean(dataGridViewRow.Cells[8].Value.ToString());
                        this.nudIntervaloPoint.Value = int.Parse(dataGridViewRow.Cells[9].Value.ToString());
                        this.nudPrioridadePoint.Value = int.Parse(dataGridViewRow.Cells[10].Value.ToString());

                        DeleteFunctionPoint(dataGridViewRow);

                        break;
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
            }

            AtivarBtnAddPoint();
        }

        private void btnDeletePoint_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPoint.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        DeleteFunctionPoint(dataGridViewRow);
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }

                BotLogs.add(CultureLanguage.ChangeLanguageName("VerificationExcluded"), null);
            }

            AtivarBtnAddPoint();
        }

        private void btnStartPoint_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPoint.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        if (!Convert.ToBoolean(dataGridViewRow.Cells[0].Value.ToString()))
                        {
                            var idThread = int.Parse(dataGridViewRow.Cells[11].Value.ToString());

                            foreach (DataGridViewRow loop2 in this.dgvFunctionPoint.Rows)
                            {
                                if (idThread == int.Parse(loop2.Cells[11].Value.ToString()))
                                {
                                    try
                                    {
                                        loop2.Cells[0].Value = true;
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                                    }
                                }
                            }

                            try
                            {
                                HandlerFunction.RulesPoint.Where(w => w.Id == idThread).FirstOrDefault((f => f.Active = true));
                            }
                            catch (Exception ex)
                            {
                                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                            }

                            try
                            {
                                HandlerFunction.CreateThreadPoint(HandlerFunction.RulesPoint.Where(w => w.Id == idThread).FirstOrDefault());
                            }
                            catch (Exception ex)
                            {
                                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                            }

                            BotLogs.add(null, new Exception("START THREAD: IDPOI" + idThread));
                        }
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
            }
        }

        private void btnStopPoint_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPoint.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        StopFunctionPointById(int.Parse(dataGridViewRow.Cells[11].Value.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
                BotLogs.add(CultureLanguage.ChangeLanguageName("VerificationStopped"), null);
            }
        }

        #endregion

        private void DeleteAllFunctionPoint()
        {
            foreach (var loop in HandlerFunction.threadListPoint)
            {
                try
                {
                    loop.Interrupt();
                    loop.Abort();
                    BotLogs.add(null, new Exception("REMOVE THREAD: " + loop.Name));
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, loop.Name);
                }
            }
            HandlerFunction.threadListPoint = new List<System.Threading.Thread>();

            HandlerFunction.RulesPoint = new List<FunctionRulePoint>();

            HandlerFunction.ControlePrioridadeListPoint = new List<ControlePrioridade>();

            this.dgvFunctionPoint.Rows.Clear();

        }

        private void DeleteFunctionPoint(DataGridViewRow dataGridViewRow)
        {
            var idFunctionPoint = int.Parse(dataGridViewRow.Cells[11].Value.ToString());

            #region REMOVE FROM THREAD
            foreach (var loop in HandlerFunction.threadListPoint)
            {
                if (loop.Name.Equals(("IDPOI" + idFunctionPoint.ToString())))
                {
                    try
                    {
                        loop.Interrupt();
                        loop.Abort();
                        BotLogs.add(null, new Exception("REMOVE THREAD: " + loop.Name));
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, loop.Name);
                    }
                    finally
                    {
                        try
                        {
                            HandlerFunction.threadListPoint.Remove(loop);
                        }
                        catch (Exception ex)
                        {
                            BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                        }
                    }
                    break;
                }
            }

            #endregion

            #region REMOVE FROM RulesPoint

            foreach (var loop in HandlerFunction.RulesPoint)
            {
                if (loop.Id == idFunctionPoint)
                {
                    try
                    {
                        HandlerFunction.RulesPoint.Remove(loop);
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }

            #endregion

            #region REMOVE FROM ControlePrioridadeListPoint

            foreach (var loop in HandlerFunction.ControlePrioridadeListPoint)
            {
                if (loop.Id == idFunctionPoint)
                {
                    try
                    {
                        HandlerFunction.ControlePrioridadeListPoint.Remove(loop);
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }

            #endregion

            this.dgvFunctionPoint.Rows.Remove(dataGridViewRow);
        }

        private void StopFunctionPointById(int idThread)
        {
            foreach (DataGridViewRow loop in this.dgvFunctionPoint.Rows)
            {
                if (idThread == int.Parse(loop.Cells[11].Value.ToString()))
                {
                    try
                    {
                        loop.Cells[0].Value = false;
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }

            try
            {
                HandlerFunction.RulesPoint.Where(w => w.Id == idThread).FirstOrDefault((f => f.Active = false));
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            foreach (var loop in HandlerFunction.threadListPoint)
            {
                if (loop.Name.Equals(("IDPOI" + idThread)))
                {
                    try
                    {
                        loop.Interrupt();
                        loop.Abort();
                        BotLogs.add(null, new Exception("STOP THREAD: " + loop.Name));
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, loop.Name);
                    }
                    finally
                    {
                        try
                        {
                            HandlerFunction.threadListPoint.Remove(loop);
                        }
                        catch (Exception ex)
                        {
                            BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                        }
                    }
                    break;
                }
            }

            foreach (var loop in HandlerFunction.ControlePrioridadeListPoint)
            {
                if (loop.Id == idThread)
                {
                    try
                    {
                        HandlerFunction.ControlePrioridadeListPoint.Remove(loop);
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }
        }

        private void InicializarDataGridViewPoint()
        {
            try
            {
                this.dgvFunctionPoint.ColumnCount = 11;
                DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                dataGridViewCheckBoxColumn.HeaderText = "Active";
                dataGridViewCheckBoxColumn.FalseValue = false;
                dataGridViewCheckBoxColumn.TrueValue = true;
                this.dgvFunctionPoint.Columns.Insert(0, dataGridViewCheckBoxColumn);
                this.dgvFunctionPoint.Columns[1].Name = "Pos. X";
                this.dgvFunctionPoint.Columns[2].Name = "Pos. Y";
                this.dgvFunctionPoint.Columns[3].Name = "HotKey(True) or Spell(False)";
                this.dgvFunctionPoint.Columns[4].Name = "Spell";
                this.dgvFunctionPoint.Columns[5].Name = "HotKey 01";
                this.dgvFunctionPoint.Columns[6].Name = "HotKey 02";
                this.dgvFunctionPoint.Columns[7].Name = "Absent(True) or Present(False)";
                this.dgvFunctionPoint.Columns[8].Name = "Health(True) or Mana(False)";
                this.dgvFunctionPoint.Columns[9].Name = "Interval";
                this.dgvFunctionPoint.Columns[10].Name = "Priority";

                this.dgvFunctionPoint.Columns[11].Name = "ID";
                this.dgvFunctionPoint.Columns[11].Visible = false;


                HandlerFunction.RulesPoint.ForEach(delegate (FunctionRulePoint rule)
                {
                    this.dgvFunctionPoint.Rows.Add(new string[]
                    {
                    idThreadPoint.ToString(),
                    rule.Active.ToString().Trim(),
                    rule.PosX.ToString().Trim(),
                    rule.PoxY.ToString().Trim(),
                    rule.Hotkey.ToString().Trim(),
                    rule.Spell.ToString().Trim(),
                    rule.Hotkey01.ToString().Trim(),
                    rule.Hotkey02.ToString().Trim(),
                    rule.Ausent.ToString().Trim(),
                    rule.Health.ToString().Trim(),
                    nudIntervaloPoint.Value.ToString().Trim(),
                    nudPrioridadePoint.Value.ToString().Trim()
                    });
                });

                this.dgvFunctionPoint.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void AtivarBtnAddPoint()
        {
            if (!string.IsNullOrEmpty(this.tbPosXPoint.Text.Trim()) && !string.IsNullOrEmpty(this.tbPosYPoint.Text.Trim()))
            {
                if (this.rbHotkeyPoint.Checked)
                {
                    this.btnAddPoint.Enabled = true;
                }
                else
                {
                    this.btnAddPoint.Enabled = !string.IsNullOrEmpty(this.tbSpellPoint.Text.Trim());
                }
            }
            else
            {
                this.btnAddPoint.Enabled = false;
            }
        }

        private void LimparCamposPoint()
        {
            this.tbPosXPoint.Text = string.Empty;
            this.tbPosYPoint.Text = string.Empty;
            this.rbHotkeyPoint.Checked = true;
            this.tbSpellPoint.Text = string.Empty;
            this.cbHotkey01Point.Text = "Empty";
            this.cbHotkey02Point.Text = "F1";
            this.rbAusentePoint.Checked = true;
            this.rbLifePoint.Checked = true;
            this.nudIntervaloPoint.Value = 500;
            this.nudPrioridadePoint.Value = 0;
        }

        #endregion

        #region PORCENT

        #region VARIAVEIS

        public int idThreadPorcent = 0;

        public static List<int> idThreadsStopPorcent = new List<int>();

        private int TentativasPorc = 0;

        public static Point PointHeart = new Point();

        public static Point PosMinHealth = new Point();
        public static Point PosMaxHealth = new Point();

        public static Point PosMinMana = new Point();
        public static Point PosMaxMana = new Point();

        #endregion

        #region VALUE CHANGED

        private void cbCaptureMinHelPorc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCaptureMinHelPorc.Checked)
            {
                this.cbCaptureMinHelPorc.Text = CultureLanguage.ChangeLanguageName("WaitingCapture");
            }
            else
            {
                this.cbCaptureMinHelPorc.Text = CultureLanguage.ChangeLanguageName("NotCapturing");
            }
        }

        private void cbCaptureMaxHelPorc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCaptureMaxHelPorc.Checked)
            {
                this.cbCaptureMaxHelPorc.Text = CultureLanguage.ChangeLanguageName("WaitingCapture");
            }
            else
            {
                this.cbCaptureMaxHelPorc.Text = CultureLanguage.ChangeLanguageName("NotCapturing");
            }
        }

        private void cbCaptureMinManPorc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCaptureMinManPorc.Checked)
            {
                this.cbCaptureMinManPorc.Text = CultureLanguage.ChangeLanguageName("WaitingCapture");
            }
            else
            {
                this.cbCaptureMinManPorc.Text = CultureLanguage.ChangeLanguageName("NotCapturing");
            }
        }

        private void cbCaptureMaxManPorc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbCaptureMaxManPorc.Checked)
            {
                this.cbCaptureMaxManPorc.Text = CultureLanguage.ChangeLanguageName("WaitingCapture");
            }
            else
            {
                this.cbCaptureMaxManPorc.Text = CultureLanguage.ChangeLanguageName("NotCapturing");
            }
        }

        private void cbAutoScanPorcent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbAutoScanPorcent.Checked)
            {
                BotLogs.add(CultureLanguage.ChangeLanguageName("AutoScanEnabled"), null);
            }
            else
            {
                BotLogs.add(CultureLanguage.ChangeLanguageName("AutoScanDisabled"), null);
            }
        }

        private void rbHotkeyPorcent_CheckedChanged(object sender, EventArgs e)
        {
            this.tbSpellPorcent.Visible = false;
            this.cbHotkey01Porcent.Visible = true;
            this.cbHotkey02Porcent.Visible = true;
        }

        private void rbSpellPorcent_CheckedChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPorcent();
            this.tbSpellPorcent.Visible = true;
            this.cbHotkey01Porcent.Visible = false;
            this.cbHotkey02Porcent.Visible = false;
        }

        private void tbSpellPorcent_TextChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPorcent();
        }

        private void nudPorcentMinHealth_ValueChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPorcent();
        }

        private void nudPorcentMaxHealth_ValueChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPorcent();
        }

        private void nudPorcentMinMana_ValueChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPorcent();
        }

        private void nudPorcentMaxMana_ValueChanged(object sender, EventArgs e)
        {
            AtivarBtnAddPorcent();
        }

        private void cbEnableManaPorcent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbEnableManaPorcent.Checked)
            {
                this.cbEnableManaPorcent.Text = "Enabled";
                this.nudPorcentMinMana.Enabled = true;
                this.nudPorcentMaxMana.Enabled = true;
                //this.nudPorcentMinMana.Value = 0;
                //this.nudPorcentMaxMana.Value = 100;
            }
            else
            {
                this.cbEnableManaPorcent.Text = "Disabled";
                this.nudPorcentMinMana.Enabled = false;
                this.nudPorcentMaxMana.Enabled = false;
                //this.nudPorcentMinMana.Value = 0;
                //this.nudPorcentMaxMana.Value = 0;
            }
        }

        private void cbEnableHealthPorcent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbEnableHealthPorcent.Checked)
            {
                this.cbEnableHealthPorcent.Text = "Enabled";
                this.nudPorcentMinHealth.Enabled = true;
                this.nudPorcentMaxHealth.Enabled = true;
                //this.nudPorcentMinHealth.Value = 0;
                //this.nudPorcentMaxHealth.Value = 100;
            }
            else
            {
                this.cbEnableHealthPorcent.Text = "Disabled";
                this.nudPorcentMinHealth.Enabled = false;
                this.nudPorcentMaxHealth.Enabled = false;
                //this.nudPorcentMinHealth.Value = 0;
                //this.nudPorcentMaxHealth.Value = 0;
            }
        }

        #endregion

        #region BUTTON

        private void btnAddPorcent_Click(object sender, EventArgs e)
        {
            AddFunctionPorcent(true);
        }

        private void AddFunctionPorcent(bool functionActive)
        {
            try
            {

                #region Tratar ComboBox
                if (!TratarCombox(this.cbHotkey01Porcent.Text, new List<string>() { "Empty", "Shift +", "Ctrl +" }))
                {
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                    throw new Exception();
                }

                if (!TratarCombox(this.cbHotkey02Porcent.Text, new List<string>() { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" }))
                {
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                    throw new Exception();
                }

                #endregion

                if (nudPorcentMaxHealth.Value < nudPorcentMinHealth.Value)
                {
                    nudPorcentMaxHealth.Value = 100;
                    nudPorcentMinHealth.Value = 0;
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                    throw new Exception();
                }

                if (nudPorcentMaxMana.Value < nudPorcentMinMana.Value)
                {
                    nudPorcentMaxMana.Value = 100;
                    nudPorcentMinMana.Value = 0;
                    FormMessageBox.Show(CultureLanguage.ChangeLanguageName("ValueIncorrectly"), CultureLanguage.ChangeLanguageName("Problems"));
                    throw new Exception();
                }

                idThreadPorcent++;

                this.dgvFunctionPorcent.Rows.Add(new string[]
                {
                functionActive.ToString(),
                this.nudPorcentMinHealth.Value.ToString(),
                this.nudPorcentMaxHealth.Value.ToString(),
                this.nudPorcentMinMana.Value.ToString(),
                this.nudPorcentMaxMana.Value.ToString(),
                this.rbHotkeyPorcent.Checked.ToString(),
                this.rbHotkeyPorcent.Checked ? string.Empty :  this.tbSpellPorcent.Text.Trim(),
                this.rbHotkeyPorcent.Checked ? this.cbHotkey01Porcent.Text.Trim() : string.Empty,
                this.rbHotkeyPorcent.Checked ? this.cbHotkey02Porcent.Text.Trim() : string.Empty,
                this.rbAusentePorcentHealth.Checked.ToString(),
                this.rbAusentePorcentMana.Checked.ToString(),
                this.nudIntervaloPorcent.Value.ToString(),
                this.nudPrioridadePorcent.Value.ToString(),
                idThreadPorcent.ToString(),
                this.cbEnableHealthPorcent.Checked.ToString(),
                this.cbEnableManaPorcent.Checked.ToString()
                });
                HandlerFunction.RulesPorcent.Add(
                    new FunctionRulePorcent(
                        idThreadPorcent,
                        functionActive,
                        Convert.ToInt32(this.nudPorcentMinHealth.Value),
                        Convert.ToInt32(this.nudPorcentMaxHealth.Value),
                        Convert.ToInt32(this.nudPorcentMinMana.Value),
                        Convert.ToInt32(this.nudPorcentMaxMana.Value),
                        this.rbHotkeyPorcent.Checked,
                        this.rbHotkeyPorcent.Checked ? string.Empty : this.tbSpellPorcent.Text.Trim(),
                        this.rbHotkeyPorcent.Checked ? (this.cbHotkey01Porcent.Text.Equals("Empty") ? string.Empty : this.cbHotkey01Porcent.Text.Trim()) : string.Empty,
                        this.rbHotkeyPorcent.Checked ? this.cbHotkey02Porcent.Text.Trim() : string.Empty,
                        this.rbAusentePorcentHealth.Checked,
                        this.rbAusentePorcentMana.Checked,
                        Convert.ToInt32(this.nudIntervaloPorcent.Value),
                        Convert.ToInt32(this.nudPrioridadePorcent.Value),
                        this.cbEnableHealthPorcent.Checked,
                        this.cbEnableManaPorcent.Checked
                        ));

                LimparCamposPorcent();

                BotLogs.add(CultureLanguage.ChangeLanguageName("VerificationAdded"), null);

            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            AtivarBtnAddPorcent();
        }

        private void btnEditPorcent_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPorcent.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        this.nudPorcentMinHealth.Value = int.Parse(dataGridViewRow.Cells[1].Value.ToString());
                        this.nudPorcentMaxHealth.Value = int.Parse(dataGridViewRow.Cells[2].Value.ToString());
                        this.nudPorcentMinMana.Value = int.Parse(dataGridViewRow.Cells[3].Value.ToString());
                        this.nudPorcentMaxMana.Value = int.Parse(dataGridViewRow.Cells[4].Value.ToString());
                        this.rbHotkeyPorcent.Checked = Convert.ToBoolean(dataGridViewRow.Cells[5].Value.ToString());
                        this.rbSpellPorcent.Checked = !Convert.ToBoolean(dataGridViewRow.Cells[5].Value.ToString());
                        this.tbSpellPorcent.Text = dataGridViewRow.Cells[6].Value.ToString();
                        this.cbHotkey01Porcent.Text = dataGridViewRow.Cells[7].Value.ToString();
                        this.cbHotkey02Porcent.Text = dataGridViewRow.Cells[8].Value.ToString();
                        this.rbAusentePorcentHealth.Checked = Convert.ToBoolean(dataGridViewRow.Cells[9].Value.ToString());
                        this.rbPresentePorcentHealth.Checked = !Convert.ToBoolean(dataGridViewRow.Cells[9].Value.ToString());
                        this.rbAusentePorcentMana.Checked = Convert.ToBoolean(dataGridViewRow.Cells[10].Value.ToString());
                        this.rbPresentePorcentMana.Checked = !Convert.ToBoolean(dataGridViewRow.Cells[10].Value.ToString());
                        this.nudIntervaloPorcent.Value = int.Parse(dataGridViewRow.Cells[11].Value.ToString());
                        this.nudPrioridadePorcent.Value = int.Parse(dataGridViewRow.Cells[12].Value.ToString());
                        this.cbEnableHealthPorcent.Checked = Convert.ToBoolean(dataGridViewRow.Cells[14].Value.ToString());
                        this.cbEnableManaPorcent.Checked = Convert.ToBoolean(dataGridViewRow.Cells[15].Value.ToString());
                        DeleteFunctionPorcent(dataGridViewRow);

                        break;
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
            }

            AtivarBtnAddPorcent();
        }

        private void btnDeletePorcent_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPorcent.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        DeleteFunctionPorcent(dataGridViewRow);
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }

                BotLogs.add(CultureLanguage.ChangeLanguageName("VerificationExcluded"), null);
            }

            AtivarBtnAddPorcent();
        }

        private void btnStartPorcent_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPorcent.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        if (!Convert.ToBoolean(dataGridViewRow.Cells[0].Value.ToString()))
                        {
                            var idThread = int.Parse(dataGridViewRow.Cells[13].Value.ToString());

                            foreach (DataGridViewRow loop2 in this.dgvFunctionPorcent.Rows)
                            {
                                if (idThread == int.Parse(loop2.Cells[13].Value.ToString()))
                                {
                                    try
                                    {
                                        loop2.Cells[0].Value = true;
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                                    }
                                }
                            }

                            try
                            {
                                HandlerFunction.RulesPorcent.Where(w => w.Id == idThread).FirstOrDefault((f => f.Active = true));
                            }
                            catch (Exception ex)
                            {
                                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                            }

                            try
                            {
                                HandlerFunction.CreateThreadPorcent(HandlerFunction.RulesPorcent.Where(w => w.Id == idThread).FirstOrDefault());
                            }
                            catch (Exception ex)
                            {
                                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                            }

                            BotLogs.add(null, new Exception("START THREAD: IDPOR" + idThread));
                        }
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
            }
        }

        private void btnStopPorcent_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in this.dgvFunctionPorcent.SelectedRows)
            {
                try
                {
                    if (dataGridViewRow.Selected)
                    {
                        StopFunctionPorcentById(int.Parse(dataGridViewRow.Cells[13].Value.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                }
                BotLogs.add(CultureLanguage.ChangeLanguageName("VerificationStopped"), null);
            }
        }

        #endregion

        delegate void TrheadDelegate();
        private void AtualizarPosicoesPorcent()
        {
            this.tbPosXHelMinPorc.Text = PosMinHealth.X.ToString();
            this.tbPosYHelMinPorc.Text = PosMinHealth.Y.ToString();

            this.tbPosXHelMaxPorc.Text = PosMaxHealth.X.ToString();
            this.tbPosYHelMaxPorc.Text = PosMaxHealth.Y.ToString();

            this.tbPosXManMinPorc.Text = PosMinMana.X.ToString();
            this.tbPosYManMinPorc.Text = PosMinMana.Y.ToString();

            this.tbPosXManMaxPorc.Text = PosMaxMana.X.ToString();
            this.tbPosYManMaxPorc.Text = PosMaxMana.Y.ToString();

            AtivarBtnAddPorcent();
        }
        private void LimparPosicoesPorcent()
        {
            if (TentativasPorc >= Properties.Settings.Default.AutoScanAttempts)
            {
                this.cbAutoScanPorcent.Checked = false;
                TentativasPorc = 0;
                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("AutoScanCould"), CultureLanguage.ChangeLanguageName("Problems"));
            }

            this.tbPosXHelMinPorc.Text = string.Empty;
            this.tbPosYHelMinPorc.Text = string.Empty;

            this.tbPosXHelMaxPorc.Text = string.Empty;
            this.tbPosYHelMaxPorc.Text = string.Empty;

            this.tbPosXManMinPorc.Text = string.Empty;
            this.tbPosYManMinPorc.Text = string.Empty;

            this.tbPosXManMaxPorc.Text = string.Empty;
            this.tbPosYManMaxPorc.Text = string.Empty;

            PosMinHealth = new Point();
            PosMaxHealth = new Point();
            PosMinMana = new Point();
            PosMaxMana = new Point();

            AtivarBtnAddPorcent();
        }

        private void CapturarPosicaoVidaManaPorcentagem()
        {
            try
            {
                if (this.cbAutoScanPorcent.Checked && BitmapHeart != null)
                {
                    PointHeart = BitmapExtensions.VerificarPosicaoCoracaoVida(BitmapHeart);

                    if (PointHeart != new Point())
                    {
                        TentativasPorc = 0;

                        PosMinHealth = new Point((PointHeart.X + 16), (PointHeart.Y + 4));
                        PosMaxHealth = new Point((PosMinHealth.X + 87), PosMinHealth.Y);

                        PosMinMana = new Point((PosMinHealth.X), PosMinHealth.Y + 13);
                        PosMaxMana = new Point((PosMaxHealth.X), PosMinMana.Y);

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new TrheadDelegate(AtualizarPosicoesPorcent));
                        }
                        else
                        {
                            this.tbPosXHelMinPorc.Text = PosMinHealth.X.ToString();
                            this.tbPosYHelMinPorc.Text = PosMinHealth.Y.ToString();

                            this.tbPosXHelMaxPorc.Text = PosMaxHealth.X.ToString();
                            this.tbPosYHelMaxPorc.Text = PosMaxHealth.Y.ToString();

                            this.tbPosXManMinPorc.Text = PosMinMana.X.ToString();
                            this.tbPosYManMinPorc.Text = PosMinMana.Y.ToString();

                            this.tbPosXManMaxPorc.Text = PosMaxMana.X.ToString();
                            this.tbPosYManMaxPorc.Text = PosMaxMana.Y.ToString();

                            AtivarBtnAddPorcent();
                        }
                    }
                    else
                    {
                        TentativasPorc++;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new TrheadDelegate(LimparPosicoesPorcent));
                        }
                        else
                        {
                            if (TentativasPorc >= Properties.Settings.Default.AutoScanAttempts)
                            {
                                this.cbAutoScanPorcent.Checked = false;
                                TentativasPorc = 0;
                                FormMessageBox.Show(CultureLanguage.ChangeLanguageName("AutoScanCould"), CultureLanguage.ChangeLanguageName("Problems"));
                            }

                            this.tbPosXHelMinPorc.Text = string.Empty;
                            this.tbPosYHelMinPorc.Text = string.Empty;

                            this.tbPosXHelMaxPorc.Text = string.Empty;
                            this.tbPosYHelMaxPorc.Text = string.Empty;

                            this.tbPosXManMinPorc.Text = string.Empty;
                            this.tbPosYManMinPorc.Text = string.Empty;

                            this.tbPosXManMaxPorc.Text = string.Empty;
                            this.tbPosYManMaxPorc.Text = string.Empty;

                            PosMinHealth = new Point();
                            PosMaxHealth = new Point();
                            PosMinMana = new Point();
                            PosMaxMana = new Point();

                            AtivarBtnAddPorcent();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void AtivarBtnAddPorcent()
        {
            if (PosMinHealth != new Point() && PosMaxHealth != new Point() &&
                PosMinMana != new Point() && PosMaxMana != new Point() &&
                ((this.nudPorcentMinHealth.Value > 0 || this.nudPorcentMaxHealth.Value > 0) ||
                (this.nudPorcentMinMana.Value > 0 || this.nudPorcentMaxMana.Value > 0)))
            {
                if (this.rbHotkeyPorcent.Checked)
                {
                    this.btnAddPorcent.Enabled = true;
                }
                else
                {
                    this.btnAddPorcent.Enabled = !string.IsNullOrEmpty(this.tbSpellPorcent.Text.Trim());
                }
            }
            else
            {
                this.btnAddPorcent.Enabled = false;
            }
        }

        private void InicializarDataGridViewPorcent()
        {
            try
            {
                this.dgvFunctionPorcent.ColumnCount = 15;
                DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                dataGridViewCheckBoxColumn.HeaderText = "Active";
                dataGridViewCheckBoxColumn.FalseValue = false;
                dataGridViewCheckBoxColumn.TrueValue = true;
                this.dgvFunctionPorcent.Columns.Insert(0, dataGridViewCheckBoxColumn);
                this.dgvFunctionPorcent.Columns[1].Name = "% Min. Health";
                this.dgvFunctionPorcent.Columns[2].Name = "% Max. Health";
                this.dgvFunctionPorcent.Columns[3].Name = "% Min. Mana";
                this.dgvFunctionPorcent.Columns[4].Name = "% Max. Mana";
                this.dgvFunctionPorcent.Columns[5].Name = "HotKey(True) or Spell(False)";
                this.dgvFunctionPorcent.Columns[6].Name = "Spell";
                this.dgvFunctionPorcent.Columns[7].Name = "HotKey 01";
                this.dgvFunctionPorcent.Columns[8].Name = "HotKey 02";
                this.dgvFunctionPorcent.Columns[9].Name = "Absent(True) or Present(False) Health";
                this.dgvFunctionPorcent.Columns[10].Name = "Absent(True) or Present(False) Mana";
                this.dgvFunctionPorcent.Columns[11].Name = "Interval";
                this.dgvFunctionPorcent.Columns[12].Name = "Priority";

                this.dgvFunctionPorcent.Columns[13].Name = "ID";
                this.dgvFunctionPorcent.Columns[13].Visible = false;

                this.dgvFunctionPorcent.Columns[14].Name = "Enabled Health";
                this.dgvFunctionPorcent.Columns[14].Visible = false;

                this.dgvFunctionPorcent.Columns[15].Name = "Enabled Mana";
                this.dgvFunctionPorcent.Columns[15].Visible = false;


                HandlerFunction.RulesPorcent.ForEach(delegate (FunctionRulePorcent rule)
                {
                    this.dgvFunctionPorcent.Rows.Add(new string[]
                    {
                    rule.Active.ToString().Trim(),
                    rule.PorcentMinHealth.ToString().Trim(),
                    rule.PorcentMaxHealth.ToString().Trim(),
                    rule.PorcentMinMana.ToString().Trim(),
                    rule.PorcentMaxMana.ToString().Trim(),
                    rule.Hotkey.ToString().Trim(),
                    rule.Spell.ToString().Trim(),
                    rule.Hotkey01.ToString().Trim(),
                    rule.Hotkey02.ToString().Trim(),
                    rule.AusentHealth.ToString().Trim(),
                    rule.AusentMana.ToString().Trim(),
                    rule.TimerInterval.ToString().Trim(),
                    rule.Prioridade.ToString().Trim(),
                    rule.Id.ToString().Trim(),
                    rule.EnabledHealth.ToString().Trim(),
                    rule.EnabledMana.ToString().Trim()
                    });
                });

                this.dgvFunctionPorcent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void StopFunctionPorcentById(int idThread)
        {
            foreach (DataGridViewRow loop2 in this.dgvFunctionPorcent.Rows)
            {
                if (idThread == int.Parse(loop2.Cells[13].Value.ToString()))
                {
                    try
                    {
                        loop2.Cells[0].Value = false;
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }

            try
            {
                HandlerFunction.RulesPorcent.Where(w => w.Id == idThread).FirstOrDefault((f => f.Active = false));
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }

            foreach (var loop2 in HandlerFunction.threadListPorcent)
            {
                if (loop2.Name.Equals(("IDPOR" + idThread)))
                {
                    try
                    {
                        loop2.Interrupt();
                        loop2.Abort();
                        BotLogs.add(null, new Exception("STOP THREAD: " + loop2.Name));
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, loop2.Name);
                    }
                    finally
                    {
                        try
                        {
                            HandlerFunction.threadListPorcent.Remove(loop2);
                        }
                        catch (Exception ex)
                        {
                            BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                        }
                    }
                    break;
                }
            }
        }

        private void DeleteFunctionPorcent(DataGridViewRow dataGridViewRow)
        {
            var idFunctionPorcent = int.Parse(dataGridViewRow.Cells[13].Value.ToString());

            #region REMOVE FROM THREAD
            foreach (var loop in HandlerFunction.threadListPorcent)
            {
                if (loop.Name.Equals(("IDPOR" + idFunctionPorcent.ToString())))
                {
                    try
                    {
                        loop.Interrupt();
                        loop.Abort();
                        BotLogs.add(null, new Exception("REMOVE THREAD: " + loop.Name));
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, loop.Name);
                    }
                    finally
                    {
                        try
                        {
                            HandlerFunction.threadListPorcent.Remove(loop);
                        }
                        catch (Exception ex)
                        {
                            BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                        }
                    }
                    break;
                }
            }

            #endregion

            #region REMOVE FROM RulesPorcent

            foreach (var loop in HandlerFunction.RulesPorcent)
            {
                if (loop.Id == idFunctionPorcent)
                {
                    try
                    {
                        HandlerFunction.RulesPorcent.Remove(loop);
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }

            #endregion

            #region REMOVE FROM ControlePrioridadeListPorcent

            foreach (var loop in HandlerFunction.ControlePrioridadeListPorcent)
            {
                if (loop.Id == idFunctionPorcent)
                {
                    try
                    {
                        HandlerFunction.ControlePrioridadeListPorcent.Remove(loop);
                        break;
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }

            #endregion

            this.dgvFunctionPorcent.Rows.Remove(dataGridViewRow);
        }

        private void LimparCamposPorcent()
        {
            this.rbHotkeyPorcent.Checked = true;
            this.tbSpellPorcent.Text = string.Empty;
            this.cbHotkey01Porcent.Text = "Empty";
            this.cbHotkey02Porcent.Text = "F1";
            this.rbAusentePorcentHealth.Checked = true;
            this.rbPresentePorcentMana.Checked = true;
            this.nudIntervaloPorcent.Value = 500;
            this.nudPrioridadePorcent.Value = 0;
            this.nudPorcentMinHealth.Value = 0;
            this.nudPorcentMaxHealth.Value = 100;
            this.nudPorcentMinMana.Value = 0;
            this.nudPorcentMaxMana.Value = 100;
            this.cbEnableHealthPorcent.Checked = true;
            this.cbEnableManaPorcent.Checked = false;
        }

        private void DeleteAllFunctionPorcent()
        {
            foreach (var loop in HandlerFunction.threadListPorcent)
            {
                try
                {
                    loop.Interrupt();
                    loop.Abort();
                    BotLogs.add(null, new Exception("REMOVE THREAD: " + loop.Name));
                }
                catch (Exception ex)
                {
                    BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, loop.Name);
                }
            }
            HandlerFunction.threadListPorcent = new List<System.Threading.Thread>();

            HandlerFunction.RulesPorcent = new List<FunctionRulePorcent>();

            HandlerFunction.ControlePrioridadeListPorcent = new List<ControlePrioridade>();

            this.dgvFunctionPorcent.Rows.Clear();

        }

        #endregion

        #region AUTO CLICK

        #region LOOT

        KeyboardHook.VKeys HotkeyLoot01 = new KeyboardHook.VKeys();
        KeyboardHook.VKeys HotkeyLoot02 = new KeyboardHook.VKeys();
        private bool HotkeyLootControlShift = false;
        private bool AutoLootControl = false;
        private bool AutoLootShift = false;

        public static Point PosLoot = new Point();
        public static Point PosPlayer = new Point();

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private void MouseClickLoot()
        {
            if (this.cbEnabledAutoLoot.Checked)
            {
                if (PosPlayer != new Point() && PosLoot != new Point())
                {
                    try
                    {
                        if (TibiaFunction.TibiaPrimeiraJanela())
                        {
                            var PosPlayerX = PosPlayer.X;
                            var PosPlayerY = PosPlayer.Y;
                            var diferenca = PosLoot.X - PosPlayer.X;
                            var posInicial = Cursor.Position;
                            int intervalo = Convert.ToInt32(nudIntervaloAutoLoot.Value);

                            MouseClickLootPosicao(intervalo, PosPlayerX, PosPlayerY); //Centro
                            MouseClickLootPosicao(intervalo, PosPlayerX + diferenca, PosPlayerY);//Direita
                            MouseClickLootPosicao(intervalo, PosPlayerX + diferenca, PosPlayerY - diferenca);//Direita Cima
                            MouseClickLootPosicao(intervalo, PosPlayerX, PosPlayerY - diferenca); //Cima
                            MouseClickLootPosicao(intervalo, PosPlayerX - diferenca, PosPlayerY - diferenca);//Esquerda Cima
                            MouseClickLootPosicao(intervalo, PosPlayerX - diferenca, PosPlayerY);//Esquerda
                            MouseClickLootPosicao(intervalo, PosPlayerX - diferenca, PosPlayerY + diferenca);//Esquerda Baixo
                            MouseClickLootPosicao(intervalo, PosPlayerX, PosPlayerY + diferenca); //Baixo
                            MouseClickLootPosicao(intervalo, PosPlayerX + diferenca, PosPlayerY + diferenca);//Direita Baixo
                            SetCursorPos(posInicial.X, posInicial.Y); //PosInicial
                        }
                    }
                    catch (Exception ex)
                    {
                        BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                    }
                }
            }
        }

        private static void MouseClickLootPosicao(int intervalo, int posX, int posY)
        {
            try
            {
                SetCursorPos(posX, posY);
                System.Threading.Thread.Sleep(10);
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                System.Threading.Thread.Sleep(intervalo);
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void cbEnabledAutoLoot_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbEnabledAutoLoot.Checked)
            {
                if (HotkeyLoot02 == new KeyboardHook.VKeys())
                {
                    this.cbEnabledAutoLoot.Checked = false;
                    FormMessageBox.Show("Choose a Hotkey before active this function.", CultureLanguage.ChangeLanguageName("Problems"));
                }
                else
                {
                    keyboardHook.Install();
                    this.cbEnabledAutoLoot.Text = "Enabled";
                }
            }
            else
            {
                keyboardHook.Uninstall();
                this.cbEnabledAutoLoot.Text = "Disabled";
            }
        }

        private void btnHotkeyAutoLoot_Click(object sender, EventArgs e)
        {
            using (var form = new FormGetHotkey("AUTO LOOT"))
            {
                HotkeyLoot01 = new KeyboardHook.VKeys();
                HotkeyLoot02 = new KeyboardHook.VKeys();

                var result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (form.ReturnKey02 != new KeyboardHook.VKeys())
                    {
                        this.labelHotkeyLoot.Text = "HOTKEY: (";
                        if (form.ReturnKey01 != new KeyboardHook.VKeys())
                        {
                            HotkeyLoot01 = form.ReturnKey01;
                            this.labelHotkeyLoot.Text += form.ReturnKey01.ToString() + " + ";
                        }

                        HotkeyLoot02 = form.ReturnKey02;
                        this.labelHotkeyLoot.Text += form.ReturnKey02.ToString() + ")";
                        return;
                    }
                }
                this.labelHotkeyLoot.Text = "HOTKEY:";
                this.cbEnabledAutoLoot.Checked = false;
            }
        }

        private void CapturarKeyDown(KeyboardHook.VKeys key)
        {

            if (key.Equals(KeyboardHook.VKeys.RCONTROL) || key.Equals(KeyboardHook.VKeys.LCONTROL))
            {
                AutoLootControl = true;
            }
            else if (key.Equals(KeyboardHook.VKeys.RSHIFT) || key.Equals(KeyboardHook.VKeys.LSHIFT))
            {
                AutoLootShift = true;
            }

            if ((AutoLootShift || AutoLootControl) && HotkeyLoot01 == new KeyboardHook.VKeys())
            {
                HotkeyLootControlShift = false;
            }
            else if (HotkeyLoot01 == new KeyboardHook.VKeys())
            {
                HotkeyLootControlShift = true;
            }


            if (key.Equals(HotkeyLoot01))
            {
                HotkeyLootControlShift = true;
            }
            else if (key.Equals(HotkeyLoot02))
            {
                if (HotkeyLootControlShift)
                {
                    MouseClickLoot();
                }
            }
        }

        private void CapturarKeyUp(KeyboardHook.VKeys key)
        {
            if (key.Equals(KeyboardHook.VKeys.RCONTROL) || key.Equals(KeyboardHook.VKeys.LCONTROL))
            {
                AutoLootControl = false;
            }
            else if (key.Equals(KeyboardHook.VKeys.RSHIFT) || key.Equals(KeyboardHook.VKeys.LSHIFT))
            {
                AutoLootShift = false;
            }

            if (key.Equals(HotkeyLoot01))
            {
                HotkeyLootControlShift = false;
            }
        }
        #endregion

        #endregion

    }
}
