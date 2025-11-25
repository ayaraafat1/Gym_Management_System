using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels
{
    public class HomeAnalyticsViewModel
    {
        public int TotalMembers { get; set; }
        public int TotalTrainers { get; set; }
        public int ActiveMembers { get; set; }
        public int UpcomingSessions { get; set; }
        public int OngoingSessinos { get; set; }
        public int CompletedSessinos { get; set; }
    }
}
