using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class DbContextPoolService:IDbContextPoolService
    {
        private int maxSize = 20;
        private Dictionary<string, ConcurrentBag<PasswordManagerDbContext>> contextsMap = [];
        private Func<PasswordManagerDbContext> contextFactory;

        public DbContextPoolService(Func<PasswordManagerDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public PasswordManagerDbContext Acquire(string databaseName)
        {
            if (!contextsMap.TryGetValue(databaseName, out var contexts))
            {
                contextsMap[databaseName] = [];
            }
            if (contexts?.TryTake(out var context) ?? false)
            {
                return context;
            }

            return contextFactory();
        }

        public void Release(string databaseName, PasswordManagerDbContext context)
        {
            if (contextsMap[databaseName].Count < maxSize)
            {
                context.ChangeTracker.Clear();
                contextsMap[databaseName].Add(context);
            }
            else
            {
                context.Dispose();
            }
        }
    }
}
