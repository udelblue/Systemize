using Azure;
using Azure.AI.Projects;
using Azure.Identity;
using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;




namespace Systemize.Services.ActionStratagies
{
    public class BasicAIStratagy : IAsyncActionProcess
    {
        private readonly SystemizeContext _context;

        public BasicAIStratagy(SystemizeContext context)
        {
            _context = context;
        }

        public async Task<Workflow> ExecuteAsync(Workflow workflow, ActionResponse response)
        {
            await RunAgentConversation();
            return workflow;
        }

        private async Task RunAgentConversation()
        {
            var connectionString = "eastus.api.azureml.ms;24ca348a-2c2e-432f-b614-7afc13015a3a;AI-Hub;test_project";
            AgentsClient client = new AgentsClient(connectionString, new DefaultAzureCredential());

            Response<Agent> agentResponse = await client.GetAgentAsync("asst_XyZrUgYJO2dUBynwu9qQ285y");
            Agent agent = agentResponse.Value;

            Response<AgentThread> threadResponse = await client.GetThreadAsync("thread_ul0C9RYYMksZk8Ebpjfkh8BX");
            AgentThread thread = threadResponse.Value;

            Response<ThreadMessage> messageResponse = await client.CreateMessageAsync(
                thread.Id,
                MessageRole.User,
                "Hi Agent856");
            ThreadMessage message = messageResponse.Value;

            Response<ThreadRun> runResponse = await client.CreateRunAsync(
                thread.Id,
                agent.Id);
            ThreadRun run = runResponse.Value;

            // Poll until the run reaches a terminal status
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                runResponse = await client.GetRunAsync(thread.Id, runResponse.Value.Id);
            }
            while (runResponse.Value.Status == RunStatus.Queued
                || runResponse.Value.Status == RunStatus.InProgress);

            Response<PageableList<ThreadMessage>> messagesResponse = await client.GetMessagesAsync(thread.Id);
            IReadOnlyList<ThreadMessage> messages = messagesResponse.Value.Data;

            // Display messages
            foreach (ThreadMessage threadMessage in messages)
            {
                Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");

                /* 
                foreach (MessageContent contentItem in threadMessage.ContentItems)
                {
                    if (contentItem is MessageTextContent textItem)
                    {
                        Console.Write(textItem.Text);
                    }
                }
                 */
                Console.WriteLine();
            }
        }
    }

}