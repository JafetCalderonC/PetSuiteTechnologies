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
                Id = (int)row["service_id"],
                ServiceName = (string)row["service_name"],
                ServiceDescription = (string)row["service_description"],
                ServiceStatus = (int)row["service_status"],
                ServiceCost = (decimal)row["service_cost"],
                ServiceCreatedDate = (DateTime)row["service_created_date"],
                ServiceModifiedDate = (DateTime)row["service_modified_date"]
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
