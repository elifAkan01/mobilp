using SQLite;

namespace ToDoApp.Models
{
    public class ToDo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}