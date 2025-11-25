using GymManagementBLL.ViewModels.MemberVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createMember);

        MemberViewModel? GetMemberDetails(int memberId);
        HealthRecordViewModel? GetMemberHealthDetails(int memberId);

        UpdateMemberViewModel? GetMemberToUpdateDetails(int memberId);
        bool UpdateMemberDetails(int memberId,UpdateMemberViewModel updateMemberViewModel);

        bool DeleteMember(int memberId);
    }
}
