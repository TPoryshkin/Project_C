public class BookDetails
{
    public int Pages { get; set; }
    public string Genre { get; set; }
    public DateTime PublicationDate { get; set; }

    public BookDetails(int pages, string genre, DateTime publicationDate)
    {
        Pages = pages;
        Genre = genre;
        PublicationDate = publicationDate;
    }
}