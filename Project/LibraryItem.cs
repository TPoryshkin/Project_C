
public abstract class LibraryItem
{
    public abstract string ItemId { get; set; }
    public abstract string Title { get; set; }

    public abstract string GetDescription();
    public abstract string GetItemType();

    public virtual string GetFullInfo()
    {
        return $"{GetItemType()}: {Title} (ID: {ItemId})";
    }
}