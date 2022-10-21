using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public partial class FormSettings : MaterialForm
    {
        private Control.ControlCollection formPrincipal = null;

        private void Settings_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            CultureLanguage.ChangeLanguageForm<FormSettings>(this);
            this.cbChangeLanguage.Text = Properties.Settings.Default.SelectedLanguage;
            this.cbCloseToTray.Checked = Properties.Settings.Default.CloseToTray;
            this.cbMinimizeToTray.Checked = Properties.Settings.Default.MinimizeToTray;
            this.nudRule.Value = Properties.Settings.Default.RulesAttempts;
            this.nudAutoScan.Value = Properties.Settings.Default.AutoScanAttempts;
        }

        public FormSettings(Control.ControlCollection formPrincipal)
        {
            this.formPrincipal = formPrincipal;
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

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                var SelectedLanguage = "";
                var languageProp = "";
                var execute = false;

                if (cbChangeLanguage.SelectedItem.ToString() == "English")
                {
                    execute = true;
                    languageProp = "en";
                    SelectedLanguage = "English";
                }
                else if (cbChangeLanguage.SelectedItem.ToString() == "Español")
                {
                    execute = true;
                    languageProp = "es-ES";
                    SelectedLanguage = "Español";
                }
                else if (cbChangeLanguage.SelectedItem.ToString() == "Português")
                {
                    execute = true;
                    languageProp = "pt-BR";
                    SelectedLanguage = "Português";
                }
                else if (cbChangeLanguage.SelectedItem.ToString() == "Français")
                {
                    execute = true;
                    languageProp = "fr-FR";
                    SelectedLanguage = "Français";
                }

                if (execute)
                {
                    Properties.Settings.Default.SelectedLanguage = SelectedLanguage;
                    Properties.Settings.Default.LanguageProp = languageProp;
                    Properties.Settings.Default.Save();

                    CultureLanguage.ChangeLanguageForm<FormSettings>(this);
                    CultureLanguage.ChangeLanguageControlCollection<FormPrincipal>(formPrincipal);

                    this.cbChangeLanguage.Text = SelectedLanguage;

                    FormPrincipal.AplicandoLinguagem = true;
                }

                Properties.Settings.Default.MinimizeToTray = this.cbMinimizeToTray.Checked;
                Properties.Settings.Default.CloseToTray = this.cbCloseToTray.Checked;
                Properties.Settings.Default.AutoScanAttempts = Convert.ToInt32(this.nudAutoScan.Value);
                Properties.Settings.Default.RulesAttempts = Convert.ToInt32(this.nudRule.Value);
                Properties.Settings.Default.Save();

                this.Close();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void FormSettings_Move(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
