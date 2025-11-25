using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.TrainerVM;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get all Trainers
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        #endregion
        #region Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or 0";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(trainer);
        }
        #endregion
        #region Create

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainerModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "There are missing Fields");
                return View(nameof(Create), createTrainerModel);
            }
            bool res = _trainerService.CreateTrainer(createTrainerModel);
            if (res)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot Create Trainer";

            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
        #region Edit

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be Neagtive or Zero";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerToUpdateDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
            }

            return View(trainer);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, [FromForm] TrainerToUpdateViewModel trainerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Missing Data", " There are missing data");
                return View(trainerToUpdate);
            }
            bool res = _trainerService.UpdateTrainerDetails(id, trainerToUpdate);
            if (res)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Update";

            }
            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Delete

        #region Delete With Confirmation
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be Neagtive or Zero";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "trainer Not Found";
            }
            ViewBag.TrainerId = trainer.Id;
            ViewBag.TrainerName = trainer.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var result = _trainerService.DeleteTrainer(id);
            if (result)
            {
                TempData["SuccessMessaage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer cannot Deleted";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion


        #endregion
    }
}
