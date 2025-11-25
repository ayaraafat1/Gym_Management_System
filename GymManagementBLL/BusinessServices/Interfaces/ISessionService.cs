using GymManagementBLL.ViewModels.MemberVM;
using GymManagementBLL.ViewModels.SessionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionDetails(int sessionId);
        bool CreateSession(CreateSessionViewModel createSession);
        UpdateSessionViewModel? GetSessionToUpdateDetails(int sessionId);
        bool UpdateMemberDetails(int sessionId, UpdateSessionViewModel updateSession);
        bool DeleteSession(int sessionId);
    }
}
