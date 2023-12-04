using DTOs;
using DataAccess.Mapper;
using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class ReservationCrudFactory : CrudFactory<Reservation>
    {
        private readonly ReservationMapper _mapper;
        protected SqlDao _dao;
        public ReservationCrudFactory()
        {
            _mapper = new ReservationMapper();
            _dao = SqlDao.GetInstance();
        }
        public override void Create(Reservation dto)
        {
            var sqlOperation = new SqlOperation("CREATE_RESERVATION_PR");
            sqlOperation.AddParameter("@P_START_DATE", dto.StartDate);
            sqlOperation.AddParameter("@P_END_DATE", dto.EndDate);
            sqlOperation.AddParameter("@P_USER_ID", dto.UserID);
            sqlOperation.AddParameter("@P_PET_ID", dto.PetId);
            sqlOperation.AddParameter("@P_PACKAGE_ID", dto.PackageId);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.ReservationCreatedDate);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ReservationModifiedDate);
            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;

        }
        public override void Update(Reservation dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_RESERVATION_PR");
            sqlOperation.AddParameter("@P_RESERVATION_ID", dto.Id);
            sqlOperation.AddParameter("@P_START_DATE", dto.StartDate);
            sqlOperation.AddParameter("@P_END_DATE", dto.EndDate);
            sqlOperation.AddParameter("@P_USER_ID", dto.UserID);
            sqlOperation.AddParameter("@P_PET_ID", dto.PetId);
            sqlOperation.AddParameter("@P_PACKAGE_ID", dto.PackageId);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ReservationModifiedDate);
            _dao.ExecuteProcedure(sqlOperation);
        }
        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_RESERVATION_PR");
            sqlOperation.AddParameter("@P_RESERVATION_ID", id);
            _dao.ExecuteProcedure(sqlOperation);
        }
        public override Reservation RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_RESERVATION_BY_ID_PR");
            sqlOperation.AddParameter("@P_RESERVATION_ID", id);
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var dto = _mapper.BuildObject(lstResult[0]);
                return dto;
            }
            return null;
        }
        public override List<Reservation>? RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_RESERVATIONS_PR");
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var lstServices = _mapper.BuildObjects(lstResult);
                return lstServices;
            }
            return null;
        }


    }
}
