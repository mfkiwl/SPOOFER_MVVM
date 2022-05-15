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
        public double? Longitude => Math.Round((double)_coordinates.Longitude, 5);
        public double? Latitude => Math.Round((double)_coordinates.Latitude, 5);
        public double? Height => Math.Round((double)_coordinates.Height, 5);
        public int? NumberInOrder => _coordinates.NumberInOrder;
        public bool HasFile => _coordinates.HasFile;

    }
}
