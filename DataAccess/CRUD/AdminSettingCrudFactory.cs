using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class AdminSettingCrud
    {
        protected SqlDao _dao;

        public AdminSettingCrud()
        {
            _dao = SqlDao.GetInstance();
        }

        public string Retrieve(string name)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ADMIN_SETTING_BY_NAME_PR");
            sqlOperation.AddParameter("@P_NAME", name);

            var result = _dao.ExecuteQueryProcedure(sqlOperation);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0]["VALUE"].ToString();
        }

        public void Update(string name, string value)
        {
            var sqlOperation = new SqlOperation("UPDATE_ADMIN_SETTING_PR");
            sqlOperation.AddParameter("@P_NAME", name);
            sqlOperation.AddParameter("@P_VALUE", value);
            _dao.ExecuteProcedure(sqlOperation);
        }
    }
}