using PupUp.Data;

namespace PupUp.Services
{
    public class PointService
    {
        private readonly PupUpDbContext m_dbContext;
        public PointService(PupUpDbContext dbContext)
        {
            m_dbContext = dbContext;
        }
        public void AddExp(string userId, long exp)
        {
            var points = m_dbContext.Points.Find(userId);
            if (points == null) points = new Models.Points()
            {
                UserId = userId
            };
            m_dbContext.Points.Add(points);
            points.Exp += exp;
        }
        public void AddCoin(string userId, long coin)
        {
            var points = m_dbContext.Points.Find(userId);
            if (points == null)
            {
                points = new Models.Points()
                {
                    UserId = userId
                };
                m_dbContext.Points.Add(points);
            }
            points.Coin += coin;
        }
    }
}
