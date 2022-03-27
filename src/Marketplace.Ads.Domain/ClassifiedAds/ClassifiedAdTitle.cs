using Marketplace.EventSourcing;
using Marketplace.Ads.Domain.Shared;
using static System.String;
using System.Text.RegularExpressions;

namespace Marketplace.Ads.Domain.ClassifiedAds;

/// <summary>
/// 表示分類廣告的標題 (Value Object)
/// </summary>
public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
{
    #region constructor
    protected ClassifiedAdTitle() { }
    internal ClassifiedAdTitle(string value) => Value = value;
    #endregion

    #region property
    public string Value { get; }
    #endregion

    #region static
    public static ClassifiedAdTitle FromString(string title)
    {
        CheckValidity(title);
        return new ClassifiedAdTitle(title);
    }

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
                .Replace("<i>", "*")
                .Replace("</i>", "*")
                .Replace("<b>", "**")
                .Replace("</b>", "**");

        var value = Regex.Replace(supportedTagsReplaced, "<.*?>", Empty);
        CheckValidity(value);

        return new ClassifiedAdTitle(value);
    }
    #endregion

    public static implicit operator string(ClassifiedAdTitle title) => title.Value;
    static void CheckValidity(string value)
    {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value), "Title cannot be empty");

        if (value.Length < 10)
            throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be shorter than 10 characters");

        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 100 characters");
    }
}