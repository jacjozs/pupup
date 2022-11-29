using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PupUp.Data;
using PupUp.Models.Trainings;
using PupUp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PupUp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainingStepController : Controller
    {
        private readonly PupUpDbContext m_context;
        private readonly TrainingService m_trainingService;
        public TrainingStepController(PupUpDbContext context, TrainingService service)
        {
            m_context = context;
            m_trainingService = service;
        }

        // GET: TrainingStep
        [HttpGet]
        public IActionResult Index()
        {
            return View(m_trainingService.TrainingSteps);
        }

        // GET: TrainingStep/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || m_context.TrainingSteps == null)
            {
                return NotFound();
            }

            var trainingStep = await  m_context.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }

            return View(trainingStep);
        }

        // GET: TrainingStep/Create
        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewData["TrainingId"] = id;
            return View();
        }

        // POST: TrainingStep/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,ImageFile")] TrainingStep trainingStep, int trainingId)
        {
            if (ModelState.IsValid)
            {
                trainingStep.TrainingId = trainingId;
                var training = await m_context.Trainings.Include(t => t.Steps).FirstOrDefaultAsync(t => t.Id == trainingId);
                if (training == null) return NotFound();
                trainingStep.Index = training.Steps.Count();
                if (await m_trainingService.SaveImage(trainingStep))
                {
                    await m_context.TrainingSteps.AddAsync(trainingStep);
                    await m_context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(trainingStep);
        }

        // GET: TrainingStep/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || m_context.TrainingSteps == null)
            {
                return NotFound();
            }

            var trainingStep = await m_context.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }
            return View(trainingStep);
        }

        // POST: TrainingStep/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,ImageFile")] TrainingStep trainingStep)
        {
            if (id != trainingStep.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var step = await m_context.TrainingSteps.FindAsync(trainingStep.Id);
                    if (step == null) return NotFound();
                    if(trainingStep.ImageFile != null)
                    {
                        m_trainingService.DeleteImage(step);
                        await m_trainingService.SaveImage(trainingStep);
                    }
                    m_context.TrainingSteps.Update(trainingStep);
                    await m_context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingStepExists(trainingStep.Id))
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
            return View(trainingStep);
        }

        // GET: TrainingStep/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || m_context.TrainingSteps == null)
            {
                return NotFound();
            }

            var trainingStep = await m_context.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }

            return View(trainingStep);
        }

        // POST: TrainingStep/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (m_context.TrainingSteps == null)
            {
                return Problem("Entity set 'PupUpDbContext.TrainingSteps'  is null.");
            }
            var trainingStep = await m_context.TrainingSteps.FindAsync(id);
            if (trainingStep == null)
            {
                return NotFound();
            }
            m_trainingService.DeleteImage(trainingStep);
            m_context.TrainingSteps.Remove(trainingStep);
            await m_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingStepExists(int id)
        {
          return m_context.TrainingSteps.Any(e => e.Id == id);
        }
    }
}
