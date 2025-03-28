using Systemize.Models;
using Systemize.Models.ViewModel;


namespace Systemize.Services
{
    public interface IActionProcess
    {
        Workflow Execute(Workflow workflow, ActionResponse response);
    }
}
