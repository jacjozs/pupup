using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PupUp.Data;
using PupUp.Models.Trainings;
using PupUp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PupUp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainingController : Controller
    {
        private readonly PupUpDbContext m_context;
        private readonly TrainingService m_trainingService;

        public TrainingController(PupUpDbContext context, TrainingService service)
        {
            m_context = context;
            m_trainingService = service;
        }

        // GET: Training
        public IActionResult Index()
        {
              return View(m_trainingService.Trainings);
        }

        // GET: Training/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || m_context.Trainings == null)
            {
                return NotFound();
            }

            var training = await m_context.Trainings.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }
        [HttpGet]
        public async Task<IActionResult> Steps(int? id)
        {
            if (id == null || m_context.Trainings == null || m_context.TrainingSteps == null)
            {
                return NotFound();
            }

            var steps = await m_context.TrainingSteps.Where(s => s.TrainingId == id.Value).ToListAsync();
            if (steps == null)
            {
                return NotFound();
            }

            return View(steps);
        }
        [HttpPost]
        public async Task<IActionResult> Steps(string ItemPair)
        {
            if (ItemPair == null) return BadRequest();
            var pairs = ItemPair.Split(';');
            string[] items;
            List<(int, int)> ids = new List<(int, int)>();
            foreach (var pair in pairs)
            {
                if (string.IsNullOrEmpty(pair)) continue;
                items = pair.Split(':');
                ids.Add(new (int.Parse(items[0]), int.Parse(items[1]) + 1));
            }
            var steps = m_context.TrainingSteps.ToList().Where(s => ids.Count(i => i.Item1 == s.Id) > 0);
            if (steps == null) return BadRequest();

            foreach (var step in steps)
            {
                step.Index = ids.Find(i => i.Item1 == step.Id).Item2;
            }
            m_context.TrainingSteps.UpdateRange(steps);
            await m_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Training/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Training/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Training training)
        {
            if (ModelState.IsValid)
            {
                await m_context.Trainings.AddAsync(training);
                await m_context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        // GET: Training/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || m_context.Trainings == null)
            {
                return NotFound();
            }

            var training = await m_context.Trainings.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }

        // POST: Training/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Training training)
        {
            if (id != training.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    m_context.Trainings.Update(training);
                    await m_context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.Id))
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
            return View(training);
        }

        // GET: Training/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || m_context.Trainings == null)
            {
                return NotFound();
            }

            var training = await m_context.Trainings.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Training/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (m_context.Trainings == null)
            {
                return Problem("Entity set 'PupUpDbContext.Trainings'  is null.");
            }
            var training = await m_context.Trainings.FindAsync(id);
            if (training == null)
                return NotFound();
            m_context.Trainings.Remove(training);
            await m_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
          return m_context.Trainings.Any(e => e.Id == id);
        }
    }
}
