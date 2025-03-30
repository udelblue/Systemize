using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;


namespace Systemize.Services
{
    public interface IActionProcess
    {
        Workflow Execute(Workflow workflow, ActionResponse response);
    }
}
