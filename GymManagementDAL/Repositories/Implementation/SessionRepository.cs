using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Implementation
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbcontext;

        public SessionRepository(GymDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Session> GetAllWithCategoryAndTrainer()
        {
           return _dbcontext.Sessions
                .Include(S=>S.Category)
                .Include(S=>S.Trainer)
                .ToList();
        }

        public Session? GetByIdWithCategoryAndTrainer(int sessionId)
        {
            return _dbcontext.Sessions
                 .Include(S => S.Category)
                 .Include(S => S.Trainer)
                 .FirstOrDefault(S=>S.Id == sessionId);
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbcontext.MemberSessions.Count(S => S.SessionId == sessionId);
        }
    }
}
