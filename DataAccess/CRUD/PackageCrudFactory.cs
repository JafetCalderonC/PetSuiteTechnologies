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
    public class PackageCrudFactory : CrudFactory<Package>
    {
        private readonly PackageMapper _mapper;
        protected SqlDao _dao;

        public PackageCrudFactory()
        {
            _mapper = new PackageMapper();
            _dao = SqlDao.GetInstance();
        }

        public override void Create(Package dto)
        {

            var sqlOperation = new SqlOperation("CREATE_PACKAGE_PR");
            sqlOperation.AddParameter("@P_PACKAGE_NAME", dto.PackageName);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.Description);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_ROOM_ID", dto.RoomId);
            sqlOperation.AddParameter("@P_PET_BREED_TYPE", dto.PetBreedType);   
            sqlOperation.AddParameter("@P_PET_SIZE", dto.PetSize);
            sqlOperation.AddParameter("@P_PET_AGGRESSIVENESS", dto.PetAggressiveness);
            sqlOperation.AddParameter("@P_CREATED_DATE", dto.CreatedDate);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);

            // Create the package and get the id
            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;

            // Add services to package
            foreach (var servivces in dto.Services)
            {
                sqlOperation = new SqlOperation("ADD_SERVICE_TO_PACKAGE_PR");
                sqlOperation.AddParameter("@P_PACKAGE_ID", dto.Id);
                sqlOperation.AddParameter("@P_SERVICE_ID", servivces);
                _dao.ExecuteProcedure(sqlOperation);
            }
        }

        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_PACKAGE_PR");
            sqlOperation.AddParameter("@P_PACKAGE_ID", id);
            _dao.ExecuteProcedure(sqlOperation);
        }

        public override List<Package>? RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_PACKAGES_PR");
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var lstPackages = _mapper.BuildObjects(lstResult);
                return lstPackages;
            }
            return null;
        }

        public override Package? RetrieveById(int id)
        {
            //var sqlOperation = new SqlOperation("RETRIEVE_USER_BY_ID_PR");
            //sqlOperation.AddParameter("@P_USER_ID", id);

            //var result = _dao.ExecuteQueryProcedure(sqlOperation);
            //if (result.Count == 0)
            //    return null;

            //return _mapper.BuildObject(result[0]);
            return null;
        }

        public override void Update(Package dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_PACKAGE_PR");
            sqlOperation.AddParameter("@P_PACKAGE_ID", dto.Id);
            sqlOperation.AddParameter("@P_PACKAGE_NAME", dto.PackageName);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_DESCRIPTION", dto.Description);
            sqlOperation.AddParameter("@P_ROOM_ID", dto.RoomId);
            sqlOperation.AddParameter("@P_PET_BREED_TYPE", dto.PetBreedType);
            sqlOperation.AddParameter("@P_PET_SIZE", dto.PetSize);
            sqlOperation.AddParameter("@P_PET_AGGRESSIVENESS", dto.PetAggressiveness);
            sqlOperation.AddParameter("@P_MODIFIED_DATE", dto.ModifiedDate);

            _dao.ExecuteProcedure(sqlOperation);

            // Get phone numbers of the user
            sqlOperation = new SqlOperation("RETRIEVE_PACKAGE_SERVICES_BY_PACKAGE_ID_PR");
            sqlOperation.AddParameter("@P_PACKAGE_ID", dto.Id);
            var resultServices = _dao.ExecuteQueryProcedure(sqlOperation);

            if (dto.Services == null)
            {
                return;
            }

            // If the phone number is not in the database, add it
            foreach (var services in dto.Services)
            {
                if (!resultServices.Any(p => p["service_id"].ToString() == services))
                {
                    sqlOperation = new SqlOperation("ADD_SERVICE_TO_PACKAGE_PR");
                    sqlOperation.AddParameter("@P_PACKAGE_ID", dto.Id);
                    sqlOperation.AddParameter("@P_SERVICE_ID", services);
                    _dao.ExecuteProcedure(sqlOperation);
                }
            }

            // If the phone number is not in the dto, delete it
            foreach (var phoneNumberRow in resultServices)
            {
                var currentService = phoneNumberRow["service_id"].ToString();
                if (!dto.Services.Any(p => p == currentService))
                {
                    sqlOperation = new SqlOperation("REMOVE_SERVICE_FROM_PACKAGE_PR");
                    sqlOperation.AddParameter("@P_PACKAGE_ID", dto.Id);
                    sqlOperation.AddParameter("@P_SERVICE_ID", currentService);
                    _dao.ExecuteProcedure(sqlOperation);
                }
            }
        }
    }
}
