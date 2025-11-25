using GymManagementBLL.ViewModels.MemberVM;
using GymManagementBLL.ViewModels.TrainerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        TrainerViewModel? GetTrainerDetails(int trainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdateDetails(int trainerId);
        bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel trainerToUpdate);

        bool DeleteTrainer(int trainerId);
    }
}
