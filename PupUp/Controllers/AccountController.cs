using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PupUp.Models.Identity;
using PupUp.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PupUp.Controllers
{
    public class AccountController : Controller
    {
        private RoleManager<IdentityRole> m_roleManager;
        private UserManager<PupUpUser> m_userManager;
        private SignInManager<PupUpUser> m_signInManager;
        private UserService m_userService;
        public AccountController(UserManager<PupUpUser> userManager, SignInManager<PupUpUser> signInManager, UserService userService)
        {
            m_userManager = userManager;
            m_signInManager = signInManager;
            m_userService = userService;
        }

        public IActionResult Auth(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Type"] = "login";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(PupUpUser user, string returnUrl)
        {
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Type"] = "signup";
            if (ModelState.IsValid)
            {
                user.EmailConfirmed = true;
                user.ProfilImageUrl = UserService.DefaultProfilName;
                IdentityResult result = await m_userManager.CreateAsync(user, user.Password);
                if (result.Succeeded)
                {
                    await m_userManager.AddToRoleAsync(user, "user");
                    result = await m_userManager.AddClaimAsync(user, new Claim(UserService.ClaimProfilName, UserService.DefaultProfilName));
                    if (!result.Succeeded)
                    {
                        Errors(result);
                        return View("Auth");
                    }
                    ViewData["Type"] = "login";
                    await m_signInManager.PasswordSignInAsync(user, user.Password, false, true);
                    return View("Auth");
                }
                else
                {
                    Errors(result);
                    return View("Auth");
                }
            }
            return View("Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> Validate(string UserName, string Password, string returnUrl)
        {
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Type"] = "login";
            if (ModelState.IsValid)
            {
                var user = await m_userManager.FindByNameAsync(UserName);
                if (user != null)
                {
                    await m_signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await m_signInManager.PasswordSignInAsync(user, Password, false, true);
                    if (result.Succeeded)
                        return Redirect(returnUrl);
                    ModelState.AddModelError("UserName", "Error. Username or Password is invalid");
                }
            }
            return View("Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> ChangeProfil(IFormFile newProfil)
        {
            if (newProfil != null)
            {
                await m_userService.SaveImage(User, newProfil);
            }
            return Redirect("/");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await m_signInManager.SignOutAsync();
            return Redirect("/");
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
        }
    }
}
