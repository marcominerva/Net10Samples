using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddValidation();
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.ProblemDetails is HttpValidationProblemDetails validationProblem)
        {
            context.ProblemDetails.Detail =
                $"Error(s) occurred: {validationProblem.Errors.Values.Sum(x => x.Length)}";
        }

        context.ProblemDetails.Extensions.TryAdd("timestamp", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", app.Environment.ApplicationName);
});

app.MapPost("/api/customers", CustomerEndpoints.Create)
    .Produces(StatusCodes.Status201Created)
    .ProducesValidationProblem();

app.MapGet("/api/stockprices", (CancellationToken cancellationToken) =>
{
    static async IAsyncEnumerable<StockPrice> GetStockPrices([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var stockPrice = new StockPrice(Math.Round(Random.Shared.NextDouble() * 100, 2), DateTime.UtcNow);
            yield return stockPrice;
            await Task.Delay(2000, cancellationToken);
        }
    }

    return TypedResults.ServerSentEvents(GetStockPrices(cancellationToken), eventType: "stockUpdates");
});

app.Run();

public static class CustomerEndpoints
{
    /// <summary>
    /// Creates and adds a new customer in the system.
    /// </summary>
    /// <remarks>Every customer needs an account code. When a new customer is created, it is possible to assign it an existing code. If not provided, a new account code will be automatically generated.</remarks>
    /// <param name="customer">The customer to create.</param>
    /// <response code="201">The customer has been successfully created.</response>
    /// <response code="400">The request is invalid.</response>
    public static IResult Create(CreateCustomerRequest customer)
        => TypedResults.StatusCode(StatusCodes.Status201Created);
}

/// <summary>
/// Represents a customer that can be added to the system.
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>
    /// The first name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// The middle name (if any).
    /// </summary>
    [MaxLength(30)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// The last name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// The email address.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The account code (if existing). If not provided, a new account code will be generated.
    /// </summary>
    public Guid? AccountCode { get; set; }

    /// <summary>
    /// A value indicating whether the customer is active (i.e., can make order).
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The base discount rate applied by default to all orders of this customer (0.0 - 1.0, where 0.1 = 10%).
    /// </summary>
    [Range(0, 1.0D)]
    public double BaseDiscount { get; set; }
}

public record class StockPrice(double Value, DateTime DateTime);