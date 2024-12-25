using System;

namespace HairSalon.Core
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsActive { get; set; }

        public Service()
        {
            IsActive = true;
        }

        public Service(string name, string description, decimal price, TimeSpan duration)
        {
            Name = name;
            Description = description;
            Price = price;
            Duration = duration;
            IsActive = true;
        }
    }
}