using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public abstract class CrudFactory<T>
    {
        public abstract void Create(T dto);

        public abstract void Update(T dto);

        public abstract void Delete(int id);

        public abstract T? RetrieveById(int id);

        public abstract List<T>? RetrieveAll();
    }
}
