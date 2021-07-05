using Common.Insfratructure.Enums;
using Common.Insfratructure.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Common.DataAccess.EF.Extensions;

namespace Common.DataAccess.EF
{
    public class CommonContext : DbContext
    {
        private readonly string _connectionName;

        public CommonContext() : this("ConnectionString")
        {
        }

        public CommonContext(string connectionName)
        {
            this._connectionName = connectionName;
        }

        public string ConnectionString => ConfigurationManager.ConnectionStrings[_connectionName] ?? _connectionName;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var buildDictionary = new Dictionary<DataBaseType, Action>
            {
                {
                    DataBaseType.MySQL,
                    () => {
                        optionsBuilder.UseLazyLoadingProxies(false)
                        .UseMySql(
                            this.ConnectionString,
                            new MySqlServerVersion(new Version(8,0,22))); }
                },
                {
                    DataBaseType.Postgresql,
                    () => {
                        optionsBuilder.UseLazyLoadingProxies()
                         .UseNpgsql(this.ConnectionString); }
                }
            };

            var dbType = this.DBType();
            if (buildDictionary.ContainsKey(dbType))
            {
                buildDictionary[dbType]();
            }
            else
            {
                buildDictionary[DataBaseType.SQL]();
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = RemoveForeignKeyExetension.RemoveForeignKeys(modelBuilder);
            base.OnModelCreating(modelBuilder);

            var maps = ConfigurationManager.ConnectionStrings["EF.Maps"];
            var toReisterMaps = Assembly.Load(maps);
            modelBuilder.ApplyConfigurationsFromAssembly(toReisterMaps);
        }
    }
}