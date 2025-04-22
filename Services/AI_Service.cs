using Microsoft.SemanticKernel;

namespace Systemize.Services
{
    public class AI_Service
    {
        private readonly IKernel _kernel;

        public AI_Service(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ProcessChatAsync(string chat)
        {
            if (string.IsNullOrWhiteSpace(chat))
            {
                throw new ArgumentException("Chat input cannot be null or empty.", nameof(chat));
            }

            // Assuming the agent is a pre-configured semantic function in the kernel
            var agentFunction = _kernel.GetFunction("AgentSkill", "ProcessChat");

            // Invoke the semantic kernel function with the chat input
            var result = await _kernel.RunAsync(chat, agentFunction);

            return result.Result;
        }
    }
}
