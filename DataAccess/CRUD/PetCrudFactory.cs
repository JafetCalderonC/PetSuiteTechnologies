using DataAccess.DAOs;
using DataAccess.Mapper;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class PetCrudFactory : CrudFactory<Pet>
    {
        private readonly PetMapper _mapper;
        protected SqlDao _dao;

        public PetCrudFactory()
        {
            _mapper = new PetMapper();
            _dao = SqlDao.GetInstance();
        }

        public override void Create(Pet dto)
        {

            var sqlOperation = new SqlOperation("CREATE_PET_PR");
            sqlOperation.AddParameter("@P_PET_NAME", dto.PetName);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.Description);
            sqlOperation.AddParameter("@P_AGE", dto.PetAge);   
            sqlOperation.AddParameter("@P_BREED", dto.PetBreedType);
            sqlOperation.AddParameter("@P_AGGRESSIVENESS", dto.PetAggressiveness);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.CreatedDate);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);
            sqlOperation.AddParameter("@P_USER_ID", dto.UserId);

            // Create the package and get the id
            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;
        }

        public override void Update(Pet dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_PET_PR");
            sqlOperation.AddParameter("@P_PET_ID", dto.Id);
            sqlOperation.AddParameter("@P_PET_NAME", dto.PetName);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.Description);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_AGE", dto.PetAge);
            sqlOperation.AddParameter("@P_BREED", dto.PetBreedType);
            sqlOperation.AddParameter("@P_AGGRESSIVENESS", dto.PetAggressiveness);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);

            _dao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_PET_PR");
            sqlOperation.AddParameter("@P_PET_ID", id);
            _dao.ExecuteProcedure(sqlOperation);
        }

        public override List<Pet>? RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_PETS_PR");
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var lstPets = _mapper.BuildObjects(lstResult);
                return lstPets;
            }
            return null;
        }

        public override Pet? RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_PET_BY_ID_PR");
            sqlOperation.AddParameter("@P_PET_ID", id);

            var result = _dao.ExecuteQueryProcedure(sqlOperation);
            if (result.Count == 0)
                return null;

            return _mapper.BuildObject(result[0]);
        }

        public List<Pet>? RetrieveByUserId(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_PETS_BY_USER_ID_PR");
            sqlOperation.AddParameter("@P_USER_ID", id);

            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            return _mapper.BuildObjects(lstResult);
        }

    }
}
