using System.Collections.Generic;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Data
{
    public class HistoryRepository : IRepository<CommandHistoryItem>
    {
        private readonly List<CommandHistoryItem> _memoryDb = new List<CommandHistoryItem>();

        public void Add(CommandHistoryItem entity)
        {
            _memoryDb.Add(entity);
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _memoryDb.Remove(item);
            }
        }

        public IEnumerable<CommandHistoryItem> GetAll()
        {
            return _memoryDb;
        }

        public CommandHistoryItem? GetById(int id)
        {
            return _memoryDb.Find(x => x.HistoryId == id);
        }
    }
}