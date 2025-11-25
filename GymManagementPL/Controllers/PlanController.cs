using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.PlanVM;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }


        #region Get all plans
        //Get: Index
        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();

            return View(plans);
        }
        #endregion
        #region Details
        //Get: Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or zero";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanDetails(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        #endregion

        #region Update Plan
        //Get: Date To Update
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["MessageError"] = "Id cannot be Negative or 0";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["MessageError"] = "Plan Not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        //Post : Submit update
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, PlanToUpdateViewModel planToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check data Validation");
                return View(planToUpdate);
            }
            var result = _planService.UpdatePlan(id, planToUpdate);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update";

            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Toggle 
        //Post : Toggle status
        [HttpPost]
        public ActionResult Activate(int id)
        {
            var result = _planService.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan ToggleStatus Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Plan ToggleStatus Failed ";

            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
