using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MemberVM;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone))
                    return false;

                var member = _mapper.Map<CreateMemberViewModel,Member>(createMember);
 
                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool DeleteMember(int memberId)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();
                var membershipReo = _unitOfWork.GetRepository<Membership>();

                var member = memberRepo.GetById(memberId);
                if (member is null) return false;
                var memberSessionIds = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(m => m.MemberId == memberId)
                    .Select(m => m.SessionId);

                var hasFutureSessions = _unitOfWork.GetRepository<Session>().GetAll(
                   S => memberSessionIds.Contains(S.Id) && S.StartDate > DateTime.Now).Any();

                if (hasFutureSessions) return false;

                var memberShips = membershipReo.GetAll(m => m.MemberId == memberId);
                if (memberShips.Any())
                {
                    foreach (var memberShip in memberShips)
                    {
                        membershipReo.Delete(memberShip);
                    }
                }
                memberRepo.Delete(member);

                return _unitOfWork.SaveChanges()> 0;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();

            if (members is null || !members.Any()) return [];

            return _mapper.Map<IEnumerable<MemberViewModel>>(members);

        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            
            var memberViewModel = _mapper.Map<MemberViewModel>(member);


            var memberShip = _unitOfWork.GetRepository<Membership>().GetAll(MS => MS.MemberId == memberId && MS.Status == "Active").FirstOrDefault();

            if (memberShip is not null) 
            {
                memberViewModel.MemberShipStartDate = memberShip.CreatedAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = memberShip.EndDate.ToShortDateString();
                
                var plan = _unitOfWork.GetRepository<Plan>().GetById(memberShip.PlanId);
                memberViewModel.PlanName = plan?.Name;
            }

            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthDetails(int memberId)
        {
            var memberHealth = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (memberHealth is null) return null;


            return _mapper.Map<HealthRecordViewModel>(memberHealth);
        }

        public UpdateMemberViewModel? GetMemberToUpdateDetails(int memberId)
        {

            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            return _mapper.Map<UpdateMemberViewModel>(member);
        }

        public bool UpdateMemberDetails(int memberId, UpdateMemberViewModel updateMemberViewModel)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();

                if (IsPhoneExist(updateMemberViewModel.Phone) || IsEmailExist(updateMemberViewModel.Email)) 
                    return false;

                var member = memberRepo.GetById(memberId);
                if (member is null) return false;

                _mapper.Map(updateMemberViewModel,member);

                memberRepo.Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExist(string email)
        {

            return _unitOfWork.GetRepository<Member>().GetAll(M => M.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {

            return _unitOfWork.GetRepository<Member>().GetAll(M => M.Phone == phone).Any();
        }
        #endregion
    }
}
