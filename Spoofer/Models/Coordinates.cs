﻿// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spoofer.Models
{
    public partial class Coordinates
    {
        public string CoorfianteId { get; set; }
        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Height { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}