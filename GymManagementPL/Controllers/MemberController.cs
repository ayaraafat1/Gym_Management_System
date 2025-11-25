using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MemberVM;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memeberService;

        public MemberController(IMemberService memeberService)
        {
            _memeberService = memeberService;
        }
        #region Get All Members
        public ActionResult Index()
        {
            var members = _memeberService.GetAllMembers();
            return View(members);
        }
        #endregion
        #region Get Member Details
        public ActionResult MemberDetails(int id)
        {
            // id = memberViewModel 
            // id not valid  id <= 0
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot bet negative or Zero";
                return RedirectToAction(nameof(Index));
            }

            var member = _memeberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }
        #endregion

        #region Get Health record Details
        public ActionResult HealthReordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot bet negative or Zero";
                return RedirectToAction(nameof(Index));
            }


            var healthRecordData = _memeberService.GetMemberHealthDetails(id);
            if (healthRecordData is null)
            {
                TempData["ErrorMessage"] = "health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecordData);
        }
        #endregion

        #region Create Member
        //BaseUrl/Member/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "There are missing Fields");
                return View(nameof(Create), createMember);
            }
            bool result = _memeberService.CreateMember(createMember);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Edit Member


        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot be Neagtive or Zero";
                return RedirectToAction(nameof(Index));
            }

            var member = _memeberService.GetMemberToUpdateDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, UpdateMemberViewModel memberToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(memberToUpdate);
            }

            var result = _memeberService.UpdateMemberDetails(id, memberToUpdate);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Update";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Delete Member


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be negative or 0";
                return RedirectToAction(nameof(Index));
            }

            var member = _memeberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
            }
            ViewBag.MemberId = id;
            return View();


        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _memeberService.DeleteMember(id);
            if (result)
            {
                TempData["SuccessMessaage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member cannot deleted";
            }
            return RedirectToAction(nameof(Index));


        }

        #endregion
    }
}
