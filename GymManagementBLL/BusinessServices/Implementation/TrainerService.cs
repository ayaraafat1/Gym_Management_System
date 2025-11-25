using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MemberVM;
using GymManagementBLL.ViewModels.PlanVM;
using GymManagementBLL.ViewModels.TrainerVM;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (createTrainer is null || IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone))
                return false;

            var trainer = _mapper.Map< CreateTrainerViewModel ,Trainer>(createTrainer);
            try
            {

                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();

            if (trainers is null || !trainers.Any()) return [];

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null) return null;

            return _mapper.Map<TrainerViewModel>(trainer);

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdateDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);

            if (trainer is null) return null;


            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel trainerToUpdate)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var EmailExistForAnotherOldTranier= trainerRepo
                .GetAll(t => t.Email == trainerToUpdate.Email&& t.Id != trainerId)
                .Any();

            var PhoneExistForAnotherOldTranier= trainerRepo
                .GetAll(t => t.Phone == trainerToUpdate.Phone&& t.Id != trainerId)
                .Any();

            if (trainerToUpdate is null || PhoneExistForAnotherOldTranier || EmailExistForAnotherOldTranier)
                return false;

            var trainer = trainerRepo.GetById(trainerId);

            if (trainer is null) return false;
            _mapper.Map(trainerToUpdate, trainer);
            try
            {
                trainerRepo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteTrainer(int trainerId)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = trainerRepo.GetById(trainerId);

            if (trainer is null || HasActiveSessions(trainerId)) return false;
            try
            {
                trainerRepo.Delete(trainer);
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

            return _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {

            return _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();
        }

        private bool HasActiveSessions(int trainerId)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now)
                .Any();

            return activeSessions;
        }
        #endregion

    }
}
