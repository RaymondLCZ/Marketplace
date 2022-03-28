using System;
using Xunit;
using Marketplace.Ads.Domain.ClassifiedAds;
using Marketplace.Ads.Domain.Shared;

namespace Marketplace.Tests;

public class ClassifiedAd_Publish_Spec
{
    private ClassifiedAd Ad { get; }

    public ClassifiedAd_Publish_Spec()
            => Ad = ClassifiedAd.Create(
                ClassifiedAdId.FromGuid(Guid.NewGuid()),
                UserId.FromGuid(Guid.NewGuid()));

    [Fact]
    public void Can_publish_a_valid_ad()
    {
        Ad.SetTitle(ClassifiedAdTitle.FromString("Test ad TESTTEST"));

        Ad.UpdateText(
            ClassifiedAdText.FromString("Please buy my stuff")
        );

        Ad.UpdatePrice(
            Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
        );

        Ad.RequestToPublish();

        Assert.Equal(
            ClassifiedAd.ClassifiedAdState.PendingReview,
            Ad.State
        );
    }

    [Fact]
    public void Cannot_publish_with_zero_price()
    {
        Ad.SetTitle(ClassifiedAdTitle.FromString("Test ad TEST"));

        Ad.UpdateText(
            ClassifiedAdText.FromString("Please buy my stuff")
        );

        Ad.UpdatePrice(
            Price.FromDecimal(0.0m, "EUR", new FakeCurrencyLookup())
        );

        Assert.Throws<DomainExceptions.InvalidEntityState>(
            () => Ad.RequestToPublish()
        );
    }
}