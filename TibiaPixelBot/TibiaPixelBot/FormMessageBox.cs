using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public partial class FormMessageBox : MaterialForm
    {
        MaterialLabel textLabel = null;

        private static int QtdFormMessageBoxAbertas = 0;

        public FormMessageBox(string body, string header)
        {
            InitializeComponent();

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.DARK;
            skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            this.Text = header;

            CreateLabel(body);
        }

        public FormMessageBox(string body)
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

            this.Text = string.Empty;

            CreateLabel(body);

            QtdFormMessageBoxAbertas++;
        }

        public static void Show(string body, string header)
        {
            try
            {
                FormMessageBox form = new FormMessageBox(body, header);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        public static void Show(string body)
        {
            try
            {
                FormMessageBox form = new FormMessageBox(body);
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

        private void CreateLabel(string body)
        {
            textLabel = new MaterialLabel()
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.None,
                Left = 10,
                Width = this.Width - 10,
                Location = new Point(5, 93),
                Text = body
            };
            this.Controls.Add(textLabel);
        }

        private void FormMessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Controls.Remove(textLabel);
            QtdFormMessageBoxAbertas--;
        }

        private void FormMessageBox_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Location.X + (10 * QtdFormMessageBoxAbertas), this.Location.Y + (10 * QtdFormMessageBoxAbertas));
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            QtdFormMessageBoxAbertas++;
        }

        private void FormMessageBox_Move(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
