using AspNetMVCServerSideDatatableExample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AspNetMVCServerSideDatatableExample.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext() : base("ServerSideDB")
        {
            Database.Connection.ConnectionString = "Server=.; Initial Catalog=ServerSideDB; Integrated Security=true";
        }

        public DbSet<Users> Users { get; set; }
    }
}