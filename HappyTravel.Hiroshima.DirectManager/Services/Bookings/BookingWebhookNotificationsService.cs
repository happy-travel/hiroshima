using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.HttpClients;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Options;
using HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.DirectManager.Services.Bookings
{
    public class BookingWebhookNotificationsService : IBookingWebhookService
    {
        public BookingWebhookNotificationsService(BookingWebhookClient bookingWebhookClient, IOptions<BookingWebhookOptions> bookingWebhookOptions, IOptions<JsonOptions> jsonOptions)
        {
            _bookingWebhookClient = bookingWebhookClient;
            _bookingWebhookOptions = bookingWebhookOptions.Value;
            _jsonOptions = jsonOptions.Value;
        }

        
        public Task<Result> Send(string bookingReferenceCode, BookingStatuses bookingStatus)
        {
            var webhookData = CreateWebhookData(bookingReferenceCode, bookingStatus);

            return _bookingWebhookClient.Send(webhookData);
        }


        public async Task<Result<BookingWebhookData>> Get(Stream stream)
        {
            using var readStream = new StreamReader(stream, Encoding.UTF8);
            var json = await readStream.ReadToEndAsync();
            if (string.IsNullOrEmpty(json))
                return Result.Failure<BookingWebhookData>("Request body is empty");
                    
            var webhookData = JsonSerializer.Deserialize<BookingWebhookData>(json, _jsonOptions.JsonSerializerOptions) ;
            
            return IsWebhookDataValid(webhookData)
                ? Result.Success(webhookData)
                : Result.Failure<BookingWebhookData>("Invalid webhook signature");
        }
        
        private BookingWebhookData CreateWebhookData(string bookingReferenceCode, BookingStatuses bookingStatus)
        {
            var timestamp = CalculateTimestamp();

            return new BookingWebhookData
            {
                Data = new BookingStatusData
                {
                    Status = bookingStatus,
                    ReferenceCode = bookingReferenceCode
                },
                Signature = CalculateSignature(timestamp),
                Timestamp = timestamp
            };
        }
        

        private string CalculateSignature(long timestamp)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_bookingWebhookOptions.Key);
            var messageBytes = Encoding.ASCII.GetBytes(_bookingWebhookOptions.Key + timestamp);
            using var hmac = new HMACSHA256(keyBytes);
            var computedHash = hmac.ComputeHash(messageBytes);

            return ConvertHashToString(computedHash);
        }


        private bool IsWebhookDataValid(BookingWebhookData webhookData) => IsSignatureValid(webhookData.Signature, webhookData.Timestamp);
        
        
        private bool IsSignatureValid(string signature, long timestamp)
        {
            var keyBytes = Encoding.ASCII.GetBytes(_bookingWebhookOptions.Key);
            var messageBytes = Encoding.ASCII.GetBytes(_bookingWebhookOptions.Key + timestamp);
            using var hmac = new HMACSHA256(keyBytes);
            var computedHash = hmac.ComputeHash(messageBytes);
            var calculatedSignature = ConvertHashToString(computedHash);
            
            return signature.Equals(calculatedSignature);
        }

        
        private long CalculateTimestamp() => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        
        
        private static string ConvertHashToString(byte[] hash) =>
            BitConverter.ToString(hash).Replace("-", "").ToLower();

        
        private readonly BookingWebhookOptions _bookingWebhookOptions;
        private readonly BookingWebhookClient _bookingWebhookClient;
        private readonly JsonOptions _jsonOptions;
    }
}