using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
    [Authorize]
    public class MyAccountsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public MyAccountsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            AppUserEditDto appUserEditDto = new AppUserEditDto();
            appUserEditDto.Name = values.Name;
            appUserEditDto.Surname = values.Surname;
            appUserEditDto.PhoneNumber = values.PhoneNumber;
            appUserEditDto.Email = values.Email;
            appUserEditDto.City = values.City;
            appUserEditDto.District = values.District;
            appUserEditDto.ImageUrl = values.ImageUrl;
            return View(appUserEditDto);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Index(AppUserEditDto appUserEditDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.PhoneNumber = appUserEditDto.PhoneNumber;
            user.Surname = appUserEditDto.Surname;
            user.City = appUserEditDto.City;
            user.District = appUserEditDto.District;
            user.Name = appUserEditDto.Name;
            user.ImageUrl = "test";
            user.Email = appUserEditDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.Message = "Your information has been successfully updated.";
            }
            else
            {
                ViewBag.Message = "An error occurred while updating your information.";
            }
            return View(appUserEditDto);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AppUserChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    if (model.NewPassword == model.ConfirmNewPassword)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Login");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    ModelState.AddModelError("", "New password and confirm password do not match.");
                }
                ModelState.AddModelError("", "User not found.");
            }
            return View(model);
        }


    }
}