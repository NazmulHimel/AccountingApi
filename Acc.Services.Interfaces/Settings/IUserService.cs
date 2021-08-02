using Acc.Domain.Entities.BodyModel;
using Acc.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acc.Services.Interfaces.Settings
{
    public interface IUserService
    {
        Task<User> UserLogin(LoginBodyModel user);
        Task<IEnumerable<User>> GetUser();
    }
}
