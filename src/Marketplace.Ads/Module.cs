using Marketplace.Ads.ClassifiedAds;
using Marketplace.Ads.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Ads;
public static class AdsModule
{
    const string SubscriptionName = "adsSubscription";

    public static IMvcCoreBuilder AddAdsModule(this IMvcCoreBuilder builder, string databaseName, ICurrencyLookup currencyLookup, UploadFile uploadFile)
    {
        EventMappings.MapEventTypes();

        builder.Services.AddSingleton(
            c =>
                new ClassifiedAdsCommandService(
                    new EsAggregateStore(c.GetEsConnection()),
                    currencyLookup,
                    uploadFile
                )
        );

        builder.Services.AddSingleton(
            c =>
            {
                var store = c.GetRavenStore();
                store.CheckAndCreateDatabase(databaseName);

                IAsyncDocumentSession GetSession()
                    => c.GetRavenStore()
                        .OpenAsyncSession(databaseName);

                return new SubscriptionManager(
                    c.GetEsConnection(),
                    new RavenDbCheckpointStore(
                        GetSession, SubscriptionName
                    ),
                    SubscriptionName,
                    new RavenDbProjection<ClassifiedAdDetails>(
                        GetSession,
                        ClassifiedAdDetailsProjection.GetHandler
                    ),
                    new RavenDbProjection<MyClassifiedAds>(
                        GetSession,
                        MyClassifiedAdsProjection.GetHandler
                    )
                );
            }
        );

        builder.AddApplicationPart(typeof(AdsModule).Assembly);

        return builder;
    }
}
