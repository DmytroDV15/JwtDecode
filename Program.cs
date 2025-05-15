using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using JwtDecode.Models;

namespace JwtDecode;

class Program
{
    static void Main(string[] args)
    {
        const string jwtToken = "eyJhbGciOiJub25lIn0.eyJkYXRhIjpbeyJ1c2VySWQiOiIxMjM0NSIsInRyYW5zYWN0aW9ucyI6W3siaWQiOiIxIiwiYW1vdW50Ijo1MCwiY3VycmVuY3kiOiJVQUgiLCJtZXRhIjp7InNvdXJjZSI6IkNBQiIsImNvbmZpcm1lZCI6dHJ1ZX0sInN0YXR1cyI6IkNvbXBsZXRlZCJ9LHsiaWQiOiIyIiwiYW1vdW50IjozMC41LCJjdXJyZW5jeSI6IlVBSCIsIm1ldGEiOnsic291cmNlIjoiQUNCIiwiY29uZmlybWVkIjpmYWxzZX0sInN0YXR1cyI6IkluUHJvZ3Jlc3MifSx7ImlkIjoiMyIsImFtb3VudCI6ODkuOTksImN1cnJlbmN5IjoiVUFIIiwibWV0YSI6eyJzb3VyY2UiOiJDQUIiLCJjb25maXJtZWQiOnRydWV9LCJzdGF0dXMiOiJDb21wbGV0ZWQifV19LHsidXNlcklkIjoidTEyMyIsInRyYW5zYWN0aW9ucyI6W3siaWQiOiIxIiwiYW1vdW50Ijo0NDM0LCJjdXJyZW5jeSI6IkVVUiIsIm1ldGEiOnsic291cmNlIjoiQ0FCIiwiY29uZmlybWVkIjp0cnVlfSwic3RhdHVzIjoiQ29tcGxldGVkIn0seyJpZCI6IjIiLCJhbW91bnQiOjU2LjUzLCJjdXJyZW5jeSI6IlVBSCIsIm1ldGEiOnsic291cmNlIjoiQUNCIiwiY29uZmlybWVkIjpmYWxzZX0sInN0YXR1cyI6Mn1dfV19.";

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

        var dataJson = jwtSecurityToken.Claims
            .Where(claim => claim.Type == "data")
            .Select(c => c.Value);

        var data = new List<UserTransactions>();
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (var itemJson in dataJson)
        {
            var userTransaction = JsonSerializer.Deserialize<UserTransactions>(itemJson, options: jsonSerializerOptions);
            if (userTransaction != null)
            {
                data.Add(userTransaction);
            }
        }

        foreach (var user in data)
        {
            var userId = user.UserId;
            var transactionCount = user.Transactions?.Count ?? 0;
            var totalConfirmedAmountUAH = user.Transactions?
                .Where(t => t.Meta != null && t.Meta.Confirmed)
                .Sum(t =>
                {
                    var currency = t.Currency ?? "UAH";
                    var rate = ExchangeRates.Rates.TryGetValue(currency, out decimal value) ? value : 1m;
                    return Convert.ToDecimal(t.Amount) * rate;
                }) ?? 0;

            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"Transaction count: {transactionCount}");
            Console.WriteLine($"Total confirmed amount (UAH): {totalConfirmedAmountUAH}");
            Console.WriteLine();
        }
    }
}