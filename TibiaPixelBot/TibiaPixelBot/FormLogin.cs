using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public partial class FormLogin : MaterialForm
    {
        string fileName = string.Empty;

        public FormLogin(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                fileName = file;
            }

            InicializarComponentes();
        }

        public FormLogin()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            InitializeComponent();
            try
            {
                var skinManager = MaterialSkinManager.Instance;
                skinManager.AddFormToManage(this);
                skinManager.Theme = MaterialSkinManager.Themes.DARK;
                skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    string result = string.Join("", File.ReadAllLines(fileName));
                    var objectSerialize = XmlConvert.DeserializeObject<ObjectSerialize>(result);
                    if (objectSerialize != null)
                    {
                        if (!string.IsNullOrEmpty(objectSerialize.Email) && !string.IsNullOrEmpty(objectSerialize.Password))
                        {
                            if (LogarNoSistema(objectSerialize.Email, objectSerialize.Password))
                            {
                                this.Activated += AfterLoading;
                                return;
                            }

                        }
                    }
                }

                this.MaximumSize = this.Size;
                this.MinimumSize = this.Size;

                this.cbRememberMe.Checked = Properties.Settings.Default.RememberMe;
                if (Properties.Settings.Default.RememberMe)
                {
                    this.tbPassword.Text = Properties.Settings.Default.PasswordUser;
                    this.tbEmail.Text = Properties.Settings.Default.EmailUser;
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void AfterLoading(object sender, EventArgs e)
        {
            this.Activated -= AfterLoading;
            this.Hide();
        }

        private void labelLostPassword_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.google.com/");
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void labelRegister_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.google.com/");
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbPassword.Text.Trim()) && !string.IsNullOrEmpty(this.tbEmail.Text.Trim()))
            {
                LogarNoSistema(this.tbEmail.Text.Trim(), this.tbPassword.Text.Trim());
            }
            else
            {
                FormMessageBox.Show("Insert your login data!");
            }
        }

        private bool LogarNoSistema(string email, string password)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string result = webClient.DownloadString(string.Concat(new string[]
                    {
                                    "http://localhost:53099/api/Account/VerificarLogin/",
                                    email,
                                    "/",
                                   password,
                                    "/",
                                    TibiaFunction.Version,
                                    "/",
                                    TibiaFunction.GetHDDSerialNo()
                    }));


                    result = result.Replace("\\", "");
                    result = result.Replace("\"{", "{");
                    result = result.Replace("}\"", "} ");

                    var objectSerialize = JsonConvert.DeserializeObject<ObjetoRetornoAplicativo>(result);

                    //if (result.Equals("this_computer_used_trial"))
                    //{
                    //    FormMessageBox.Show("You have used a free trial, Please buy a subscription.", CultureLanguage.ChangeLanguageName("Problems"));
                    //    Process.Start("http://tibiaeyebot.com/purchase/");
                    //}
                    //else if (result.Equals("update_to_new_version"))
                    //{
                    //    FormMessageBox.Show("There is a new version of this bot, Please update it!.", CultureLanguage.ChangeLanguageName("Problems"));
                    //    Process.Start("http://tibiaeyebot.com/download/");
                    //}
                    //else if (result.Equals("error"))
                    //{
                    //    FormMessageBox.Show("An error has been occurred, please try again.", CultureLanguage.ChangeLanguageName("Problems"));
                    //}
                    //else if (result.Equals("is_not_member"))
                    //{
                    //    FormMessageBox.Show("Please buy a subscription time.", CultureLanguage.ChangeLanguageName("Problems"));
                    //}
                    //else if (result.Equals("expired_member"))
                    //{
                    //    FormMessageBox.Show("Your subscription time has been expired, Please renew it.", CultureLanguage.ChangeLanguageName("Problems"));
                    //}
                    //else if (result.Equals("user_input_invalid"))
                    //{
                    //    FormMessageBox.Show("Your email and password are incorrect.", CultureLanguage.ChangeLanguageName("Problems"));
                    //}
                    //else if (result.Equals("ok"))

                    if (objectSerialize.Valido)
                    {
                        if (!string.IsNullOrEmpty(objectSerialize.Mensagem))
                        {
                            FormMessageBox.Show(objectSerialize.Mensagem);
                        }

                        if (objectSerialize.Mensagens != null)
                        {
                            foreach (var loop in objectSerialize.Mensagens)
                            {
                                FormMessageBox.Show(loop);
                            }
                        }

                        Properties.Settings.Default.PasswordUser = password;
                        Properties.Settings.Default.EmailUser = email;
                        Properties.Settings.Default.Save();

                        TibiaFunction.Email = email;
                        TibiaFunction.Password = password;

                        FormPrincipal formPrincipal = null;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            formPrincipal = new FormPrincipal(this, fileName);
                        }
                        else
                        {
                            formPrincipal = new FormPrincipal(this);
                        }
                        formPrincipal.Show();
                        formPrincipal.Focus();
                        base.Hide();
                        return true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(objectSerialize.Mensagem))
                        {
                            FormMessageBox.Show(objectSerialize.Mensagem);
                        }
                        else
                        {
                            FormMessageBox.Show("An error has been occurred, please try again.", CultureLanguage.ChangeLanguageName("Problems"));
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                FormMessageBox.Show("An error has been occurred, please try again.", CultureLanguage.ChangeLanguageName("Problems"));
                return false;
            }
        }

        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.RememberMe = this.cbRememberMe.Checked;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void FormLogin_Move(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
