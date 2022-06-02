using Spoofer.ViewModels;

namespace Spoofer.Services.Spoofer
{
    public interface ISpooferService
    {
        void GenerateIQFile(string[] argv);
        void GenerateIQFile(string[] argv, MapViewModel viewModel);
        void TransmitFromFile(MapViewModel viewModel);
        void StopTransmitting(ViewModelBase viewModel);
        void GenerateInOrder(TransmitInOrderViewModel viewModel);
        void TransmitInOrder(TransmitInOrderViewModel viewModel);
    }
}
