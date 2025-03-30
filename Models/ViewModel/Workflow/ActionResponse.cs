namespace Systemize.Models.ViewModel.Workflow
{
    public class ActionResponse
    {

        public string ActionType { get; set; }
        public string? Executor { get; set; }
        public Dictionary<string, string> Data { get; set; }

    }
}
