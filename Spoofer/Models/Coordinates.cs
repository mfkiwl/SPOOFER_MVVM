﻿// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

using System;

namespace Spoofer.Models
{

    public partial class Coordinates : IEntityWithId
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Height { get; set; }
        public int? NumberInOrder { get; set; }
        public bool HasFile { get; set; }
        public string UserId { get; set; }
        public DateTime? GenerationDate { get; set; }

        public virtual User User { get; set; }
    }
}