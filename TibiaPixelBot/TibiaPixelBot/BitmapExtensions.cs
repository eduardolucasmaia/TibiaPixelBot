using AForge.Imaging;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TibiaPixelBot
{
    public static class BitmapExtensions
    {

        public static Bitmap bitmapGeral = null;

        public static Bitmap CloneBitmap(Bitmap bitmap)
        {
            try
            {
                Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
                using (Graphics graphics = Graphics.FromImage(bitmap2))
                {
                    graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
                }
                return bitmap2;
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return null;
            }
        }

        public static void getToolAreaNew()
        {
            try
            {
                var temPosX = 0;
                var temPosY = 0;

                ////Unica Tela ou esta na tela Primaria
                //if (FormPrincipal.PosXAction > 0 && FormPrincipal.PosXAction < Screen.PrimaryScreen.Bounds.Width &&
                //    FormPrincipal.PosYAction > 0 && FormPrincipal.PosYAction < Screen.PrimaryScreen.Bounds.Height)
                //{
                //    temPosX = Screen.PrimaryScreen.Bounds.Width;
                //    temPosY = Screen.PrimaryScreen.Bounds.Height;
                //}
                //else
                //{
                if (Screen.AllScreens.Length > 1)
                {
                    Screen[] screens;
                    screens = Screen.AllScreens;
                    for (int i = 0; i < screens.Length; i++)
                    {
                        if (temPosX < (screens[i].Bounds.X + screens[i].Bounds.Width))
                        {
                            temPosX = screens[i].Bounds.X + screens[i].Bounds.Width;
                        }
                        if (temPosY < (screens[i].Bounds.Y + screens[i].Bounds.Height))
                        {
                            temPosY = screens[i].Bounds.Y + screens[i].Bounds.Height;
                        }
                    }
                }
                else
                {
                    temPosX = Screen.PrimaryScreen.Bounds.Width;
                    temPosY = Screen.PrimaryScreen.Bounds.Height;
                }
                // }

                bitmapGeral = new Bitmap(temPosX, temPosY);
                Graphics gaaphics = Graphics.FromImage(bitmapGeral);
                gaaphics.CopyFromScreen(0, 0, 0, 0, bitmapGeral.Size);

            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
            }
        }

        public static Bitmap RetornaQuadranteImagem(int posX, int posY, int limX, int LimY)
        {
            try
            {
                Bitmap image = bitmapGeral.Clone(new Rectangle(posX, posY, limX, LimY), PixelFormat.Format24bppRgb);
                // FormPrincipal.bitmapCaputrado = image;
                return image;
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return null;
            }
        }

        public static bool VerificarAcaoTela(Bitmap sourceImage, Bitmap template)
        {
            try
            {
                var achou = false;
                // 0.8f == 80% || 1f == 100% de igualdade  
                ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.95f);

                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template);

                //BitmapData data = sourceImage.LockBits(
                //     new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                //     ImageLockMode.ReadWrite, sourceImage.PixelFormat);
                foreach (TemplateMatch m in matchings)
                {
                    //Drawing.Rectangle(data, m.Rectangle, Color.White);
                    BotLogs.add(null, new Exception(m.Rectangle.Location.ToString()));
                    achou = true;
                    break;
                }

                //sourceImage.UnlockBits(data);

                return achou;
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return false;
            }
        }

        public static Point VerificarPosicaoCoracaoVida(Bitmap template)
        {
            try
            {
                if (bitmapGeral != null)
                {
                    Bitmap sourceImage = null;

                    if (FormPrincipal.PointHeart != new Point())
                    {
                        sourceImage = RetornaQuadranteImagem(FormPrincipal.PointHeart.X, FormPrincipal.PointHeart.Y, 9, 9);
                    }
                    else
                    {
                        sourceImage = bitmapGeral;
                    }

                    sourceImage = BitmapExtensions.CloneBitmap(sourceImage);
                    Bitmap imageCompare = BitmapExtensions.CloneBitmap(template);

                    // 0.8f == 80% || 1f == 100% de igualdade  
                    ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.95f);

                    TemplateMatch[] matchings = tm.ProcessImage(sourceImage, imageCompare);

                    //BitmapData data = sourceImage.LockBits(
                    //     new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                    //     ImageLockMode.ReadWrite, sourceImage.PixelFormat);
                    foreach (TemplateMatch m in matchings)
                    {
                        // Drawing.Rectangle(data, m.Rectangle, Color.White);
                        BotLogs.add(null, new Exception(m.Rectangle.Location.ToString()));
                        if (m.Rectangle.X == 0 && m.Rectangle.Y == 0)
                        {
                            return FormPrincipal.PointHeart;
                        }
                        return new Point(m.Rectangle.X, m.Rectangle.Y);
                    }
                    // sourceImage.UnlockBits(data);
                }

                return new Point();
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return new Point();
            }
        }


    }
}
