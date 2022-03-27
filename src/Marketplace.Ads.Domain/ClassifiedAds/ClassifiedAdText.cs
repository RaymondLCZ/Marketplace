using Marketplace.EventSourcing;

namespace Marketplace.Ads.Domain.ClassifiedAds;

public class ClassifiedAdText : Value<ClassifiedAdText>
{
    #region constructor
    internal ClassifiedAdText(string text) => Value = text;

    // Satisfy the serialization requirements 
    protected ClassifiedAdText() { }
    #endregion

    #region properties
    public string Value { get; }
    #endregion

    #region Factory function
    public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
    #endregion

    #region operator
    public static implicit operator string(ClassifiedAdText text) => text.Value;
    #endregion
}