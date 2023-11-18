using DataAccess.DAOs;
using DataAccess.Mapper;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class ServiceCrudFactory : CrudFactory <Service>
    {
        private readonly ServiceMapper _mapper;
        protected SqlDao _dao;
        public ServiceCrudFactory()
        {
            _mapper = new ServiceMapper();
            _dao = SqlDao.GetInstance();
        }
        public override void Create(Service dto)
        {
            var sqlOperation = new SqlOperation("CREATE_SERVICE_PR");
            sqlOperation.AddParameter("@P_SERVICE_NAME", dto.ServiceName);
            sqlOperation.AddParameter("@P_STATUS", dto.ServiceStatus);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.ServiceDescription);
            sqlOperation.AddParameter("@P_COST", dto.ServiceCost);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.ServiceCreatedDate);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ServiceModifiedDate);
            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;

        }
        public override void Update(Service dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_SERVICE_PR");
            sqlOperation.AddParameter("@P_SERVICE_ID", dto.Id);
            sqlOperation.AddParameter("@P_SERVICE_NAME", dto.ServiceName);
            sqlOperation.AddParameter("@P_STATUS", dto.ServiceStatus);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.ServiceDescription);
            sqlOperation.AddParameter("@P_COST", dto.ServiceCost);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ServiceModifiedDate);
            _dao.ExecuteProcedure(sqlOperation);
        }
        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_SERVICE_PR");
            sqlOperation.AddParameter("@P_SERVICE_ID",id);
            _dao.ExecuteProcedure(sqlOperation);
        }
        public override Service RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_SERVICE_BY_ID_PR");
            sqlOperation.AddParameter("@P_SERVICE_ID", id);
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var dto = _mapper.BuildObject(lstResult[0]);
                return dto;
            }
            return null;
        }
        public override List<Service>? RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_SERVICES_PR");
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
/*        @P_SERVICE_ID INT */ 