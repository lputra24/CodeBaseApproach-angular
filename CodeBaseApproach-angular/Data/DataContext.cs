using CodeBaseApproach_angular.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBaseApproach_angular.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<ValueAngular> ValuesAngular { get; set; }
        public DbSet<User> User { get; set; }
    }
}
