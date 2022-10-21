using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TibiaPixelBot.DataModels;

namespace TibiaPixelBot.Email
{
    public class EmailSender : IEmailSender
    {
        #region Construtor

        //private ILogErroBusiness _logErroBusiness;

        //public EmailSender(ILogErroBusiness logErroBusiness)
        //{
        //    _logErroBusiness = logErroBusiness;
        //}

        #endregion

        public async Task EnviarEmailAsync(string subject, string body, string to)
        {
            try
            {
                ConfiguracaoEmail email = null;

                using (var db = new TibiaDBContext())
                {
                    email = await db.ConfiguracaoEmail.Where(x => !x.Excluido).LastOrDefaultAsync();
                }

                if (email != null)
                {
                    var mailMessage = new MailMessage();

                    mailMessage.From = new MailAddress(email.Email);
                    mailMessage.To.Add(to.Trim());
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.EnableSsl = email.Ssl != null ? email.Ssl.Value : true;
                        smtpClient.Host = email.Host.Trim();
                        smtpClient.Port = email.Port.Value;

                        if (string.IsNullOrEmpty(email.Dominio))
                        {
                            smtpClient.Credentials = new NetworkCredential(email.Email.Trim(), email.Senha.Trim());
                        }
                        else
                        {
                            smtpClient.Credentials = new NetworkCredential(email.Email.Trim(), email.Senha.Trim(), email.Dominio.Trim());
                        }

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // _logErroBusiness.SalvarLogErro("", email, ex);
                throw ex;
            }
        }
    }
}
