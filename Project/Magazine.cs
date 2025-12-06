public class Magazine : LibraryItem, IPrintable
{
    public override string ItemId { get; set; }
    public override string Title { get; set; }
    public string IssueNumber { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Publisher { get; set; }

    public Magazine(string issn, string title, string issueNumber, DateTime releaseDate, string publisher = "")
    {
        ItemId = issn;
        Title = title;
        IssueNumber = issueNumber;
        ReleaseDate = releaseDate;
        Publisher = publisher;
    }

    public override string GetDescription() =>
        $"Журнал '{Title}', випуск {IssueNumber}. Видавець: {Publisher}";

    public override string GetItemType() => "Журнал";

    public override string GetFullInfo()
    {
        return base.GetFullInfo() + $"\nВипуск: {IssueNumber}, Дата: {ReleaseDate:dd.MM.yyyy}";
    }

    public string Print()
    {
        return $"=== Друк інформації про журнал ===\n" +
               $"ISSN: {ItemId}\n" +
               $"Назва: {Title}\n" +
               $"Випуск: {IssueNumber}\n" +
               $"Дата випуску: {ReleaseDate:dd.MM.yyyy}\n" +
               $"Видавець: {Publisher}";
    }

    public override string ToString() => $"{Title} (Випуск {IssueNumber})";
}