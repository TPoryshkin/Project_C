using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    private static DigitalLibrary library = new DigitalLibrary("Цифрова бібліотека Університету");

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Підписка на події бібліотеки
        SubscribeToLibraryEvents();

        // 6 Конкретні реалізації - ініціалізація тестовими даними
        InitializeSampleData();

        Console.WriteLine("=== ЦИФРОВА БІБЛІОТЕКА ===");

        bool exit = false;
        while (!exit)
        {
            // 8 Логічне меню додатка
            DisplayMenu();

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": AddNewBook(); break;
                case "2": AddNewMagazine(); break;
                case "3": RegisterNewUser(); break;
                case "4": DisplayAllItems(); break;
                case "5": PrintAllItems(); break;
                case "6": ShowSortedBooks(); break;
                case "7": SearchBooksByAuthor(); break;
                case "8": ManageUserBooks(); break;
                case "9": ShowStatistics(); break;
                case "10": DemonstratePolymorphism(); break;
                case "11": DemonstrateDelegates(); break;
                case "0": exit = true; break;
                default: ShowInvalidChoice(); break;
            }
        }

        Console.WriteLine("Дякуємо за використання цифрової бібліотеки!");
    }

    static void SubscribeToLibraryEvents()
    {
        // 2. Підписка на події бібліотеки (використовуємо +=)
        library.BookAddedEvent += (message, item) =>
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[ПОДІЯ] {message}");
            Console.ResetColor();
        };

        library.LibraryNotification += message =>
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[СПОВІЩЕННЯ] {message}");
            Console.ResetColor();
        };

        // Налаштування делегатів (не подій!) - використовуємо =
        library.BookSearchFilter = book => book.Details.Pages < 50;
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\n=== МЕНЮ ===");
        Console.WriteLine("1. Додати нову книгу");
        Console.WriteLine("2. Додати новий журнал");
        Console.WriteLine("3. Зареєструвати нового користувача");
        Console.WriteLine("4. Показати всі елементи (коротка інформація)");
        Console.WriteLine("5. Надрукувати всі елементи (детальна інформація)");
        Console.WriteLine("6. Показати відсортовані книги");
        Console.WriteLine("7. Пошук книг за автором");
        Console.WriteLine("8. Керування книгами користувача");
        Console.WriteLine("9. Статистика бібліотеки");
        Console.WriteLine("10. Демонстрація поліморфізму");
        Console.WriteLine("11. Демонстрація делегатів");
        Console.WriteLine("0. Вийти");
        Console.Write("Оберіть опцію: ");
    }

    static void InitializeSampleData()
    {
        // 6 Конкретні реалізації - створення тестових даних
        var author1 = new Author("Джоан Роулінг", "Велика Британія");
        var author2 = new Author("Джордж Орвелл", "Велика Британія");
        var author3 = new Author("Айзек Азімов", "США");

        library.AddAuthor(author1);
        library.AddAuthor(author2);
        library.AddAuthor(author3);

        // Додаємо валідацію для журналів (не подія, тому використовуємо =)
        library.ItemValidationRequested = item =>
        {
            if (item is Magazine magazine)
            {
                return !string.IsNullOrEmpty(magazine.Publisher) &&
                       magazine.ReleaseDate.Year >= 2000;
            }
            return true;
        };

        // 3 Абстрактний клас - створення об'єктів похідних класів
        library.AddBook(new Book("978-0545010221", "Гаррі Поттер і філософський камінь",
            new BookDetails(320, "Фентезі", new DateTime(1997, 6, 26)), author1));

        library.AddBook(new Book("978-0451524935", "1984",
            new BookDetails(328, "Антиутопія", new DateTime(1949, 6, 8)), author2));

        library.AddBook(new Book("978-0553293357", "Я, робот",
            new BookDetails(253, "Наукова фантастика", new DateTime(1950, 12, 2)), author3));

        // 3 Абстрактний клас - створення об'єкта другого похідного класу
        library.AddMagazine(new Magazine("ISSN-1234", "Наука сьогодні", "2023-03",
            new DateTime(2023, 3, 15), "Наукове видавництво"));

        library.AddMagazine(new Magazine("ISSN-5678", "Технології майбутнього", "2023-02",
            new DateTime(2023, 2, 10), "Технології"));

        // 6 Конкретні реалізації - реєстрація користувачів
        var user1 = new LibraryUser("U001", "Іван Петренко");
        var user2 = new LibraryUser("U002", "Марія Коваленко");

        // Підписка на події користувачів (використовуємо +=)
        user1.BookBorrowedEvent += (message, book) =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ПОДІЯ КОРИСТУВАЧА] {message}: {book.Title}");
            Console.ResetColor();
        };

        // Налаштування делегата валідації (не подія, тому використовуємо =)
        user1.BookBorrowValidation = book => book.Details.Pages < 500;

        library.RegisterUser(user1);
        library.RegisterUser(user2);

        Console.WriteLine("Тестові дані успішно завантажені!");
    }

    static void AddNewBook()
    {
        Console.WriteLine("\n=== ДОДАВАННЯ НОВОЇ КНИГИ ===");

        try
        {
            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Назва: ");
            string title = Console.ReadLine();

            Console.Write("Кількість сторінок: ");
            int pages = int.Parse(Console.ReadLine());

            Console.Write("Жанр: ");
            string genre = Console.ReadLine();

            Console.Write("Ім'я автора: ");
            string authorName = Console.ReadLine();

            Console.Write("Країна автора: ");
            string country = Console.ReadLine();

            var author = new Author(authorName, country);

            var book = new Book(isbn, title, new BookDetails(pages, genre, DateTime.Now), author);

            library.AddBook(book);
            Console.WriteLine($"Книга '{title}' успішно додана!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при додаванні книги: {ex.Message}");
        }
    }

    static void AddNewMagazine()
    {
        Console.WriteLine("\n=== ДОДАВАННЯ НОВОГО ЖУРНАЛУ ===");

        try
        {
            Console.Write("ISSN: ");
            string issn = Console.ReadLine();

            Console.Write("Назва: ");
            string title = Console.ReadLine();

            Console.Write("Номер випуску: ");
            string issue = Console.ReadLine();

            Console.Write("Видавець: ");
            string publisher = Console.ReadLine();

            var magazine = new Magazine(issn, title, issue, DateTime.Now, publisher);
            library.AddMagazine(magazine);
            Console.WriteLine($"Журнал '{title}' успішно доданий!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при додаванні журналу: {ex.Message}");
        }
    }

    static void RegisterNewUser()
    {
        Console.WriteLine("\n=== РЕЄСТРАЦІЯ НОВОГО КОРИСТУВАЧА ===");

        try
        {
            Console.Write("ID користувача: ");
            string userId = Console.ReadLine();

            Console.Write("Повне ім'я: ");
            string fullName = Console.ReadLine();

            var user = new LibraryUser(userId, fullName);

            // Підписка на події нового користувача (використовуємо +=)
            user.BookBorrowedEvent += (message, book) =>
            {
                Console.WriteLine($"[НОВА ПОДІЯ] {fullName}: {message}");
            };

            library.RegisterUser(user);
            Console.WriteLine($"Користувач {fullName} успішно зареєстрований!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при реєстрації користувача: {ex.Message}");
        }
    }

    static void DisplayAllItems()
    {
        library.DisplayAllItems();
    }

    static void PrintAllItems()
    {
        library.PrintAllPrintableItems();
    }

    static void ShowSortedBooks()
    {
        Console.WriteLine("\n=== ВІДСОРТОВАНІ КНИГИ ЗА НАЗВОЮ ===");

        var sortedBooks = library.GetSortedBooks();

        if (sortedBooks.Count == 0)
        {
            Console.WriteLine("В бібліотеці немає книг");
            return;
        }

        foreach (var book in sortedBooks)
        {
            Console.WriteLine($"- {book.Title} (автор: {book.Author?.Name})");
        }
    }

    static void SearchBooksByAuthor()
    {
        Console.Write("\nВведіть ім'я автора для пошуку: ");
        string authorName = Console.ReadLine();

        var books = library.FindBooksByAuthor(authorName);

        Console.WriteLine($"\n=== РЕЗУЛЬТАТИ ПОШУКУ ДЛЯ '{authorName}' ===");
        if (books.Count == 0)
        {
            Console.WriteLine("Книги не знайдено");
            return;
        }

        foreach (var book in books)
        {
            Console.WriteLine($"- {book.Title} (ISBN: {book.ItemId})");
        }
    }

    static void ManageUserBooks()
    {
        if (library.Users.Count == 0)
        {
            Console.WriteLine("Немає зареєстрованих користувачів");
            return;
        }

        Console.WriteLine("\n=== КЕРУВАННЯ КНИГАМИ КОРИСТУВАЧА ===");
        Console.WriteLine("Оберіть користувача:");

        for (int i = 0; i < library.Users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {library.Users[i]}");
        }

        try
        {
            int userIndex = int.Parse(Console.ReadLine()) - 1;
            if (userIndex < 0 || userIndex >= library.Users.Count)
            {
                Console.WriteLine("Невірний номер користувача");
                return;
            }

            var user = library.Users[userIndex];

            Console.WriteLine($"\nОбрано: {user.FullName}");
            Console.WriteLine("1. Взяти книгу");
            Console.WriteLine("2. Повернути книгу");
            Console.WriteLine("3. Показати взяті книги");
            Console.WriteLine("4. Перевірити наявність книги за критерієм");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": BorrowBookForUser(user); break;
                case "2": ReturnBookForUser(user); break;
                case "3": user.ShowBorrowedBooks(); break;
                case "4": CheckUserBooksByCriteria(user); break;
                default: Console.WriteLine("Невірний вибір"); break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void BorrowBookForUser(LibraryUser user)
    {
        var books = library.Items.OfType<Book>().ToList();
        if (books.Count == 0)
        {
            Console.WriteLine("В бібліотеці немає книг");
            return;
        }

        Console.WriteLine("\nОберіть книгу для взяття:");
        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {books[i].Title} ({books[i].Details.Pages} стор.)");
        }

        try
        {
            int bookIndex = int.Parse(Console.ReadLine()) - 1;
            if (bookIndex < 0 || bookIndex >= books.Count)
            {
                Console.WriteLine("Невірний номер книги");
                return;
            }

            user.BorrowBook(books[bookIndex]);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void ReturnBookForUser(LibraryUser user)
    {
        if (user.BorrowedBooks.Count == 0)
        {
            Console.WriteLine("Користувач не має взятих книг");
            return;
        }

        Console.WriteLine("\nОберіть книгу для повернення:");
        for (int i = 0; i < user.BorrowedBooks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {user.BorrowedBooks[i].Title}");
        }

        try
        {
            int bookIndex = int.Parse(Console.ReadLine()) - 1;
            if (bookIndex < 0 || bookIndex >= user.BorrowedBooks.Count)
            {
                Console.WriteLine("Невірний номер книги");
                return;
            }

            user.ReturnBook(user.BorrowedBooks[bookIndex]);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void CheckUserBooksByCriteria(LibraryUser user)
    {
        Console.WriteLine("\nПеревірка наявності книги за критерієм:");
        Console.WriteLine("Введіть мінімальну кількість сторінок: ");

        if (int.TryParse(Console.ReadLine(), out int minPages))
        {
            Predicate<Book> criteria = book => book.Details.Pages >= minPages;
            bool hasBook = user.HasBook(criteria);

            Console.WriteLine(hasBook
                ? $"Користувач має книгу з {minPages}+ сторінками"
                : $"Користувач не має книг з {minPages}+ сторінками");
        }
    }

    static void ShowStatistics()
    {
        library.ShowStatistics();
    }

    static void DemonstratePolymorphism()
    {
        Console.WriteLine("\n=== ДЕМОНСТРАЦІЯ ПОЛІМОРФІЗМУ ===");

        Console.WriteLine("\n1. Поліморфізм через абстрактний клас LibraryItem:");
        Console.WriteLine("Виклик методів GetItemType(), GetDescription(), GetFullInfo():");
        foreach (var item in library.Items)
        {
            Console.WriteLine($"Тип: {item.GetItemType()}");
            Console.WriteLine($"Опис: {item.GetDescription()}");
            Console.WriteLine($"Повна інформація: {item.GetFullInfo()}");
            Console.WriteLine("---");
        }

        Console.WriteLine("\n2. Поліморфізм через інтерфейс IPrintable:");
        Console.WriteLine("Виклик методу Print() для всіх об'єктів, що реалізують IPrintable:");
        foreach (var printable in library.Items.OfType<IPrintable>())
        {
            Console.WriteLine(printable.Print());
            Console.WriteLine("---");
        }

        Console.WriteLine("\n3. Демонстрація клонування (ICloneable):");
        if (library.Items.OfType<Book>().Any())
        {
            var originalBook = library.Items.OfType<Book>().First();
            var clonedBook = (Book)originalBook.Clone();
            clonedBook.Title = "КЛОН: " + clonedBook.Title;

            Console.WriteLine($"Оригінал: {originalBook.Title}");
            Console.WriteLine($"Клон: {clonedBook.Title}");
            Console.WriteLine("Клонування пройшло успішно - створено незалежну копію!");
        }

        Console.WriteLine("\n4. Демонстрація сортування (IComparable):");
        var sortedBooks = library.GetSortedBooks();
        Console.WriteLine("Книги відсортовані за назвою:");
        foreach (var book in sortedBooks)
        {
            Console.WriteLine($"- {book.Title}");
        }

        Console.WriteLine("\n5. Демонстрація перевизначення віртуального методу:");
        foreach (var item in library.Items)
        {
            Console.WriteLine($"Вирішення на етапі виконання: {item.GetFullInfo()}");
        }

        Console.WriteLine("\n6. Демонстрація ітерації через IEnumerable:");
        Console.WriteLine("Всі книги в бібліотеці (через foreach):");
        foreach (var book in library)
        {
            Console.WriteLine($"- {book.Title}");
        }
    }

    static void DemonstrateDelegates()
    {
        Console.WriteLine("\n=== ДЕМОНСТРАЦІЯ ДЕЛЕГАТІВ ===");

        // 1. Демонстрація Action делегата
        Console.WriteLine("\n1. Використання Action делегата для обробки книг:");
        library.ProcessAllBooks(book =>
        {
            Console.WriteLine($"Обробка: {book.Title} ({book.Details.Pages} стор.)");
        });

        // 2. Демонстрація Func делегата
        Console.WriteLine("\n2. Використання Func делегата для отримання даних:");
        var bookTitles = library.GetBookTitles(book =>
            $"{book.Title} - {book.Author?.Name}");

        Console.WriteLine("Назви книг з авторами:");
        foreach (var title in bookTitles)
        {
            Console.WriteLine($"  {title}");
        }

        // 3. Демонстрація Predicate делегата
        Console.WriteLine("\n3. Використання Predicate делегата для фільтрації:");
        var longBooks = library.FilterBooks(book => book.Details.Pages > 300);
        Console.WriteLine($"Книги з більше ніж 300 сторінками: {longBooks.Count}");

        // 4. Демонстрація мультикаст делегата
        Console.WriteLine("\n4. Ланцюжок делегатів (multicast delegate):");
        Action<string> multiDelegate = null;

        multiDelegate += message => Console.WriteLine($"Обробник 1: {message}");
        multiDelegate += message => Console.WriteLine($"Обробник 2: {message.ToUpper()}");
        multiDelegate += message => Console.WriteLine($"Обробник 3: Довжина = {message.Length}");

        multiDelegate?.Invoke("Тестове повідомлення");

        // 5. Демонстрація подій
        Console.WriteLine("\n5. Демонстрація підписки на події:");
        Console.WriteLine("Додаємо додаткову обробку для події додавання книги...");

        library.BookAddedEvent += (message, item) =>
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[ДОДАТКОВА ПІДПИСКА] Подія: {message}");
            Console.ResetColor();
        };

        // Додамо тестову книгу для демонстрації подій
        Console.WriteLine("\nДодаємо тестову книгу для демонстрації подій...");
        var testBook = new Book("999-9999999999", "Тестова книга для демонстрації",
            new BookDetails(150, "Технічна", DateTime.Now),
            new Author("Тестовий автор", "Україна"));

        library.AddBook(testBook);
    }

    static void ShowInvalidChoice()
    {
        Console.WriteLine("Невірний вибір! Спробуйте ще раз.");
    }
}