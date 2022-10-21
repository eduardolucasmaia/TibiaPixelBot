using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TibiaPixelBot.DataModels;
using TibiaPixelBot.Email;
using TibiaPixelBot.Models;

namespace TibiaPixelBot.Controllers
{
    public class HomeController : Controller
    {

        #region Construtor

        private IEmailSender _emailSender;

        public HomeController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        #endregion

        public async Task<IActionResult> Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ViewData["divFocus"] != null)
            {
                ViewData["divFocus"] = null;
            }
            try
            {
                ParametroSistema parametroSistema;
                using (var db = new TibiaDBContext())
                {
                    parametroSistema = await db.ParametroSistema.Where(x => x.Nome.Equals("tempoTrial")).FirstOrDefaultAsync();
                }
                ViewData["tempoTrial"] = parametroSistema.Valor;
            }
            catch (Exception)
            {
                ViewData["tempoTrial"] = "5";
            }


            var model = new UsuarioViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var claimsList = (ClaimsIdentity)User.Identity;
                if (claimsList != null)
                {
                    if (claimsList.Claims.Count() > 0)
                    {
                        model.Name = claimsList.Claims.Where(x => x.Type.Equals(ClaimTypes.Name)).FirstOrDefault().Value;
                        model.Email = claimsList.Claims.Where(x => x.Type.Equals(ClaimTypes.Email)).FirstOrDefault().Value;
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Index(UsuarioViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    StringBuilder body = new StringBuilder();
                    body.Append("Name: " + model.Name);
                    body.Append(Environment.NewLine);
                    body.Append("Email: " + model.Email);
                    body.Append(Environment.NewLine);
                    body.Append(model.Message);
                    await _emailSender.EnviarEmailAsync("Site - Contact Me", body.ToString(), "tibiapixelbot@hotmail.com");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    //Salvar log aqui.
                    ViewData["divFocus"] = "contact";
                    ModelState.AddModelError(string.Empty, "An internal system error has occurred. Please try again later.");
                }
            }
            else
            {
                ViewData["divFocus"] = "contact";
            }

            return View(model);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
