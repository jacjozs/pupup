using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PupUp.Data;
using PupUp.Models.Badges;
using PupUp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PupUp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BadgeController : Controller
    {
        private readonly PupUpDbContext m_context;
        private readonly BadgeService m_badgeService;

        public BadgeController(BadgeService badge, PupUpDbContext context)
        {
            m_badgeService = badge;
            m_context = context;
        }

        // GET: Badge
        public async Task<IActionResult> Index()
        {
              return View(await m_context.Badges.ToListAsync());
        }

        // GET: Badge/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || m_context.Badges == null)
            {
                return NotFound();
            }

            var Badge = await m_context.Badges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Badge == null)
            {
                return NotFound();
            }

            return View(Badge);
        }

        // GET: Badge/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Badge/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,UserBadge,ImageFile")] Badge Badge)
        {
            if (ModelState.IsValid)
            {
                if (await m_badgeService.SaveImage(Badge))
                {
                    m_context.Add(Badge);
                    await m_context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(Badge);
        }

        // GET: Badge/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || m_context.Badges == null)
            {
                return NotFound();
            }

            var Badge = await m_context.Badges.FindAsync(id);
            if (Badge == null)
            {
                return NotFound();
            }
            return View(Badge);
        }

        // POST: Badge/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageFile")] Badge Badge)
        {
            if (id != Badge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldBadge = await m_context.Badges.FindAsync(Badge.Id);
                    if (oldBadge == null) return NotFound();
                    if (Badge.ImageFile != null)
                    {
                        m_badgeService.DeleteImage(oldBadge);
                        await m_badgeService.SaveImage(Badge);
                    }
                    m_context.Update(Badge);
                    await m_context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BadgeExists(Badge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Badge);
        }

        // GET: Badge/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || m_context.Badges == null)
            {
                return NotFound();
            }

            var Badge = await m_context.Badges
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Badge == null)
            {
                return NotFound();
            }

            return View(Badge);
        }

        // POST: Badge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (m_context.Badges == null)
            {
                return Problem("Entity set 'PupUpDbContext.Badges'  is null.");
            }
            var Badge = await m_context.Badges.FindAsync(id);
            if (Badge != null)
            {
                m_context.Badges.Remove(Badge);
            }
            m_badgeService.DeleteImage(Badge);
            m_context.Badges.Remove(Badge);
            await m_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BadgeExists(int id)
        {
          return m_context.Badges.Any(e => e.Id == id);
        }
    }
}
