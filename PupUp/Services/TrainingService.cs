using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PupUp.Data;
using PupUp.Models.Dogs;
using PupUp.Models.Trainings;
using PupUp.Models.Trainings.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PupUp.Services
{
    public sealed class TrainingService
    {
        private readonly PupUpDbContext m_dbContext;
        private readonly IWebHostEnvironment m_hostEnvironment;
        public TrainingService(PupUpDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            m_dbContext = dbContext;
            m_hostEnvironment = hostEnvironment;
        }
        public IEnumerable<Training> Trainings => m_dbContext.Trainings;
        public IEnumerable<TrainingStep> TrainingSteps => m_dbContext.TrainingSteps;
        public async Task<Training> GetTraining(int? id) => await m_dbContext.Trainings.FindAsync(id);
        public async Task<List<TrainingStep>> GetTrainingSteps(int? id) => id.HasValue ? await m_dbContext.TrainingSteps.Where(t => t.TrainingId == id.Value).ToListAsync() : null;
        public async Task<DogTrainingState> GetState(int trainingId, int dogId) => await m_dbContext.DogTrainingStates.FirstOrDefaultAsync(s => s.DogId == dogId && s.TrainingId == trainingId);
        public int GetTrainingStateCount(int dogId, TrainingState state) => m_dbContext.DogTrainingStates.Where(d => d.DogId == dogId && d.State == state).Count();
        public int GetTrainingNotLearnedCount(int dogId)
        {
            var dogStates = m_dbContext.DogTrainingStates.Where(d => d.DogId == dogId).ToList();
            return m_dbContext.Trainings.Count() - dogStates.Count;
        }
        public int GetDogPercent(int dogId)
        {
            var dogStates = m_dbContext.DogTrainingStates.Where(d => d.DogId == dogId).ToList();
            int max = m_dbContext.Trainings.Count();
            float percent = 0f, delta = max / 100f;
            foreach (var state in dogStates)
            {
                switch (state.State)
                {
                    case TrainingState.InProgress:
                        percent += (delta * .5f);
                        break;
                    case TrainingState.Learned:
                        percent += (delta * .75f);
                        break;
                    case TrainingState.Skill:
                        percent += delta;
                        break;
                    default:
                        break;
                }
            }
            return (int)MathF.Round(percent * 100f);
        }
        public void DeleteImage(TrainingStep trainingStep)
        {
            if (string.IsNullOrEmpty(trainingStep.ImageUrl)) return;
            string path = Path.Combine(m_hostEnvironment.WebRootPath, "images/steps", trainingStep.ImageUrl);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public async Task<bool> SaveImage(TrainingStep trainingStep)
        {
            string fileName = Path.GetFileNameWithoutExtension(trainingStep.ImageFile.FileName);
            string extension = Path.GetExtension(trainingStep.ImageFile.FileName);
            trainingStep.ImageUrl = fileName = $"{fileName}_{DateTime.Now.ToString("yymmssfff")}{extension}";
            string path = Path.Combine(m_hostEnvironment.WebRootPath, "images/steps", fileName);
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await trainingStep.ImageFile.CopyToAsync(fileStream);
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
