namespace Marketplace.EventSourcing;
public abstract class AggregateId<T> : Value<AggregateId<T>> where T : AggregateRoot
{
protected AggregateId(Guid value)
    {
        if (value == default)
            throw new ArgumentNullException(
                nameof(value),
                "The Id cannot be empty");

        Value = value;
    }

    public Guid Value { get; }

// 實作隱含轉換 implicit conversion，將 value 指派給 primitive type
    public static implicit operator Guid(AggregateId<T> self) => self.Value;

    public override string ToString() => Value.ToString();
}
