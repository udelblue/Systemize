using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;


namespace Systemize.Services
{
    public interface IAsyncActionProcess
    {
        Task<Workflow> ExecuteAsync(Workflow workflow, ActionResponse response);
    }
}
