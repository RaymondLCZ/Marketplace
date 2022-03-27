using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.Shared;

/// <summary>
/// 定義檢核幣別的函式，是一個 Domain Service
/// </summary>
/// <remark>領域服務是在 Domain 裡只定義介面</remark>
public interface ICurrencyLookup
{
    Currency FindCurrency(string CurrencyCode);
}