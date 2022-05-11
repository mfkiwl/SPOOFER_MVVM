using Spoofer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.ViewModels
{
    public sealed class CoordinateViewModel : ViewModelBase
    {
        private readonly Coordinates _coordinates;
        public CoordinateViewModel(Coordinates coordinates)
        {
            _coordinates = coordinates;
        }
        public string Name => _coordinates.Name;
        public double? Longitude => _coordinates.Longitude;
        public double? Latitude => _coordinates.Latitude;
        public double? Height => _coordinates.Height;
        public int? NumberInOrder => _coordinates.NumberInOrder;
        public bool HasFile => _coordinates.HasFile;


    }
}
