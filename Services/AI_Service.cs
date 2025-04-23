using Microsoft.SemanticKernel;


namespace Systemize.Services
{
    public class AI_Service
    {



        public async Task<string> ProcessChatWithHistoryAsync(string chat, string chatHistory)
        {
            if (string.IsNullOrWhiteSpace(chat))
            {
                throw new ArgumentException("Chat input cannot be null or empty.", nameof(chat));
            }
            // Step 1: Create the Kernel Builder
            var kernelBuilder = Kernel.CreateBuilder();

            // Step 2: Add OpenAI API to the Kernel
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: "gpt-4o", // Specify the OpenAI model to use
                apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? ""
            );

            // Step 3: Build the Kernel
            var kernel = kernelBuilder.Build();

            // Step 4: Define a Semantic Function
            const string promptTemplate = @"
            You are a helpful assistant. Answer the following question:
            Question: {{$input}}
            Answer:";

            var semanticFunction = kernel.CreateFunctionFromPrompt(
                promptTemplate,
                functionName: "AnswerPrompt"
            );



            // Step 5: setup kernel arguements
            var kernelArguements = new KernelArguments();
            kernelArguements["input"] = chat;

            // Step 6: Execute the Semantic Function
            var result = await semanticFunction.InvokeAsync(kernel, kernelArguements);

            // Step 7: Display the Output
            Console.WriteLine("\nResponse from Semantic Kernel:");
            Console.WriteLine(result.ToString());

            return result.ToString();
        }
    }
}
