namespace Marketplace.EventSourcing;

public abstract class AggregateRoot : IInternalEventHandler
{
    readonly List<object> _changes = new List<Object>();

    #region properties
    public Guid Id { get; protected set; }
    public int Version { get; private set; } = -1;
    #endregion

    #region implement IInternalEventHandler
    public void Handle(object @event)
    {
        When(@event);
    }
    #endregion

    protected abstract void When(object @event);
    protected abstract void EnsureValidState();
    protected void Apply(object @event)
    {
        When(@event);
        EnsureValidState();
        _changes.Add(@event);
    }

    /// <summary>
    /// 檢索事件列表
    /// </summary>
    /// <returns></returns>
    public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

    public void Load(IEnumerable<object> history)
    {
        foreach (var each in history)
        {
            When(each);
            Version++;
        }
    }

    /// <summary>
    /// 清除事件列表
    /// </summary>
    public void ClearChanges() => _changes.Clear();

    protected void ApplyToEntity(IInternalEventHandler entity, object @event) => entity?.Handle(@event);
}