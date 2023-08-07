using EasyCashIdentityProject.EntityLayer.Concrete;
using EasyCashIdentityProject.PresentationLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
	public class ConfirmMailController : Controller
	{
        private readonly UserManager<AppUser> _userManager;

        //ctrl + . generate controller
        public ConfirmMailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var value = TempData["Mail"];
            TempData.Keep("Mail"); // Burada TempData'deki "Mail" değerini koruyoruz
            ViewBag.v = value;
    
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailViewModel confirmMailViewModel)
        {
            var value = TempData["Mail"];
            TempData.Keep("Mail"); // Burada TempData'daki "Mail" değerini koruyoruz
            var user = await _userManager.FindByEmailAsync(value.ToString());
            if (user.ConfirmCode == confirmMailViewModel.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

    }
}
