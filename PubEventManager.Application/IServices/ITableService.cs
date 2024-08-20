using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface ITableService
    {
        Task<bool> CreateTable(int capacity);
    }
}
