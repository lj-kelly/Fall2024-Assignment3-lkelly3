using Microsoft.AspNetCore.Mvc;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.DataProtection;
using OpenAI;
using Azure;
using static System.Net.WebRequestMethods;
using OpenAI.Chat;
using System.ClientModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Identity;

using static System.Environment;

namespace Fall2024_Assignment3_lkelly3.Services
{
    public class OpenAIhit
    {
        private readonly ChatClient _chatClient;
        private readonly string openAIKey;
        private readonly string openAIEndpoint;
        public OpenAIhit(IConfiguration config)
        {
            // Fetch key and endpoint from configuration
            openAIKey = config["OpenAIKey"];  // Must match key in appsettings.json
            openAIEndpoint = config["OpenAIEndpoint"];  // Must match endpoint in appsettings.json

            // Check if values were properly retrieved, throw an exception if not
            if (string.IsNullOrEmpty(openAIKey) || string.IsNullOrEmpty(openAIEndpoint))
            {
                throw new ArgumentNullException("OpenAI key or endpoint not provided in configuration.");
            }

            // Initialize the Azure OpenAI client
            AzureOpenAIClient azureClient = new(
                new Uri(openAIEndpoint),
                new ApiKeyCredential(openAIKey));
            var chatClient = azureClient.GetChatClient("gpt-35-turbo");

            // _chatClient is initialized correctly for later use
            _chatClient = chatClient;
        }
        public async Task<List<string>> WriteReviews(string title)
        {

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a movie reviewer. Write 20 short reviews for the given movie, and give each a star rating out of 5. Separate each review with '$$$' so they can be easily parsed."),
                new UserChatMessage($"The movie title is '{title}'. Please write reviews for this movie.")
            };


            var options = new ChatCompletionOptions
            {
                Temperature = (float)0.7,

            };

            try
            {
                List<string> reviews = new List<string>();

                // Make the API request for chat completion
                ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);

                if (completion.Content != null)
                {
                    // Assuming completion.Content contains the full response as text
                    string fullResponse = completion.Content.First().Text;

                    // Split the response based on the '###' separator
                    var reviewArray = fullResponse.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries);

                    // Add each split review to the reviews list
                    for (int i = 0; i < reviewArray.Length; i++)
                    {
                        reviews.Add(reviewArray[i].Trim());  // Trim any extra whitespace
                    }

                    return reviews;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
            
        }
        public async Task<List<string>> WriteTweets(string name)
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You represent the Twitter social media platform. Write 20 short tweets about the given actor. Separate each review with '$$$' so they can be easily parsed."),
                new UserChatMessage($"The actor's name is '{name}'. Please write tweets about this actor.")
            };


            var options = new ChatCompletionOptions
            {
                Temperature = (float)0.7,
                //MaxOutputTokenCount = 100
            };

            try
            {
                List<string> reviews = new List<string>();

                // Make the API request for chat completion
                ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);

                if (completion.Content != null)
                {
                    // Assuming completion.Content contains the full response as text
                    string fullResponse = completion.Content.First().Text;

                    // Split the response based on the '###' separator
                    var reviewArray = fullResponse.Split(new[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);

                    // Add each split review to the reviews list
                    for (int i = 0; i < reviewArray.Length; i++)
                    {
                        reviews.Add(reviewArray[i].Trim());  // Trim any extra whitespace
                    }

                    return reviews;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
