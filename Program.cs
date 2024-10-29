using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion("gpt-4o",
    "https://luiscocoaiservice.openai.azure.com/",
    "",
    "gpt-4o");

builder.Plugins.AddFromType<TimeInformation>();

var kernel = builder.Build();

// Example 1. Invoke the kernel with a prompt that asks the AI for information it cannot provide and may hallucinate
Console.WriteLine(await kernel.InvokePromptAsync("How many days until Christmas?"));

//// Example 2. Invoke the kernel with a templated prompt that invokes a plugin and display the result
Console.WriteLine(await kernel.InvokePromptAsync("The current time is {{TimeInformation.GetCurrentUtcTime}}. How many days until Christmas?"));

#pragma warning disable
// Example 3. Invoke the kernel with a prompt and allow the AI to automatically invoke functions
OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
Console.WriteLine(await kernel.InvokePromptAsync("How many days until Christmas? Explain your thinking.", new(settings)));

public class TimeInformation
{
    [KernelFunction]
    [Description("Retrieves the current time in UTC.")]
    public string GetCurrentUtcTime() => DateTime.UtcNow.ToString("R");
}