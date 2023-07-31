using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
        { 
            if(ModelState.IsValid) 
            {
                AppUser appUser = new AppUser()
                {
                    UserName = appUserRegisterDto.Username,
                    Name = appUserRegisterDto.Name,
                    Surname = appUserRegisterDto.Surname,
                    Email = appUserRegisterDto.Email,
                };
                var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password); 
                //CreateAsync identity işlemini sağlar.
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index", "ConfirmMail");
                }
            }
            return View();
        }
    }
}


//şifre en az 6 karakter, 1 küçük harf, 1 büyük harf, 1 sembol, 1sayı içermeli.
