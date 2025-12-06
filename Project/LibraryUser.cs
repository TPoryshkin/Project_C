public class LibraryUser
{
    public string UserId { get; set; }
    public string FullName { get; set; }
    public List<Book> BorrowedBooks { get; set; }

    // 2. Події користувача
    public event Action<string, Book> BookBorrowedEvent;
    public event Action<string, Book> BookReturnedEvent;

    // Делегат для валідації (не подія!)
    public Func<Book, bool> BookBorrowValidation;

    public LibraryUser(string userId, string fullName)
    {
        UserId = userId;
        FullName = fullName;
        BorrowedBooks = new List<Book>();
    }

    public void BorrowBook(Book book)
    {
        if (book == null) return;

        // Використання делегата для валідації
        bool canBorrow = true;
        if (BookBorrowValidation != null)
        {
            canBorrow = BookBorrowValidation(book);
        }

        if (!canBorrow)
        {
            Console.WriteLine($"Не вдалося взяти книгу '{book.Title}' - не пройшла валідацію");
            return;
        }

        if (!BorrowedBooks.Contains(book))
        {
            BorrowedBooks.Add(book);
            Console.WriteLine($"{FullName} взяв(ла) книгу: '{book.Title}'");

            // 2. Виклик події взяття книги
            BookBorrowedEvent?.Invoke($"{FullName} взяв(ла) книгу", book);
        }
    }

    public void ReturnBook(Book book)
    {
        if (book != null && BorrowedBooks.Contains(book))
        {
            BorrowedBooks.Remove(book);
            Console.WriteLine($"{FullName} повернув(ла) книгу: '{book.Title}'");

            // 2. Виклик події повернення книги
            BookReturnedEvent?.Invoke($"{FullName} повернув(ла) книгу", book);
        }
    }

    public void ShowBorrowedBooks()
    {
        Console.WriteLine($"\n=== КНИГИ, ВЗЯТІ {FullName.ToUpper()} ===");
        if (BorrowedBooks.Count == 0)
        {
            Console.WriteLine("Користувач не брав книг");
            return;
        }

        // Використання Action делегата для виведення
        Action<Book> displayAction = book =>
            Console.WriteLine($"- {book.Title} (ISBN: {book.ItemId})");

        BorrowedBooks.ForEach(displayAction);
    }

    // Метод для демонстрації Predicate делегата
    public bool HasBook(Predicate<Book> searchCondition)
    {
        return BorrowedBooks.Exists(searchCondition);
    }

    public override string ToString() => $"{FullName} (ID: {UserId})";
}