namespace Systemize.Models.ViewModel
{
    public class AvailableActions
    {



        public AvailableActions()
        {

        }
        public AvailableActions(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public AvailableActions(string name, string description, string actionType)
        {
            Name = name;
            Description = description;
            ActionType = actionType;
        }

        public AvailableActions(string name, string description, string actionType, string button)
        {
            Name = name;
            Description = description;
            ActionType = actionType;
            Button = button;
        }


        public string Name { get; set; }
        public string? Description { get; set; }

        public string? ActionType { get; set; }


        public string? Button { get; set; } = "btn-primary";


        public string? Feedback { get; set; }
    }
}
