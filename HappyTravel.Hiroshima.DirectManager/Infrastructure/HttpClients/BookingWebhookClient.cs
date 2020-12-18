using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Logging;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Options;
using HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.DirectManager.Infrastructure.HttpClients
{
    public class BookingWebhookClient
    {
        public BookingWebhookClient(HttpClient httpClient, IOptions<BookingWebhookOptions> webhookOptions, IOptions<JsonOptions> jsonOptions, ILogger<BookingWebhookClient> logger)
        {
            _httpClient = httpClient;
            _webhookOptions = webhookOptions.Value;
            _jsonOptions = jsonOptions.Value;
            _logger = logger;
        }


        public async Task<Result> Send(BookingWebhookData webhookData)
        {
            var serializedData = JsonSerializer.Serialize(webhookData, _jsonOptions.JsonSerializerOptions);
            var error = $"Failed to execute a request to {_webhookOptions.WebhookUrl}";

            var message = new HttpRequestMessage(HttpMethod.Post, _webhookOptions.WebhookUrl)
            {
                Content = new StringContent(serializedData)
            };

            var response = await _httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
                return Result.Failure($"{error} {nameof(response.StatusCode)} '{response.StatusCode}'");

            return Result.Success();
        }


        private readonly HttpClient _httpClient;
        private readonly BookingWebhookOptions _webhookOptions ;
        private readonly JsonOptions _jsonOptions;
        private readonly ILogger<BookingWebhookClient> _logger;
    }
}