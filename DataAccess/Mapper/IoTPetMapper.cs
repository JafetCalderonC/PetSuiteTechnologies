using DTOs;
namespace DataAccess.Mapper
{
    public class IoTPetMapper : Mapper<IoTPet>
    {
        public override IoTPet BuildObject(Dictionary<string, object> row)
        {
            var ioTPet = new IoTPet
            {
                Id = Convert.ToInt32(row["iot_id"]),
                PetID = Convert.ToInt32(row["pet_id"]),
                Temperature = row["temperature"] != DBNull.Value ? (float?)Convert.ToSingle(row["temperature"]) : null,
                Gas = row["gas"] != DBNull.Value ? (double?)Convert.ToDouble(row["gas"]) : null,
                Humidity = row["humidity"] != DBNull.Value ? (double?)Convert.ToDouble(row["humidity"]) : null,
                Pressure = row["pressure"] != DBNull.Value ? (double?)Convert.ToDouble(row["pressure"]) : null,
                Altitude = row["altitude"] != DBNull.Value ? (double?)Convert.ToDouble(row["altitude"]) : null,
                Ligth = row["light"] != DBNull.Value ? (float?)Convert.ToDouble(row["light"]) : null,
                PulseRate = row["pulse_rate"] != DBNull.Value ? Convert.ToInt32(row["pulse_rate"]) : null,
                Created = row["created_date"] != DBNull.Value ? (DateTime?)row["created_date"] : null,
                ContadorDePasos = row["ContadorDePasos"] != DBNull.Value ? (int)row["ContadorDePasos"] : null,
            };
                   
            return ioTPet;
        }
    }
}

