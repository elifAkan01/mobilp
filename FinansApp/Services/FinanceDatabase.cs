using SQLite;
using FinansApp.Model;

namespace FinansApp.Services
{
    public class FinanceDatabase
    {
        SQLiteAsyncConnection? Database;

        async Task Init()
        {
            if (Database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Finance.db3");
            Database = new SQLiteAsyncConnection(dbPath);
            await Database.CreateTableAsync<Transaction>();
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            await Init();
            return await Database!.Table<Transaction>()
                                  .OrderByDescending(x => x.Date)
                                  .ToListAsync();
        }

        public async Task<int> SaveAsync(Transaction item)
        {
            await Init();

            if (item.Id != 0)
                return await Database!.UpdateAsync(item);

            return await Database!.InsertAsync(item);
        }

        public async Task<int> DeleteAsync(Transaction item)
        {
            await Init();
            return await Database!.DeleteAsync(item);
        }

        public async Task<double> GetTotalIncomeAsync()
        {
            await Init();
            var all = await Database!.Table<Transaction>().ToListAsync();
            return all.Where(x => x.IsIncome).Sum(x => x.Amount);
        }

        public async Task<double> GetTotalExpenseAsync()
        {
            await Init();
            var all = await Database!.Table<Transaction>().ToListAsync();
            return all.Where(x => !x.IsIncome).Sum(x => x.Amount);
        }

        public async Task<double> GetTotalBalanceAsync()
        {
            var income = await GetTotalIncomeAsync();
            var expense = await GetTotalExpenseAsync();
            return income - expense;
        }
    }
}