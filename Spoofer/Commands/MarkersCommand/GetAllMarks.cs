using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using System;

namespace Spoofer.Commands.MarkersCommand
{
    public class GetAllMarks : BaseCommand
    {
        private readonly IMarkerService _service;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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
            try
            {
                _service.GetAll();
                log.Info("Get All Coordinates");
            }
            catch(Exception e)
            {
                log.Error("There is no Coordinates in Database", e);
            }
        }
    }
}