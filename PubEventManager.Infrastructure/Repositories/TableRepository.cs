using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using PubEventManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Infrastructure.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly AppDbContext _context;
        public TableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Table table)
        {
            if(table != null) { await _context.Tables.AddAsync(table); }
            await _context.SaveChangesAsync();
        }
    }
}
