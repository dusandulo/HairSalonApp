using System;

namespace HairSalon.Core
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int ClientId { get; set; }
        public int StylistId { get; set; }
        public string ServiceType { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Notes { get; set; }

        public Appointment()
        {
            CreatedAt = DateTime.Now;
            Status = "Scheduled";
        }
    }
}