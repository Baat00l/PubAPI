using PublisherDomain;

namespace PublisherData
{
    public static class DbInitializer
    {
        public static void Initialize(PubContext context)
        {
            // Om det redan finns data, gör inget
            if (context.Authors.Any()) return;

            // Skapa en Author och dess Books
            var author1 = new Author
            {
                FirstName = "John",
                LastName = "Doe",
                Books = new List<Book>
                {
                    new Book { Title = "The Great Adventure" },
                    new Book { Title = "The Second Story" }
                }
            };

            var author2 = new Author
            {
                FirstName = "Jane",
                LastName = "Smith",
                Books = new List<Book>
                {
                    new Book { Title = "Mystery of the Lost City" },
                    new Book { Title = "Secrets of the Deep" }
                }
            };

            // Lägg till Authors och Books i databasen
            context.Authors.AddRange(author1, author2);
            context.SaveChanges();
        }
    }
}
