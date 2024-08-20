using PubEventManager.Application.IServices;
using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }
        public async Task<bool> CreateTable(int capacity)
        {
            var table = new Table
            {
                Capacity = capacity,
            };
            await _tableRepository.AddAsync(table);
            return true;
        }
    }
}
