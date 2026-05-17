using SQLite;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class Database
    {
        private SQLiteAsyncConnection? _database;

        private async Task Init()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TodoSqlite.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<ToDo>();
        }

        public async Task<List<ToDo>> GetItemsAsync()
        {
            await Init();
            return await _database!.Table<ToDo>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(ToDo item)
        {
            await Init();

            if (item.Id != 0)
                return await _database!.UpdateAsync(item);

            return await _database!.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(ToDo item)
        {
            await Init();
            return await _database!.DeleteAsync(item);
        }

        public async Task<List<ToDo>> GetCompletedItemsAsync()
        {
            await Init();
            return await _database!.Table<ToDo>()
                                   .Where(x => x.IsCompleted)
                                   .ToListAsync();
        }

        public async Task<List<ToDo>> GetOverdueItemsAsync()
        {
            await Init();
            return await _database!.Table<ToDo>()
                                   .Where(x => x.DueDate < DateTime.Now && !x.IsCompleted)
                                   .ToListAsync();
        }
    }
}