public class Book : LibraryItem, IPrintable, ICloneable, IComparable<Book>
{
    public override string ItemId { get; set; }
    public override string Title { get; set; }
    public BookDetails Details { get; set; }
    public Author Author { get; set; }

    public Book(string isbn, string title, BookDetails details, Author author = null)
    {
        ItemId = isbn;
        Title = title;
        Details = details;
        Author = author;
    }

    public override string GetDescription() =>
        $"Книга '{Title}' авторства {Author?.Name ?? "Невідомий автор"}. {Details.Pages} сторінок, жанр: {Details.Genre}";

    public override string GetItemType() => "Книга";

    public override string GetFullInfo()
    {
        return base.GetFullInfo() + $"\nАвтор: {Author?.Name}, Жанр: {Details.Genre}";
    }

    public string Print()
    {
        return $"=== Друк інформації про книгу ===\n" +
               $"ISBN: {ItemId}\n" +
               $"Назва: {Title}\n" +
               $"Автор: {Author?.Name ?? "Не вказано"}\n" +
               $"Країна автора: {Author?.Country ?? "Не вказано"}\n" +
               $"Сторінок: {Details.Pages}\n" +
               $"Жанр: {Details.Genre}\n" +
               $"Дата публікації: {Details.PublicationDate:dd.MM.yyyy}";
    }

    public object Clone()
    {
        return new Book(ItemId, Title,
            new BookDetails(Details.Pages, Details.Genre, Details.PublicationDate),
            Author != null ? new Author(Author.Name, Author.Country) : null);
    }

    public int CompareTo(Book other)
    {
        if (other == null) return 1;
        return string.Compare(Title, other.Title, StringComparison.Ordinal);
    }

    public override string ToString() => $"{Title} ({Author?.Name})";
}