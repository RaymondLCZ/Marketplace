namespace Marketplace.Ads.Domain.Shared;

public class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string message) : base(message) { }
}