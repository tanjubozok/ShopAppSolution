using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        private IHttpContextAccessor _accessor;

        public AccountController(ICartService cartService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IHttpContextAccessor accessor)
        {
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _accessor = accessor;
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isCaptchaValid = await IsCaptchaValid(model.GoogleCaptchaToken);
            if (isCaptchaValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ComfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = code
                    });

                    await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:44384{callbackUrl}'>tıklayınız.</a>");

                    TempData.Put("Message", new ResultMessage
                    {
                        Title = "Hesap Onayı",
                        Message = "Eposta adresine gelen link ile hesabınızı onaylayınız.",
                        Css = "warning"
                    });

                    return RedirectToAction("login", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("GoogleCaptcha", "Captcha doğrulanmadı.");
                TempData.Put("Message", new ResultMessage
                {
                    Title = "GoogleCaptcha",
                    Message = "Captcha doğrulanmadı.",
                    Css = "warning"
                });
            }
            return View(model);
        }

        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isCaptchaValid = await IsCaptchaValid(model.GoogleCaptchaToken);
            if (isCaptchaValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamış.");
                    return View(model);
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Lütfen hesabınızı email ile onaylayınız.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl ?? "~/");
                }
            }
            else
            {
                ModelState.AddModelError("GoogleCaptcha", "Captcha doğrulanmadı.");
                TempData.Put("Message", new ResultMessage
                {
                    Title = "GoogleCaptcha",
                    Message = "Captcha doğrulanmadı.",
                    Css = "warning"
                });
            }
            ModelState.AddModelError("", "Email veya parola yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("Message", new ResultMessage
            {
                Title = "Oturum Kapatıldı.",
                Message = "Hesabınız güvenli bir şekilde kapatıldı.",
                Css = "success"
            });
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ComfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("Message", new ResultMessage
                {
                    Title = "Hesap Onayı",
                    Message = "Hesap onayı için bilgileriniz yanlıştır.",
                    Css = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _cartService.InitializeCart(user.Id);

                    TempData.Put("Message", new ResultMessage
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarılı bir şekilde onaylandı.",
                        Css = "success"
                    });
                    return RedirectToAction("Login");
                }
            }

            TempData.Put("Message", new ResultMessage
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanamadı.",
                Css = "danger"
            });
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.EmailAddress))
            {
                TempData.Put("Message", new ResultMessage
                {
                    Title = "Forget Password",
                    Message = "Mail adresi boş olamaz.",
                    Css = "danger"
                });
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user == null)
            {
                TempData.Put("Message", new ResultMessage
                {
                    Title = "Forget Password",
                    Message = "Mail adresi ile hesap oluşturulmamış.",
                    Css = "danger"
                });
                return View(model);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code
            });

            await _emailSender.SendEmailAsync(model.EmailAddress, "Reset Password.", $"Parolanızı yenilemek için linke <a href='https://localhost:44384{callbackUrl}'>tıklayınız.</a>");
            TempData.Put("Message", new ResultMessage
            {
                Title = "Forget Password",
                Message = "Şifrenizi yenilemek için mail adresinize gönderildi.",
                Css = "warning"
            });
            return RedirectToAction("login", "Account");
        }

        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel
            {
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<bool> IsCaptchaValid(string response)
        {
            try
            {
                string ipAddres = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var secret = "6Ld7mtwdAAAAANibbLefMd3IfNFFrD1H-69SbZRY";
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        {"secret",secret},
                        {"response",response},
                        {"remoteip", ipAddres},
                    };

                    var content = new FormUrlEncodedContent(values);
                    var verify = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                    var captchaResponseJson = await verify.Content.ReadAsStringAsync();
                    var captchaResult = JsonConvert.DeserializeObject<CaptchaResponseModel>(captchaResponseJson);
                    return captchaResult.Success && captchaResult.Action == "create" && captchaResult.Score > 0.5;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
