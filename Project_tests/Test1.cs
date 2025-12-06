using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

[TestClass]
public class DigitalLibraryTests
{
    [TestMethod]
    public void DigitalLibrary_AddBook_ShouldAddBookToCollection()
    {
        var library = new DigitalLibrary("Test Library");
        var author = new Author("John Doe", "USA");
        var book = new Book("123", "Test Book", new BookDetails(100, "Fiction", DateTime.Now), author);

        library.AddBook(book);

        Assert.AreEqual(1, library.Items.Count);
        Assert.AreEqual(1, library.Authors.Count);
        Assert.IsTrue(library.Items.Contains(book));
    }

    [TestMethod]
    public void DigitalLibrary_AddMagazine_ShouldAddMagazineToCollection()
    {
        var library = new DigitalLibrary("Test Library");
        var magazine = new Magazine("ISSN123", "Tech Magazine", "2023-01", DateTime.Now, "TechPub");

        library.AddMagazine(magazine);

        Assert.AreEqual(1, library.Items.Count);
        Assert.IsTrue(library.Items.Contains(magazine));
    }

    [TestMethod]
    public void Book_Print_ShouldReturnFormattedBookInfo()
    {
        var author = new Author("John Doe", "USA");
        var book = new Book("123", "Test Book", new BookDetails(100, "Fiction", new DateTime(2023, 1, 1)), author);

        string result = book.Print();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("Test Book"));
        Assert.IsTrue(result.Contains("John Doe"));
        Assert.IsTrue(result.Contains("123"));
    }

    [TestMethod]
    public void Book_Clone_ShouldCreateIndependentCopy()
    {
        var author = new Author("Original Author", "USA");
        var original = new Book("123", "Original Book", new BookDetails(100, "Sci-Fi", DateTime.Now), author);

        var cloned = (Book)original.Clone();
        cloned.Title = "Cloned Book";
        cloned.Author.Name = "Cloned Author";

        Assert.AreNotEqual(original.Title, cloned.Title);
        Assert.AreNotEqual(original.Author.Name, cloned.Author.Name);
    }

    [TestMethod]
    public void Book_CompareTo_ShouldSortBooksByTitle()
    {
        var book1 = new Book("1", "Apple Book", new BookDetails(100, "Fiction", DateTime.Now));
        var book2 = new Book("2", "Banana Book", new BookDetails(150, "Fiction", DateTime.Now));

        int result = book1.CompareTo(book2);

        Assert.IsTrue(result < 0);
    }

    [TestMethod]
    public void LibraryUser_BorrowBook_ShouldAddBookToBorrowedList()
    {
        var user = new LibraryUser("U1", "John Doe");
        var book = new Book("123", "Test Book", new BookDetails(100, "Fiction", DateTime.Now));

        user.BorrowBook(book);

        Assert.AreEqual(1, user.BorrowedBooks.Count);
        Assert.IsTrue(user.BorrowedBooks.Contains(book));
    }

    [TestMethod]
    public void LibraryUser_ReturnBook_ShouldRemoveBookFromBorrowedList()
    {
        var user = new LibraryUser("U1", "John Doe");
        var book = new Book("123", "Test Book", new BookDetails(100, "Fiction", DateTime.Now));

        user.BorrowBook(book);
        user.ReturnBook(book);

        Assert.AreEqual(0, user.BorrowedBooks.Count);
        Assert.IsFalse(user.BorrowedBooks.Contains(book));
    }

    [TestMethod]
    public void DigitalLibrary_GetSortedBooks_ShouldReturnSortedList()
    {
        var library = new DigitalLibrary("Test Library");
        var book1 = new Book("2", "Banana", new BookDetails(100, "Fiction", DateTime.Now));
        var book2 = new Book("1", "Apple", new BookDetails(150, "Fiction", DateTime.Now));

        library.AddBook(book1);
        library.AddBook(book2);

        var sorted = library.GetSortedBooks();

        Assert.AreEqual("Apple", sorted[0].Title);
        Assert.AreEqual("Banana", sorted[1].Title);
    }

    [TestMethod]
    public void DigitalLibrary_ProcessAllBooks_ShouldExecuteActionForEachBook()
    {
        var library = new DigitalLibrary("Test Library");
        var book1 = new Book("1", "Book 1", new BookDetails(100, "Fiction", DateTime.Now));
        var book2 = new Book("2", "Book 2", new BookDetails(200, "Non-Fiction", DateTime.Now));

        library.AddBook(book1);
        library.AddBook(book2);

        int count = 0;
        library.ProcessAllBooks(book => count++);

        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public void LibraryUser_HasBook_ShouldReturnCorrectResult()
    {
        var user = new LibraryUser("U1", "John Doe");
        var book1 = new Book("1", "Short Book", new BookDetails(50, "Fiction", DateTime.Now));
        var book2 = new Book("2", "Long Book", new BookDetails(500, "Fiction", DateTime.Now));

        user.BorrowBook(book1);

        bool hasShortBook = user.HasBook(book => book.Details.Pages < 100);
        bool hasLongBook = user.HasBook(book => book.Details.Pages > 400);

        Assert.IsTrue(hasShortBook);
        Assert.IsFalse(hasLongBook);
    }
}