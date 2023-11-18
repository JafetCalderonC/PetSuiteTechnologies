using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public abstract class Mapper<T>
    {
        public abstract T BuildObject(Dictionary<string, object> row);

        public List<T> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<T>();

            foreach (var item in lstRows)
            {
                var rowDTO = BuildObject(item);
                lstResults.Add(rowDTO);
            }

            return lstResults;
        }
    }
}