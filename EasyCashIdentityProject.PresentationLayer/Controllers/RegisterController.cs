using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;


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

                Random random = new Random();
                int code;
                code = random.Next(100000, 1000000);
				string strCode = code.ToString();
				string firstHalf = strCode.Substring(0, 3);
				string secondHalf = strCode.Substring(3, 3);

				string formattedCode = firstHalf + "-" + secondHalf;

				AppUser appUser = new AppUser()
                {
                    UserName = appUserRegisterDto.Username,
                    Name = appUserRegisterDto.Name,
                    Surname = appUserRegisterDto.Surname,
                    Email = appUserRegisterDto.Email,
                    City = "city",
                    District = "district",
                    ImageUrl = "img",
                    ConfirmCode = code,
                };
                var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password); 
                //CreateAsync identity işlemini sağlar.

                if (result.Succeeded) 
                {
                    MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAddressFrom = new MailboxAddress("Easy Cash Admin", "burcuozturk.dev@gmail.com");
                    MailboxAddress mailboxAddressTo = new MailboxAddress("User", appUser.Email);

                    mimeMessage.From.Add(mailboxAddressFrom);
                    mimeMessage.To.Add(mailboxAddressTo);

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = "Your code to complete the registration process is: " + formattedCode;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();
                    mimeMessage.Subject = "Easy Cash Confirmation Code";

					using (var client = new SmtpClient())
					{
						await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
						await client.AuthenticateAsync("burcuozturk.dev@gmail.com", "ddirbzgdyxbkmqlf\r\n");
						await client.SendAsync(mimeMessage);
						await client.DisconnectAsync(true);
					}


					return RedirectToAction("Index", "ConfirmMail");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }
    }
}


//şifre en az 6 karakter, 1 küçük harf, 1 büyük harf, 1 sembol, 1sayı içermeli.
