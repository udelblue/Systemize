namespace Systemize.Models.ViewModel
{
    public class ActionResponse
    {

        public String ActionType { get; set; }
        public String? Executor { get; set; }
        public Dictionary<string, string> Data { get; set; }

    }
}
