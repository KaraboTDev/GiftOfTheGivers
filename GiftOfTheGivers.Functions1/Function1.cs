using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GiftOfTheGivers.Functions1
{
    // This class holds our Azure Function.
    // An Azure Function is just a small piece of code that runs
    // when something triggers it - in our case, an HTTP request (like a web form submitting).
    public class TaxCertificateFunction
    {
        // _logger lets us write messages to the console/logs while the function runs,
        // useful for debugging (you'll see these messages when you run it locally).
        private readonly ILogger _logger;

        // Constructor - Azure Functions automatically creates this logger for us
        // and passes it in when the function starts up.
        public TaxCertificateFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TaxCertificateFunction>();
        }

        // [Function("GenerateTaxCertificate")] gives this function a name.
        // This name becomes part of the URL used to call it, e.g.:
        // http://localhost:7071/api/GenerateTaxCertificate
        [Function("GenerateTaxCertificate")]
        public async Task<HttpResponseData> Run(
            // [HttpTrigger(...)] means: "run this method when an HTTP POST request comes in".
            // AuthorizationLevel.Anonymous means no security key is required to call it (fine for testing).
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // Log a message so we can see in the console that the function actually ran.
            _logger.LogInformation("Generating dummy tax certificate...");

            // Read the raw text (JSON) sent in the body of the request.
            // This is the donation info the web app will send us.
            var body = await new StreamReader(req.Body).ReadToEndAsync();

            // Convert ("deserialize") that raw JSON text into a C# object
            // using the DonationRequest class defined below.
            var donation = JsonSerializer.Deserialize<DonationRequest>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Basic safety check: if no data was sent, or the amount is invalid,
            // return an error response (HTTP 400 Bad Request) instead of crashing.
            if (donation == null || donation.Amount <= 0)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid donation data.");
                return badResponse;
            }

            // Build a "dummy" (fake, for demo purposes) tax certificate object.
            // In a real system this might generate a PDF - here we just create
            // a simple object with certificate details.
            var certificate = new
            {
                // Creates a unique certificate number using the current date/time,
                // e.g. GOTG-TAX-20260922143012
                CertificateNumber = $"GOTG-TAX-{DateTime.UtcNow:yyyyMMddHHmmss}",

                DonorName = donation.DonorName,
                Amount = donation.Amount,

                // Today's date in yyyy-MM-dd format, e.g. 2026-09-22
                DateIssued = DateTime.UtcNow.ToString("yyyy-MM-dd"),

                // A friendly thank-you message including the donor's name and amount
                Message = $"Thank you {donation.DonorName}, your donation of R{donation.Amount} " +
                          $"has been recorded for tax purposes."
            };

            // Create a successful HTTP response (200 OK)
            var response = req.CreateResponse(HttpStatusCode.OK);

            // Tell whoever called us that we're sending back JSON data
            response.Headers.Add("Content-Type", "application/json");

            // Convert our certificate object into JSON text and write it into the response body
            await response.WriteStringAsync(JsonSerializer.Serialize(certificate));

            // Send the response back
            return response;
        }
    }

    // This class describes the shape of the data we EXPECT to receive
    // from the web app when a donation is submitted:
    // just a donor name and an amount.
    public class DonationRequest
    {
        public string DonorName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}