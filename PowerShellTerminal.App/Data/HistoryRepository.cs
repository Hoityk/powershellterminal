using System.Collections.Generic;
using System.Linq;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Data
{
    public class HistoryRepository : IRepository<CommandHistoryItem>
    {
        public void Add(CommandHistoryItem entity)
        {
            using (var db = new AppDbContext())
            {
                db.CommandHistoryItems.Add(entity);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var item = db.CommandHistoryItems.Find(id);
                if (item != null)
                {
                    db.CommandHistoryItems.Remove(item);
                    db.SaveChanges();
                }
            }
        }

        public IEnumerable<CommandHistoryItem> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return db.CommandHistoryItems.ToList();
            }
        }

        public CommandHistoryItem? GetById(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.CommandHistoryItems.Find(id);
            }
        }
    }
}