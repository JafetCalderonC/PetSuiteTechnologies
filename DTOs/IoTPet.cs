using System;

namespace DTOs
{
    public class IoTPet : BaseDTO
    {
        public int PetID { get; set; }
        public float? Temperature { get; set; }
        public double? Gas { get; set; }
        public double? Humidity { get; set; }
        public double? Pressure { get; set; }
        public double? Altitude { get; set; }      
        public DateTime? Created { get; set; }
        public int? ContadorDePasos { get; set; }
        public int? PulseRate { get; set; }
        public float? Ligth { get; set; }

       
    }
}

