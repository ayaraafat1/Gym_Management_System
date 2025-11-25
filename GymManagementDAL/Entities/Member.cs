using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member:GymUser
    {
        public string? Photo { get; set; } 

        #region Relationships

        #region Member has HealthRecord
        public HealthRecord HealthRecord { get; set; }
        #endregion


        #region Member has Many Memberships
        public ICollection<Membership> Memberships { get; set; }
        #endregion

        #region Member has Many MemberSessions
        public ICollection<MemberSession> MemberSessions { get; set; }
        #endregion

        #endregion

    }
}
