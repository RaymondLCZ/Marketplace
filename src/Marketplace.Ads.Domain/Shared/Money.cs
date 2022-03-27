using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.Shared;

public class Money : Value<Money>
{
    #region constructor
    // 所有建構子都是 protected 的，確保 Money 不會被 new 
    protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
    {
        if (string.IsNullOrEmpty(currencyCode))
            throw new ArgumentNullException(
                nameof(currencyCode), "Currency code must be specified"
            );

        var currency = currencyLookup.FindCurrency(currencyCode);

        if (!currency.InUse)
            throw new ArgumentException($"Currency {currencyCode} is not valid");

        if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                $"Amount in {currencyCode} cannot have more than {currency.DecimalPlaces} decimals"
            );

        Amount = amount;
        Currency = currency;
    }

    protected Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    // Satisfy the serialization requirements
    protected Money() { }
    #endregion

    #region properties
    // property 只提供 get，確保一賦值就不可以被修改，因為 value object 是 immutable 
    public decimal Amount { get; }
    public Currency? Currency { get; }
    #endregion

    public static Money FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
        => new Money(amount, currency, currencyLookup);

    public static Money FromString(string amount, string currency, ICurrencyLookup currencyLookup)
        => new Money(decimal.Parse(amount), currency, currencyLookup);

    public Money Add(Money summand)
    {
        if (Currency is null) throw new Exception();

        if (summand.Currency is null) throw new Exception();

        if (Currency != summand.Currency)
            throw new CurrencyMismatchException("Cannot sum amounts with different currencies");

        return new Money(Amount + summand.Amount, Currency);
    }

    public Money Subtract(Money subtrahend)
    {
        if (Currency is null) throw new Exception();

        if (subtrahend.Currency is null) throw new Exception();

        if (Currency != subtrahend.Currency)
            throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");

        return new Money(Amount - subtrahend.Amount, Currency);
    }

    public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);

    public static Money operator -(Money minuend, Money subtrahend) => minuend.Subtract(subtrahend);

    public override string ToString() => $"{Currency?.CurrencyCode} {Amount}";
}

