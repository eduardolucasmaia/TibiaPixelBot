using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using TibiaPixelBot.DataModels;
using TibiaPixelBot.Models;

namespace TibiaPixelBot.Controllers.api
{
    [Produces("application/json")]
    [Route("api/Account/[action]")]
    public class AccountController : Controller
    {

        Fazer batch que passar os status logado para false


        //http://localhost:53099/api/Account/VerificarLogin/email/password/version/serial
        [HttpGet("{email}/{password}/{version}/{serial}")]
        [ActionName("VerificarLogin")]
        public async Task<string> VerificarLogin(string email, string password, string version, string serial)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(serial))
                {
                    Usuario usuario = null;

                    using (var db = new TibiaDBContext())
                    {

                        var tempoTrial = await db.ParametroSistema.Where(x => x.Nome.Equals("tempoTrial")).FirstOrDefaultAsync();

                        usuario = await db.Usuario.Where(x => x.Email.Equals(email.Trim()) && x.Senha.Equals(password.Trim()) && !x.Excluido).FirstOrDefaultAsync();

                        if (usuario == null)
                        {
                            return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                            {
                                Mensagem = "The e-mail you entered does not match any accounts. Sign up to open an account.",
                                Valido = false,
                                Mensagens = null
                            });
                        }
                        else
                        {
                            if (usuario.Logado)
                            {
                                if (!usuario.CodigoMaquina.Equals(serial))
                                {
                                    return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                                    {
                                        Mensagem = "Only one computer per account is allowed.",
                                        Valido = false,
                                        Mensagens = null
                                    });
                                }
                            }

                            if (usuario.DataExpirar == null)
                            {
                                usuario.DataExpirar = DateTime.Now.AddDays(int.Parse(tempoTrial.Valor));
                                usuario.UsouFreeTrial = false;
                            }
                            else
                            {
                                if ((DateTime.Now - usuario.DataExpirar.Value).TotalDays >= int.Parse(tempoTrial.Valor))
                                {
                                    usuario.UsouFreeTrial = true;
                                }
                            }

                            usuario.Logado = true;
                            usuario.DataUltimoAcesso = DateTime.Now;
                            usuario.CodigoMaquina = serial;
                            db.Usuario.Update(usuario);
                            await db.SaveChangesAsync();
                        }
                    }

                    var diasRestantes = Math.Round((usuario.DataExpirar.Value - DateTime.Now).TotalDays);
                    var mensagem = string.Empty;
                    var valido = false;

                    if (diasRestantes > 0)
                    {
                        mensagem = "You still have " + diasRestantes.ToString() + " days to use.";
                        valido = true;
                    }
                    else
                    {
                        mensagem = "Buy the PRO version to continue using.";
                        valido = false;
                    }

                    return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                    {
                        Valido = valido,
                        Mensagem = mensagem,
                        Mensagens = null
                    });
                }

                return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                {
                    Mensagem = "Insert your login data!",
                    Valido = false,
                    Mensagens = null
                });
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                {
                    Mensagem = "An internal system error has occurred. Please try again later.",
                    Valido = false,
                    Mensagens = null
                });
            }
        }

        //http://localhost:53099/api/Account/VerificarLogin/email/password/version/serial
        [HttpGet("{email}/{password}/{serial}")]
        [ActionName("KeepAlive")]
        public async Task<string> KeepAlive(string email, string password, string serial)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(serial))
                {
                    Usuario usuario = null;

                    using (var db = new TibiaDBContext())
                    {
                        usuario = await db.Usuario.Where(x => x.Email.Equals(email.Trim()) && x.Senha.Equals(password.Trim()) && !x.Excluido).FirstOrDefaultAsync();

                        if (usuario == null)
                        {
                            return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                            {
                                Mensagem = "The e-mail you entered does not match any accounts. Sign up to open an account.",
                                Valido = false,
                                Mensagens = null
                            });
                        }
                        else
                        {
                            if (usuario.Logado)
                            {
                                if (usuario.CodigoMaquina.Equals(serial))
                                {
                                    return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                                    {
                                        Mensagem = string.Empty,
                                        Valido = true,
                                        Mensagens = null
                                    });
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                                    {
                                        Mensagem = "Only one computer per account is allowed.",
                                        Valido = false,
                                        Mensagens = null
                                    });
                                }
                            }

                            usuario.Logado = true;
                            usuario.DataUltimoAcesso = DateTime.Now;
                            usuario.CodigoMaquina = serial;
                            db.Usuario.Update(usuario);
                            await db.SaveChangesAsync();

                            return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                            {
                                Mensagem = string.Empty,
                                Valido = true,
                                Mensagens = null
                            });
                        }
                    }
                }

                throw new Exception();
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new ObjetoRetornoAplicativo()
                {
                    Mensagem = "An internal system error has occurred. Please try again later.",
                    Valido = false,
                    Mensagens = null
                });
            }
        }

        //http://localhost:53099/api/Account/VerificarLogin/email/password/version/serial
        [HttpGet("{email}/{password}")]
        [ActionName("LogOut")]
        public async Task LogOut(string email, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    Usuario usuario = null;

                    using (var db = new TibiaDBContext())
                    {
                        usuario = await db.Usuario.Where(x => x.Email.Equals(email.Trim()) && x.Senha.Equals(password.Trim()) && !x.Excluido).FirstOrDefaultAsync();

                        if (usuario != null)
                        {
                            usuario.Logado = false;
                            db.Usuario.Update(usuario);
                            await db.SaveChangesAsync();
                        }
                    }
                }

            }
            catch (Exception) { }
        }
    }
}