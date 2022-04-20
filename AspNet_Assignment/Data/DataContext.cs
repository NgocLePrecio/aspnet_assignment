#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNet_Assignment.Models;

namespace AspNet_Assignment
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<AspNet_Assignment.Models.FileUploaded> FileUploaded { get; set; }
    }
}
