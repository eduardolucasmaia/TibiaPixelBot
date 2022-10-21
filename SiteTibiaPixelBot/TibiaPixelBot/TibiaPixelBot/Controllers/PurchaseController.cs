using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TibiaPixelBot.DataModels;
using TibiaPixelBot.Models;

namespace TibiaPixelBot.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                ParametroSistema parametroSistema;
                using (var db = new TibiaDBContext())
                {
                    parametroSistema = await db.ParametroSistema.Where(x => x.Nome.Equals("valorProduto")).FirstOrDefaultAsync();
                    ViewData["valorProduto"] = parametroSistema.Valor;
                    parametroSistema = await db.ParametroSistema.Where(x => x.Nome.Equals("tempoTrial")).FirstOrDefaultAsync();
                    ViewData["tempoTrial"] = parametroSistema.Valor;
                }
            }
            catch (Exception)
            {
                ViewData["valorProduto"] = "1.99";
                ViewData["tempoTrial"] = "5";
            }

            return View();
        }

        public IActionResult Purchase()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Purchase(CartaoViewModel model)
        {
            return View();
            //  return RedirectToAction("Index", "Purchase");
        }
    }
}