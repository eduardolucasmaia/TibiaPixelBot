using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TibiaPixelBot
{
    public static class XmlConvert
    {
        public static bool SerializeObject<T>(T dataObject)
        {
            if (dataObject == null)
            {
                return false;
            }
            try
            {
                using (StringWriter stringWriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stringWriter, dataObject);

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = CultureLanguage.ChangeLanguageName("TPBFiles") + "|*.tpb";
                    saveFileDialog1.Title = CultureLanguage.ChangeLanguageName("SaveTPB");
                    saveFileDialog1.FileName = Environment.UserName + " - " + DateTime.Now.ToString("dd.MM.yyyy");
                    saveFileDialog1.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog1.FileName.Trim()))
                    {
                        try
                        {
                            FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
                            fs.Close();
                            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, true, Encoding.ASCII);
                            sw.Write(stringWriter.ToString());
                            sw.Close();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                            FormMessageBox.Show("Erro: O arquivo já está sendo utilizado por outro processo.");
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return false;
            }
        }

        public static T DeserializeObject<T>(string xml)
             where T : new()
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new T();
            }
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                BotLogs.SaveLog(ex, new StackTrace().GetFrame(0).GetMethod().Name, "");
                return new T();
            }
        }
    }
}
