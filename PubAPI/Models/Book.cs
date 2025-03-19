namespace PubAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; } // Relation till Publisher
    }
}