namespace JwtDecode;
internal static class ExchangeRates
{
    public static readonly Dictionary<string, decimal> Rates = new()
    {
        { "UAH", 1m },
        { "EUR", 46m },
    };
}
