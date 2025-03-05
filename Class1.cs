// PublisherDomain/Author.cs
namespace PublisherDomain
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }  // Exempel på författarens namn
        public string LastName { get; set; }  // Exempel på författarens efternamn

        public List<Book> Books { get; set; }  // Relaterade böcker
    }

    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }  // FK till Author
        public Author Author { get; set; }  // Relaterad författare
    }
}
