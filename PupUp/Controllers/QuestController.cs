using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PupUp.Data;
using PupUp.Models.Quests;

namespace PupUp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestController : Controller
    {
        private readonly PupUpDbContext m_context;

        public QuestController(PupUpDbContext context)
        {
            m_context = context;
        }

        // GET: Quest
        public async Task<IActionResult> Index()
        {
              return View(await m_context.Quests.ToListAsync());
        }

        // GET: Quest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || m_context.Quests == null)
            {
                return NotFound();
            }

            var quest = await m_context.Quests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        // GET: Quest/Create
        public IActionResult Create()
        {
            ViewData["Quests"] = m_context.Quests;
            ViewData["Trainings"] = m_context.Trainings;
            ViewData["Badges"] = m_context.Badges;
            return View();
        }

        // POST: Quest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,RequiredQuests,TrainingIds,Repetable,ActionType,RewardType,RewardValue")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                m_context.Add(quest);
                await m_context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quest);
        }

        // GET: Quest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || m_context.Quests == null)
            {
                return NotFound();
            }

            var quest = await m_context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }
            ViewData["Quests"] = m_context.Quests;
            ViewData["Trainings"] = m_context.Trainings;
            ViewData["Badges"] = m_context.Badges;
            return View(quest);
        }

        // POST: Quest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,RequiredQuests,TrainingIds,Repetable,ActionType,RewardType,RewardValue")] Quest quest)
        {
            if (id != quest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    m_context.Update(quest);
                    await m_context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestExists(quest.Id))
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
            return View(quest);
        }

        // GET: Quest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || m_context.Quests == null)
            {
                return NotFound();
            }

            var quest = await m_context.Quests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        // POST: Quest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (m_context.Quests == null)
            {
                return Problem("Entity set 'PupUpDbContext.Quests'  is null.");
            }
            var quest = await m_context.Quests.FindAsync(id);
            if (quest != null)
            {
                m_context.Quests.Remove(quest);
            }
            
            await m_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestExists(int id)
        {
          return m_context.Quests.Any(e => e.Id == id);
        }
    }
}
