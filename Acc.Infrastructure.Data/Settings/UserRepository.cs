using Acc.Context;
using Acc.Domain.Entities.DataModel;
using Acc.Domain.Interfaces.Settings;
using Acc.HelpersAndUtilities.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acc.Infrastructure.Data.Settings
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly SqlServerContext _sqlServerContext;

        public UserRepository(SqlServerContext sqlServerContext) : base(sqlServerContext)
        {
            _sqlServerContext = sqlServerContext;
        }
    }
}
