using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PupUp.Data;
using PupUp.Helpers.Extensions;
using PupUp.Models.Dogs;
using PupUp.Models.Quests.Enums;
using PupUp.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PupUp.Controllers
{
    [Authorize]
    public class DogController : Controller
    {
        private readonly PupUpDbContext m_dbContext;
        private readonly TrainingService m_service;
        private readonly QuestService m_questService;
        private readonly EventService m_eventService;
        public DogController(PupUpDbContext dbContext, TrainingService service, QuestService questServie, EventService eventService)
        {
            m_dbContext = dbContext;
            m_service = service;
            m_questService = questServie;
            m_eventService = eventService;
        }
        public async Task<IActionResult> Index()
        {
            var dogs = await m_dbContext.Dogs.Where(d => d.UserId == User.Claims.GetClaim(ClaimTypes.NameIdentifier)).ToListAsync();
            return View(dogs);
        }
        public async Task<IActionResult> Dog(int id)
        {
            var dog = await m_dbContext.Dogs.Include(d => d.TrainingStates).ThenInclude(c => c.Training).FirstOrDefaultAsync(d => d.Id == id);
            ViewData["Quests"] = m_dbContext.Quests.Where(q => !q.UserQuest).ToList();
            ViewData["Badges"] = m_dbContext.Badges.Where(b => !b.UserBadge).ToList();
            ViewData["DogQuests"] = m_dbContext.DogQuests.Where(d => d.DogId == dog.Id).ToList();
            ViewData["DogBadges"] = m_dbContext.DogBadges.Where(d => d.DogId == dog.Id).ToList();
            ViewData["Events"] = m_dbContext.Events.Include(e => e.User).Where(d => d.UserId != User.Claims.GetClaim(ClaimTypes.NameIdentifier)).ToList();
            return View(dog);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type")] Dog dog)
        {
            if (ModelState.IsValid)
            {
                dog.UserId = User.Claims.GetClaim(ClaimTypes.NameIdentifier);
                m_dbContext.Dogs.Add(dog);
                await m_dbContext.SaveChangesAsync();
                m_questService.DoAction(ActionType.AddNewDog, User, null, dog.Id);
                return RedirectToAction("Profil", "Home");
            }
            return RedirectToAction("Profil", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Training(int id, int dog)
        {
            ViewData["Dog"] = dog;
            ViewData["Training"] = id;
            var trainingSteps = await m_service.GetTrainingSteps(id);
            if (trainingSteps == null) RedirectToAction("Error", "Home");
            trainingSteps.Sort();
            ViewData["Steps"] = trainingSteps;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Training([Bind("DogId,TrainingId,State")] DogTrainingState newState)
        {
            newState.UpdateTime = DateTime.Now;
            var dogState = await m_service.GetState(newState.TrainingId, newState.DogId);
            if (dogState == null)
            {
                await m_dbContext.DogTrainingStates.AddAsync(newState);
            }
            else
            {
                dogState.State = newState.State;
                dogState.UpdateTime = DateTime.Now;
                m_dbContext.DogTrainingStates.Update(dogState);
            }
            await m_dbContext.SaveChangesAsync();
            switch (newState.State)
            {
                case Models.Trainings.Enums.TrainingState.NotLearned:
                case Models.Trainings.Enums.TrainingState.InProgress:
                    m_questService.DoAction(ActionType.StartTraining, User, newState.TrainingId, newState.DogId);
                    break;
                case Models.Trainings.Enums.TrainingState.Learned:
                    m_questService.DoAction(ActionType.LearnTraning, User, newState.TrainingId, newState.DogId);
                    break;
                case Models.Trainings.Enums.TrainingState.Skill:
                    m_questService.DoAction(ActionType.SkillTraining, User, newState.TrainingId, newState.DogId);
                    break;
                default:
                    break;
            }
            return RedirectToAction(nameof(Dog), new { id = newState.DogId });
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || m_dbContext.Dogs == null)
            {
                return NotFound();
            }

            var dog = await m_dbContext.Dogs.FindAsync(id);
            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Gender,Age")] Dog dog)
        {
            if (id != dog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dog.UserId = User.Claims.GetClaim(ClaimTypes.NameIdentifier);
                    m_dbContext.Dogs.Update(dog);
                    await m_dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(HomeController.Profil), "Home");
        }
        private bool DogExists(int id)
        {
            return m_dbContext.Dogs.Any(e => e.Id == id);
        }
    }
}
