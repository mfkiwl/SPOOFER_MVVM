using Spoofer.Models;
using System;

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
        public double? Longitude => Math.Round((double)_coordinates.Longitude, 3);
        public double? Latitude => Math.Round((double)_coordinates.Latitude, 3);
        public double? Height => Math.Round((double)_coordinates.Height, 3);
        public int? NumberInOrder => _coordinates.NumberInOrder;
        public bool HasFile => _coordinates.HasFile;

    }
}
