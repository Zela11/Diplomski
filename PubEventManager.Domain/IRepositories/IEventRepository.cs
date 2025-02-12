﻿using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Domain.IRepositories;

public interface IEventRepository
{
    Task AddAsync(Event newEvent);
    Task<List<Event>> GetAllAsync();
    Task<Event> GetByIdAsync(int id);
    Task<Event> GetByDate(DateTime date);
}