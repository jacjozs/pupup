using Microsoft.AspNetCore.Hosting;
using PupUp.Data;
using PupUp.Models.Badges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PupUp.Services
{
    public class BadgeService
    {
        private readonly PupUpDbContext m_context;
        private readonly IWebHostEnvironment m_hostEnvironment;
        public BadgeService(PupUpDbContext context, IWebHostEnvironment hostEnvironment)
        {
            m_context = context;
            m_hostEnvironment = hostEnvironment;
        }
        public void AddBadges(IEnumerable<int> ids, string userId)
        {
            foreach (var id in ids)
            {
                if (m_context.UserBadges.FirstOrDefault(u => u.BadgeId == id && u.UserId == userId) == null)
                {
                    m_context.UserBadges.Add(new UserBadge()
                    {
                        BadgeId = id,
                        UserId = userId
                    });
                }
            }
        }
        public void AddDogBadges(IEnumerable<int> ids, int dogId)
        {
            foreach (var id in ids)
            {
                if (m_context.DogBadges.FirstOrDefault(u => u.BadgeId == id && u.DogId == dogId) == null)
                {
                    m_context.DogBadges.Add(new DogBadge()
                    {
                        BadgeId = id,
                        DogId = dogId
                    });
                }
            }
        }
        public void DeleteImage(Badge badge)
        {
            if (string.IsNullOrEmpty(badge.ImageUrl)) return;
            string path = Path.Combine(m_hostEnvironment.WebRootPath, "images/badges", badge.ImageUrl);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public async Task<bool> SaveImage(Badge badge)
        {
            string fileName = Path.GetFileNameWithoutExtension(badge.ImageFile.FileName);
            string extension = Path.GetExtension(badge.ImageFile.FileName);
            badge.ImageUrl = fileName = $"{fileName}_{DateTime.Now.ToString("yymmssfff")}{extension}";
            string path = Path.Combine(m_hostEnvironment.WebRootPath, "images/badges", fileName);
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await badge.ImageFile.CopyToAsync(fileStream);
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
