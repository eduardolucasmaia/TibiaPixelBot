using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TibiaPixelBot.DataModels;
using TibiaPixelBot.Email;
using TibiaPixelBot.Models;

namespace TibiaPixelBot.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Construtor

        private IEmailSender _emailSender;

        public AccountController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        #endregion

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Title"] = "Login - ";
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                Usuario usuario = null;

                try
                {
                    using (var db = new TibiaDBContext())
                    {
                        usuario = await db.Usuario.Where(x => x.Email.Equals(model.Email.Trim()) && x.Senha.Equals(model.Password.Trim()) && !x.Excluido).FirstOrDefaultAsync();

                        if (usuario != null)
                        {
                            await Logar(usuario, model.RememberMe);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            usuario = await db.Usuario.Where(x => x.Email.Equals(model.Email.Trim())).FirstOrDefaultAsync();

                            if (usuario == null)
                            {
                                ModelState.AddModelError(string.Empty, "The e-mail you entered does not match any accounts. Sign up to open an account.");
                            }
                            //else if (!usuario.Valido)
                            //{
                            //    ModelState.AddModelError(string.Empty, "The account has not been validated. Enter the email sent to your account and access the validation link.");
                            //}
                            else if (usuario.Excluido)
                            {
                                ModelState.AddModelError(string.Empty, "Account has been deleted, please create another account.");
                            }
                        }

                        db.Dispose();
                    }
                }
                catch (Exception)
                {
                    //Salvar log aqui.
                    ModelState.AddModelError(string.Empty, "An internal system error has occurred. Please try again later.");
                }
            }

            return View(model);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateAccount(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Title"] = "´Create Account - ";
            var model = new CreateAccountViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new TibiaDBContext())
                    {
                        var usuario = await db.Usuario.Where(x => x.Email.Equals(model.Email.Trim())).FirstOrDefaultAsync();

                        if (usuario != null)
                        {
                            ModelState.AddModelError(string.Empty, "That username is taken. Try another.");
                        }
                        else
                        {
                            db.Usuario.Add(new Usuario()
                            {
                                Id = 0,
                                Name = model.Name.Trim(),
                                Email = model.Email.Trim(),
                                Senha = model.Password.Trim(),
                                Logado = false,
                                NomePlayerTibia = null,
                                CodigoMaquina = null,
                                ChaveRecuperacao = null,
                                DataUltimoAcesso = null,
                                DataExpirar = null,
                                DataCadastro = DateTime.Now,
                                Excluido = false,
                                UsouFreeTrial = false
                            });

                            await db.SaveChangesAsync();

                            // await EnviarEmailSerialUsuario(usuario);

                            // ViewData["SuccessMessage"] = "Account created successfully, Check your e-mail and activate your account.";
                            ViewData["SuccessMessage"] = "Account created successfully!";

                            await Logar(usuario, false);

                            return RedirectToAction("Index", "Home");
                        }
                        db.Dispose();
                    }
                }
                catch (Exception)
                {
                    //Salvar log aqui.
                    ModelState.AddModelError(string.Empty, "An internal system error has occurred. Please try again later.");
                }
            }

            return View(model);
        }

        private async Task EnviarEmailSerialUsuario(Usuario usuario)
        {
            try
            {
                StringBuilder body = new StringBuilder();
                body.Append("Name: ");
                body.Append(Environment.NewLine);

                await _emailSender.EnviarEmailAsync("", body.ToString(), usuario.Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    scheme: CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private async Task Logar(Usuario usuario, bool rememberMe)
        {
            try
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Name,ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, usuario.Email, ClaimValueTypes.String),
                new Claim(ClaimTypes.IsPersistent, rememberMe.ToString() , ClaimValueTypes.Boolean)
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = true,
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = rememberMe
                };

                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}