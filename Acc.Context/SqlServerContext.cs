using Acc.Domain.Entities.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acc.Context
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {

        }
        public DbSet<User> User { set; get; }
    }
}
