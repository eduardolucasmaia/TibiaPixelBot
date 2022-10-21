using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public partial class FormGetHotkey : MaterialForm
    {

        public KeyboardHook.VKeys ReturnKey01 { get; set; }
        public KeyboardHook.VKeys ReturnKey01Temp { get; set; }
        public KeyboardHook.VKeys ReturnKey02 { get; set; }

        KeyboardHook keyboardHook = new KeyboardHook();

        private bool AutoLootControl = false;
        private bool AutoLootShift = false;

        private static int QtdFormGetKotkeyAbertas = 0;

        public FormGetHotkey(string body)
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

            this.Text = "EDIT HOTKEY FOR " + body;

            QtdFormGetKotkeyAbertas++;

            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(CapturarKeyDown);
            keyboardHook.KeyUp += new KeyboardHook.KeyboardHookCallback(CapturarKeyUp);

            keyboardHook.Install();

        }

        public static void Show(string body)
        {
            try
            {
                FormGetHotkey form = new FormGetHotkey(body);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormGetKotkey_FormClosing(object sender, FormClosingEventArgs e)
        {
            QtdFormGetKotkeyAbertas--;
            keyboardHook.Uninstall();
        }

        private void FormGetKotkey_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Location.X + (10 * QtdFormGetKotkeyAbertas), this.Location.Y + (10 * QtdFormGetKotkeyAbertas));
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            QtdFormGetKotkeyAbertas++;
        }

        private void FormGetKotkey_Move(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
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
        }

        private void CapturarKeyDown(KeyboardHook.VKeys key)
        {
            labelHotkey.Text = string.Empty;
            ReturnKey01 = new KeyboardHook.VKeys();
            ReturnKey02 = new KeyboardHook.VKeys();

            if (key.Equals(KeyboardHook.VKeys.RCONTROL) || key.Equals(KeyboardHook.VKeys.LCONTROL))
            {
                ReturnKey01Temp = key;
                AutoLootControl = true;
            }
            else if (key.Equals(KeyboardHook.VKeys.RSHIFT) || key.Equals(KeyboardHook.VKeys.LSHIFT))
            {
                ReturnKey01Temp = key;
                AutoLootShift = true;
            }
            else
            {
                if (AutoLootShift)
                {
                    labelHotkey.Text = ReturnKey01Temp.ToString() + " + ";
                    ReturnKey01 = ReturnKey01Temp;
                }
                else if (AutoLootControl)
                {
                    labelHotkey.Text = ReturnKey01Temp.ToString() + " + ";
                    ReturnKey01 = ReturnKey01Temp;
                }

                if (!key.Equals(KeyboardHook.VKeys.RCONTROL) && !key.Equals(KeyboardHook.VKeys.LCONTROL) && !key.Equals(KeyboardHook.VKeys.RSHIFT) && !key.Equals(KeyboardHook.VKeys.LSHIFT))
                {
                    labelHotkey.Text += key;
                    ReturnKey02 = key;
                }

                this.labelHotkey.AutoSize = false;
                this.labelHotkey.TextAlign = ContentAlignment.MiddleCenter;
                this.labelHotkey.Dock = DockStyle.None;
                this.labelHotkey.Left = 10;
                this.labelHotkey.Width = this.Width - 10;
                this.labelHotkey.Location = new Point(5, 93);
            }
        }

        private void FormGetHotkey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                e.Handled = true;
        }
    }
}
