using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class AnalyticService:IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public HomeAnalyticsViewModel GetHomeAnalyticsViewModel()
        {
            var sessions = _unitOfWork.GetRepository<Session>().GetAll();
            return new HomeAnalyticsViewModel
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(X => X.Status == "Active").Count(),
                UpcomingSessions = sessions.Count(X => X.StartDate > DateTime.Now),
                OngoingSessinos = sessions.Count(X => X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now),
                CompletedSessinos = sessions.Count(X => X.EndDate < DateTime.Now)
            };
        }
    }
}
