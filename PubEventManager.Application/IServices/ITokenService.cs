﻿using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
