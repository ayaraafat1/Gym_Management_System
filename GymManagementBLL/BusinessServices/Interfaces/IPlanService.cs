using GymManagementBLL.ViewModels.PlanVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int planId);
        PlanToUpdateViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlan(int planId,PlanToUpdateViewModel planToUpdate);
        bool ToggleStatus(int planId);
    }
}
