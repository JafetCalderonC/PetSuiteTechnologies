using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class ServiceMapper : Mapper<Service>
    {
        public override Service BuildObject(Dictionary<string, object> row)
        {
            var service = new Service
            {
                Id = Convert.ToInt32(row["service_id"]),
                ServiceName = (string)row["service_name"],
                ServiceDescription = (string)row["description"],
                ServiceStatus = Convert.ToInt32(row["status"]),
                ServiceCost = Convert.ToDecimal(row["cost"]),
                ServiceCreatedDate = (DateTime)row["created_date"],
                ServiceModifiedDate = (DateTime)row["modified_date"]
            };
            return service;
        }

        public List<Service> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<Service>();

            foreach (var item in lstRows)
            {
                var service = BuildObject(item);
                lstResults.Add(service);
            }

            return lstResults;
        }   
    }
}
