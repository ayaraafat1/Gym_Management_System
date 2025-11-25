using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.SessionVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        #region Get all Sessions
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();

            return View(sessions);
        }
        #endregion

        #region Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be Negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);

        }

        #endregion

        #region Create


        public ActionResult Create()
        {
            LoadDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDown();
                ModelState.AddModelError("DataInvalid", "There are missing Fields");
                return View(nameof(Create), createSession);
            }

            bool result = _sessionService.CreateSession(createSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                LoadDropDown();
                TempData["ErrorMessage"] = "Session Failed To Create";
                return View(createSession);
            }
        }


        #endregion


        #region Edit

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdateDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Cannot find the Session";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainerDropDown();
            return View(session);
        }
        [HttpPost]
        public ActionResult Edit(int id, UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerDropDown();
                return View(updateSession);
            }

            var result = _sessionService.UpdateSessionDetails(id, updateSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Update";
                LoadTrainerDropDown();
                return View(updateSession);
            }

        }
        #endregion


        #region Delete

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionDetails(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Cannot find Session";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = session.Id;
            return View();
        }


        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot negative or Zero";
                return RedirectToAction(nameof(Index));
            }
            var res = _sessionService.DeleteSession(id);
            if (res)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot delete this session";

            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region HelperMethod

        private void LoadDropDown()
        {
            LoadTrainerDropDown();
            LoadCategoryDropDown();
        }

        private void LoadTrainerDropDown()
        {
            var trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        private void LoadCategoryDropDown()
        {
            var categories = _sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
        #endregion
    }
}
