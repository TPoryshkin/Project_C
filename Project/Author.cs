public class Author
{
    public string Name { get; set; }
    public string Country { get; set; }

    public Author(string name, string country)
    {
        Name = name;
        Country = country;
    }

    public override bool Equals(object obj)
    {
        return obj is Author author && Name == author.Name && Country == author.Country;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Country);
    }

    public override string ToString() => $"{Name} ({Country})";
}