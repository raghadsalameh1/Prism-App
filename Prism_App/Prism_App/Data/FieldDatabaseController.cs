using Prism_App.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_App.Data
{
    public class FieldDatabaseController
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DBconfiguration.DatabasePath, DBconfiguration.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public FieldDatabaseController()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(FieldItem).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(FieldItem)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<FieldItem>> GetItemsAsync()
        {
            return Database.Table<FieldItem>().ToListAsync();
        }

        public Task<List<FieldItem>> GetSearchedFieldsAsync(string str)
        {
            // SQL queries are also possible
            return Database.Table<FieldItem>().Where(i => i.FieldName.Contains(str)).ToListAsync();
        }

        public Task<FieldItem> GetItemAsync(int id)
        {
            return Database.Table<FieldItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(FieldItem item)
        {
            if (item.Id != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                await Database.InsertAsync(item);
                return item.Id;
            }
        }

        public Task<int> DeleteItemAsync(FieldItem item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> DeleteItemsAsync()
        {
            return Database.DeleteAllAsync<FieldItem>();              
        }
    }
}
