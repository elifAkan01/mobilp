using SQLite;

namespace FinansApp.Model
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public string? Category { get; set; }

        public bool IsIncome { get; set; }
    }
}