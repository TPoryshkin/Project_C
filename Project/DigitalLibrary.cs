public class DigitalLibrary : IEnumerable<Book>
{
    public string Name { get; set; }
    public List<LibraryItem> Items { get; set; }
    public List<Author> Authors { get; set; }
    public List<LibraryUser> Users { get; set; }

    // 1. Власний делегат для подій бібліотеки
    public delegate void LibraryEventHandler(string message, LibraryItem item);

    // 2. Події бібліотеки (тільки для підписки ззовні)
    public event LibraryEventHandler BookAddedEvent;
    public event Action<string> LibraryNotification;

    // Делегати для внутрішньої логіки (не події!)
    public Predicate<Book> BookSearchFilter;
    public Func<LibraryItem, bool> ItemValidationRequested;

    public DigitalLibrary(string name)
    {
        Name = name;
        Items = new List<LibraryItem>();
        Authors = new List<Author>();
        Users = new List<LibraryUser>();
    }

    // Методи для додавання елементів
    public void AddBook(Book book)
    {
        // Використання делегата Predicate для перевірки
        if (BookSearchFilter != null && BookSearchFilter(book))
        {
            Console.WriteLine($"Книга '{book.Title}' не пройшла фільтрацію");
            return;
        }

        Items.Add(book);
        if (book.Author != null && !Authors.Contains(book.Author))
        {
            Authors.Add(book.Author);
        }

        // 2. Виклик події додавання книги
        BookAddedEvent?.Invoke($"Додано нову книгу: {book.Title}", book);
        LibraryNotification?.Invoke($"Загальна кількість книг: {Items.OfType<Book>().Count()}");
    }

    public void AddMagazine(Magazine magazine)
    {
        // Використання делегата Func для валідації
        bool isValid = true;
        if (ItemValidationRequested != null)
        {
            isValid = ItemValidationRequested(magazine);
        }

        if (isValid)
        {
            Items.Add(magazine);
            LibraryNotification?.Invoke($"Додано журнал: {magazine.Title}");
        }
        else
        {
            Console.WriteLine("Журнал не пройшов валідацію");
        }
    }

    public void AddAuthor(Author author)
    {
        if (!Authors.Contains(author))
        {
            Authors.Add(author);
            LibraryNotification?.Invoke($"Додано автора: {author.Name}");
        }
    }

    public void RegisterUser(LibraryUser user)
    {
        if (!Users.Contains(user))
        {
            Users.Add(user);
            LibraryNotification?.Invoke($"Зареєстровано користувача: {user.FullName}");
        }
    }

    public void DisplayAllItems()
    {
        Console.WriteLine($"\n=== ВСІ ЕЛЕМЕНТИ БІБЛІОТЕКИ '{Name}' ===");
        foreach (var item in Items)
        {
            Console.WriteLine(item.GetFullInfo());
            Console.WriteLine("---");
        }
    }

    public void PrintAllPrintableItems()
    {
        Console.WriteLine($"\n=== ДРУК УСІХ ЕЛЕМЕНТІВ ===");
        foreach (var printable in Items.OfType<IPrintable>())
        {
            Console.WriteLine(printable.Print());
            Console.WriteLine("---");
        }
    }

    public List<Book> GetSortedBooks()
    {
        var books = Items.OfType<Book>().ToList();
        books.Sort();
        return books;
    }

    // Пошук книг за автором з використанням делегатів
    public List<Book> FindBooksByAuthor(string authorName)
    {
        // Використання вбудованого делегата Func для пошуку
        Func<Book, bool> searchFunc = book =>
            book.Author?.Name?.Contains(authorName, StringComparison.OrdinalIgnoreCase) == true;

        return Items.OfType<Book>()
                   .Where(searchFunc)
                   .ToList();
    }

    // Метод для демонстрації Action делегата
    public void ProcessAllBooks(Action<Book> bookAction)
    {
        foreach (var book in Items.OfType<Book>())
        {
            bookAction(book);
        }
    }

    // Метод для демонстрації Func делегата
    public List<string> GetBookTitles(Func<Book, string> titleSelector)
    {
        return Items.OfType<Book>()
                   .Select(titleSelector)
                   .ToList();
    }

    // Метод для демонстрації Predicate делегата
    public List<Book> FilterBooks(Predicate<Book> filter)
    {
        var result = new List<Book>();
        foreach (var book in Items.OfType<Book>())
        {
            if (filter(book))
            {
                result.Add(book);
            }
        }
        return result;
    }

    public IEnumerator<Book> GetEnumerator()
    {
        return Items.OfType<Book>().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Статистика
    public void ShowStatistics()
    {
        var books = Items.OfType<Book>().Count();
        var magazines = Items.OfType<Magazine>().Count();

        Console.WriteLine($"\n=== СТАТИСТИКА БІБЛІОТЕКИ ===");
        Console.WriteLine($"Кількість книг: {books}");
        Console.WriteLine($"Кількість журналів: {magazines}");
        Console.WriteLine($"Кількість авторів: {Authors.Count}");
        Console.WriteLine($"Кількість користувачів: {Users.Count}");
        Console.WriteLine($"Загальна кількість елементів: {Items.Count}");
    }
}