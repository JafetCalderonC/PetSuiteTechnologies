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
        private readonly UserMapper _mapper;
        protected SqlDao _dao;
        public ServiceCrudFactory()
        {
            _mapper = new UserMapper();
            _dao = SqlDao.GetInstance();
        }

    }
}
