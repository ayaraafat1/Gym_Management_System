using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.SessionVM;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementBLL.BusinessServices.Implementation
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions = sessionRepo.GetAllWithCategoryAndTrainer();

            if (sessions is null || !sessions.Any())
                return [];

            var mappedSession = _mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSession)
                session.AvailablesSlots = session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id);


            return mappedSession;
        }

        public SessionViewModel? GetSessionDetails(int sessionId)
        {
            var sessionRepo =_unitOfWork.SessionRepository;
            var session = sessionRepo.GetByIdWithCategoryAndTrainer(sessionId);

            if (session is null) return null;

            var mappedSession = _mapper.Map<Session,SessionViewModel>(session);
            mappedSession.AvailablesSlots = mappedSession.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id);
            
            return mappedSession;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsCategoryExist(createSession.CategoryId))
                    return false;
                if (!IsTrainerExist(createSession.TrainerId))
                    return false;
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate))
                    return false;
                if (createSession.Capacity > 25 || createSession.Capacity < 0)
                    return false;

                var sessionToCreate = _mapper.Map<Session>(createSession);
                _unitOfWork.GetRepository<Session>().Add(sessionToCreate);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public UpdateSessionViewModel? GetSessionToUpdateDetails(int sessionId)
        {
           var session = _unitOfWork.SessionRepository.GetById(sessionId);

            if (IsSessionAvailableForUpdate(session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public bool UpdateMemberDetails(int sessionId, UpdateSessionViewModel updateSession)
        {
            try
            {
                var sessionRepo = _unitOfWork.SessionRepository;
                var session = sessionRepo.GetById(sessionId);

                if (!IsTrainerExist(updateSession.TrainerId)) return false;
                if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;
                if (!IsSessionAvailableForUpdate(session!)) return false;


                 _mapper.Map(updateSession,session);
                session!.UpdatedAt = DateTime.Now;

                sessionRepo.Update(session);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }
        
        public bool DeleteSession(int sessionId)
        {
            try
            {
                var sessionRepo = _unitOfWork.SessionRepository;
                var session = sessionRepo.GetById(sessionId);

                if (!IsSessionAvailableForDelete(session!)) return false;

                sessionRepo.Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #region Helper Methods
        private bool IsTrainerExist(int trainerId)
        {
          return  _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }
        private bool IsCategoryExist(int categoryId)
        {
          return  _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }
        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate<endDate;
        }
        private bool IsSessionAvailableForUpdate(Session session)
        {
            if(session is null) return false;

            if(session.EndDate < DateTime.Now) return false;

            if(session.StartDate <= DateTime.Now) return false;

            var hasActivBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if(!hasActivBookings) return false;

            return true;
        }
        private bool IsSessionAvailableForDelete(Session session)
        {
            if(session is null) return false;
            if(session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false ;

            if(session.StartDate > DateTime.Now) return false;

            var hasActivBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if(!hasActivBookings) return false;

            return true;
        }
        #endregion
    }
}
