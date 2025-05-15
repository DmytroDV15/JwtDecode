namespace JwtDecode.Models;
internal class UserTransactions
{
    public string? UserId { get; set; }
    public List<Transaction> Transactions { get; set; } = [];
}
