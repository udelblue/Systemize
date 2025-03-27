using Systemize.Models;
using Systemize.Models.ViewModel;


namespace Systemize.Services
{
    public interface IProcess
    {
        Workflow Execute(Workflow workflow, ActionResponse response);
    }
}
