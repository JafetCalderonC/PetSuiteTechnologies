using DataAccess.DAOs;
using DataAccess.Mapper;
using DTOs;
using System;
using System.Collections.Generic;

namespace DataAccess.CRUD
{
    public class IoTPetCrudFactory : CrudFactory<IoTPet>
    {
        private readonly IoTPetMapper _mapper;
        protected SqlDao _dao;

        public IoTPetCrudFactory()
        {
            _mapper = new IoTPetMapper();
            _dao = SqlDao.GetInstance();
        }

        public override void Create(IoTPet dto)
        {
            var sqlOperation = new SqlOperation("CREATE_IOT_PET_RECORD_PR");
            sqlOperation.AddParameter("@P_IOT_ID", dto.PetID);
            sqlOperation.AddParameter("@P_TEMPERATURE", dto.Temperature);
            sqlOperation.AddParameter("@P_GAS", dto.Gas);
            sqlOperation.AddParameter("@P_HUMIDITY", dto.Humidity);
            sqlOperation.AddParameter("@P_PRESSURE", dto.Pressure);
            sqlOperation.AddParameter("@P_ALTITUDE", dto.Altitude);
            sqlOperation.AddParameter("@P_CONTADOR_DE_PASOS", dto.ContadorDePasos);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.Created);
            sqlOperation.AddParameter("@P_PET_ID", dto.PetID);
            sqlOperation.AddParameter("@P_PULSE_RATE", dto.PulseRate);
            sqlOperation.AddParameter("@P_LIGHT", dto.Ligth);


            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;      
        }

        public override void Update(IoTPet dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_IOT_PET_RECORD_PR");
            sqlOperation.AddParameter("@P_IOT_ID", dto.PetID);
            sqlOperation.AddParameter("@P_TEMPERATURE", dto.Temperature);
            sqlOperation.AddParameter("@P_GAS", dto.Gas);
            sqlOperation.AddParameter("@P_HUMIDITY", dto.Humidity);
            sqlOperation.AddParameter("@P_PRESSURE", dto.Pressure);
            sqlOperation.AddParameter("@P_ALTITUDE", dto.Altitude);
            sqlOperation.AddParameter("@P_CONTADOR_DE_PASOS", dto.ContadorDePasos);
            sqlOperation.AddParameter("@P_PET_ID", dto.PetID);
            sqlOperation.AddParameter("@P_PULSE_RATE", dto.PulseRate);
            sqlOperation.AddParameter("@P_LIGHT", dto.Ligth);
            _dao.ExecuteProcedure(sqlOperation);         
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_IOT_PET_RECORD_BY_IOT_ID_PR");
            sqlOperation.AddParameter("@P_IOT_ID", id);
            _dao.ExecuteProcedure(sqlOperation);
        }

        public override IoTPet RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("IOT_PET_RECORDS_RetrieveById");
            sqlOperation.AddParameter("@P_IOT_ID", id);
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var dto = _mapper.BuildObject(lstResult[0]);
                return dto;
            }
            return null;
        }

        public IoTPet RetrieveByPetId(int id)
        {
            var sqlOperation = new SqlOperation("IOT_PET_RECORDS_RetrieveByPetId");
            sqlOperation.AddParameter("@P_PET_ID", id);
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var dto = _mapper.BuildObject(lstResult[0]);
                return dto;
            }
            return null;
        }

        public override List<IoTPet> RetrieveAll()
         {
             var sqlOperation = new SqlOperation("IOT_PET_RECORDS_RetrieveAll");
             var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
             return _mapper.BuildObjects(lstResult);
         }
    }
}