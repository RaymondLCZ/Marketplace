using Marketplace.Ads.Domain.ClassifiedAds;
using Marketplace.Ads.Domain.Shared;
using Marketplace.EventSourcing;
using static Marketplace.Ads.Messages.Ads.Commands;

namespace Marketplace.Ads.ClassifiedAds;

public class ClassifiedAdsCommandService : ApplicationService<ClassifiedAd>
{
    public ClassifiedAdsCommandService(IAggregateStore store) : base(store)
    {
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}