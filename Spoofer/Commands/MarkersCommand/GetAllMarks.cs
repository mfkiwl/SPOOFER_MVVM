using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;

namespace Spoofer.Commands.MarkersCommand
{
    public class GetAllMarks : BaseCommand
    {
        private readonly IMarkerService _service;

        public GetAllMarks(IMarkerService service)
        {
            _service = service;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _service.GetAll();
        }
    }
}