using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Options;
using HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.HttpClients
{
    public class BookingWebhookClient
    {
        public BookingWebhookClient(HttpClient httpClient, IOptions<BookingWebhookOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }


        public async Task<Result> Send(BookingWebhookData webhookData)
        {
            var serializedData = JsonSerializer.Serialize(webhookData);
            var error = $"Failed to execute a request to {_options.WebhookUrl}";
            try
            {
                var message = new HttpRequestMessage(HttpMethod.Post, _options.WebhookUrl)
                {
                    Content = new StringContent(serializedData)
                };
                
                var response = await _httpClient.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                    return Result.Failure($"{error} {nameof(response.StatusCode)} '{response.StatusCode}'");
            }
            catch(Exception ex)
            {
                return Result.Failure($"{error} {ex}");
            }

            return Result.Success();
        }


        private readonly HttpClient _httpClient;
        private readonly BookingWebhookOptions _options;
    }
}