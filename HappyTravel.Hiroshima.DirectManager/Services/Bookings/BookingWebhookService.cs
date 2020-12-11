using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.HttpClients;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Options;
using HappyTravel.Hiroshima.DirectManager.Models.Webhooks.Bookings;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.DirectManager.Services.Bookings
{
    public class BookingWebhookService : IBookingWebhookService
    {
        public BookingWebhookService(BookingWebhookClient bookingWebhookClient, IOptions<BookingWebhookOptions> bookingWebhookOptions)
        {
            _bookingWebhookOptions = bookingWebhookOptions.Value;
            _bookingWebhookClient = bookingWebhookClient;
        }

        
        public Task<Result> Send(string bookingReferenceCode, BookingStatuses bookingStatus)
        {
            var webhookData = CreateWebhookData(bookingReferenceCode, bookingStatus);

            return _bookingWebhookClient.Send(webhookData);
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
                Signature = CalculateSignature(timestamp)
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


        public bool IsSignatureValid(string signature, long timestamp)
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
    }
}