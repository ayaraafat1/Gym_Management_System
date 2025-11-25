using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.PlanVM;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (plans is null || !plans.Any()) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan is null) return null;

            return _mapper.Map<PlanViewModel>(plan);

        }

        public PlanToUpdateViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan is null || plan.IsActive == false || HasActiveMemberShips(planId))
                return null;

    
            return _mapper.Map<PlanToUpdateViewModel>(plan); 
        }

        public bool UpdatePlan(int planId, PlanToUpdateViewModel planToUpdate)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(planId);

            if (plan is null || planToUpdate is null) 
                return false;

           _mapper.Map(planToUpdate,plan);

            try
            {
                planRepo.Update(plan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool ToggleStatus(int planId)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(planId);

           if(plan is null || HasActiveMemberShips(planId)) return false;

           plan.IsActive = plan.IsActive == true? false : true;
            plan.UpdatedAt = DateTime.Now;

            try
            {
                planRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods
        private bool HasActiveMemberShips(int planId)
        {
            var activeMemberShip = _unitOfWork.GetRepository<Membership>()
                .GetAll(p=>p.PlanId == planId && p.Status=="Active");

            return activeMemberShip.Any();
        }
        #endregion
    }
}
