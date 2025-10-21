namespace Test.Models
{
    // defines thing inside a database, it has an ID and a title as a string
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsComplete { get; set; }
    }
}