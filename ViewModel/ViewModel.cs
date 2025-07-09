using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemantickKerenelDesktopApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SemantickKerenelDesktopApp.ViewModel
{
    public enum AIProvider
    {
        AzureOpenAI,
        OpenAI,
       
    }
    public class ViewModel :INotifyPropertyChanged
    {
   
        public event PropertyChangedEventHandler? PropertyChanged;

        private AIProvider _provider = AIProvider.AzureOpenAI;

        private Kernel? _kernel;

        public OpenAIPromptExecutionSettings OpenAIPromptExecutionSettings => new()
        {
            MaxTokens = 800,
            //Temperature = 0.7,
            //TopP = 0.95,
            //FrequencyPenalty = 0,
            //PresencePenalty = 0,
            //StopSequences = ["###"],
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        public ChatHistory ChatHistory { get; set; } =
        [
        new ChatMessageContent(AuthorRole.System, "You are an AI assistant about family members. If you are asked something else, say 'I do not know.'")
        ]; 

        public Kernel? Kernel
        {
            get => _kernel;
            private set
            {
                _kernel = value;
                OnPropertyChanged();
            }
        }

        public IChatCompletionService? ChatCompletionService { get; private set; }

        public bool CanBuildKernel =>
            Provider switch
            {
                AIProvider.OpenAI => !string.IsNullOrWhiteSpace(ApiKey)
                                     && !string.IsNullOrWhiteSpace(DeploymentName),
                AIProvider.AzureOpenAI => !string.IsNullOrWhiteSpace(ApiKey)
                                          && !string.IsNullOrWhiteSpace(DeploymentName)
                                          && !string.IsNullOrWhiteSpace(Endpoint),
                _ => false
            };
        public AIProvider Provider
        {
            get => _provider;
            set
            {
                if (_provider != value)
                {
                    _provider = value;
                    OnPropertyChanged();
                    TryBuildKernel();
                }
            }
        }

        private string _apiKey = string.Empty;
        public string ApiKey
        {
            get => _apiKey;
            set
            {
                if (_apiKey != value)
                {
                    _apiKey = value;
                    OnPropertyChanged();
                    TryBuildKernel();
                }
            }
        }

        private string _endpoint = string.Empty;
        public string Endpoint
        {
            get => _endpoint;
            set
            {
                if (_endpoint != value)
                {
                    _endpoint = value;
                    OnPropertyChanged();
                    TryBuildKernel();
                }
            }
        }

        private string _deploymentName = string.Empty;
        public string DeploymentName
        {
            get => _deploymentName;
            set
            {
                if (_deploymentName != value)
                {
                    _deploymentName = value;
                    OnPropertyChanged();
                    TryBuildKernel();
                }
            }
        }

        private string _userInput = string.Empty;

        public string UserInput
        {
            get => _userInput;
            set
            {
                if(value != _userInput)
                {
                    _userInput = value;
                    OnPropertyChanged();
                }
            }
        }

        private ChatMessageContent _response;

        public ChatMessageContent Response
        { 
            get => _response;
            set
            {
                if (_response != value)
                {
                    _response = value;
                    OnPropertyChanged();

                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TryBuildKernel()
        {
            if (!CanBuildKernel)
                return;

            var builder = Kernel.CreateBuilder();

            if (Provider == AIProvider.AzureOpenAI)
            {
                builder.AddAzureOpenAIChatCompletion(
                    deploymentName: DeploymentName,
                    endpoint: Endpoint,
                    apiKey: ApiKey
                );
            }
            else if (Provider == AIProvider.OpenAI)
            {
                builder.AddOpenAIChatCompletion(
                    modelId: DeploymentName,
                    apiKey: ApiKey
                );
            }

            Kernel = builder.Build();
            ChatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
            Kernel.Plugins.AddFromType<LightsPlugin>("Lights");
           // Kernel.Plugins.AddFromType<FamilyPlugin>("Family");
            Kernel.Plugins.AddFromType<FamilyPlugin>("Family");

        }
    }
}
