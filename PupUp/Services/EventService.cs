using PupUp.Data;
using PupUp.Models.Events;

namespace PupUp.Services
{
    public class EventService
    {
        private readonly PupUpDbContext m_dbContext;
        public EventService(PupUpDbContext dbContext)
        {
            m_dbContext = dbContext;
        }
        public void AddEvent(string userId, string desc)
        {
            m_dbContext.Events.Add(new PupUpEvent()
            {
                Time = System.DateTime.Now,
                UserId = userId,
                Desc = desc
            });
        }
    }
}
