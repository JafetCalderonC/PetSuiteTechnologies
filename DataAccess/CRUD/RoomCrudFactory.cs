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
    public class RoomCrudFactory : CrudFactory<Room>
    {
        private readonly RoomMapper _mapper;
        protected SqlDao _dao;

        public RoomCrudFactory()
        {
            _mapper = new RoomMapper();
            _dao = SqlDao.GetInstance();
        }

        public override void Create(Room dto)
        {
            var sqlOperation = new SqlOperation("CREATE_ROOM_PR");
            sqlOperation.AddParameter("@P_ROOM_NAME", dto.Name);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.Description);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_COST", dto.Cost);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.CreatedDate);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);

           _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_ROOM_PR");
            sqlOperation.AddParameter("@P_ROOM_ID", id);

            _dao.ExecuteProcedure(sqlOperation);
        }

        public override List<Room> RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_ROOMS_PR");
            var result = _dao.ExecuteQueryProcedure(sqlOperation);
            
            return _mapper.BuildObjects(result);
        }

        public override Room RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ROOM_BY_ID_PR");
            sqlOperation.AddParameter("@P_ROOM_ID", id);

            var result = _dao.ExecuteQueryProcedure(sqlOperation);
            if (result.Count == 0)
                return null;

            return _mapper.BuildObject(result[0]);
        }

        public List<Room> RetrieveAvailable()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_AVAILABLE_ROOMS_PR");
            var result = _dao.ExecuteQueryProcedure(sqlOperation);

            return _mapper.BuildObjects(result);
        }

        public override void Update(Room dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_ROOM_PR");
            sqlOperation.AddParameter("@P_ROOM_ID", dto.Id);
            sqlOperation.AddParameter("@P_ROOM_NAME", dto.Name);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.Description);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_COST", dto.Cost);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);

            _dao.ExecuteProcedure(sqlOperation);
        }
    }
}
