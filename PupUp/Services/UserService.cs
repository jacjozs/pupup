using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PupUp.Data;
using PupUp.Helpers.Extensions;
using PupUp.Models.Identity;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PupUp.Services
{
    public class UserService
    {
        public static string DefaultProfilName = "default_profil.png";
        public static string ClaimProfilName = "profil_img_url";
        private readonly PupUpDbContext m_dbContext;

        private UserManager<PupUpUser> m_userManager;
        private readonly IWebHostEnvironment m_hostEnvironment;
        public UserService(PupUpDbContext dbContext, UserManager<PupUpUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            m_dbContext = dbContext;
            m_userManager = userManager;
            m_hostEnvironment = hostEnvironment;
        }
        public void DeleteImage(ClaimsPrincipal userClaims)
        {
            if (userClaims.Claims.GetClaim(ClaimProfilName) == DefaultProfilName) return;
            string path = Path.Combine(m_hostEnvironment.WebRootPath, "images/profils", userClaims.Claims.GetClaim(ClaimProfilName));
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public async Task<bool> SaveImage(ClaimsPrincipal userClaims, IFormFile image)
        {
            var user = await m_userManager.GetUserAsync(userClaims);
            string extension = Path.GetExtension(image.FileName);
            await m_userManager.ReplaceClaimAsync(user, userClaims.Claims.First(c => c.Type == ClaimProfilName), new Claim(ClaimProfilName, $"{userClaims.Claims.GetClaim(ClaimTypes.NameIdentifier)}{extension}"));
            user.ProfilImageUrl = userClaims.Claims.GetClaim(ClaimProfilName);
            string path = Path.Combine(m_hostEnvironment.WebRootPath, "images/profils", userClaims.Claims.GetClaim(ClaimProfilName));
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
