﻿using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Domain.IRepositories
{
    public interface ITableRepository
    {
        Task AddAsync(Table table);
    }
}
