using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetMVCServerSideDatatableExample.EntityFramework
{
    public class RepositoryBase
    {
        protected static DataContext context;
        protected static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            if (context == null)
            {
                lock (_lockSync)
                    if (context == null)
                    {
                        context = new DataContext();
                    }
            }
        }
    }
}