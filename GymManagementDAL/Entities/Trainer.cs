using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer:GymUser
    {
        public Specialties Specialties { get; set; }



        #region Relationships

        #region Trainer - Session
        public ICollection<Session> Sessions { get; set; }
        #endregion

        #endregion
    }
}
