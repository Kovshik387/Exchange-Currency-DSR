﻿using Exchange.Context.Context;
using Exchange.Context.Settings;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Context;

public static class DbContextOptionsFactory
{
    private const string migrationProjectPrefix = "Exchange.Context.Migrations.";

    public static DbContextOptions<RateDbContext> Create(string connStr, DbType dbType, bool detailedLogging = false)
    {
        var bldr = new DbContextOptionsBuilder<RateDbContext>();

        Configure(connStr, dbType, detailedLogging).Invoke(bldr);

        return bldr.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connStr, DbType dbType, bool detailedLogging = false)
    {
        return (bldr) =>
        {
            switch (dbType)
            {
                //case DbType.MSSQL:
                //    bldr.UseSqlServer(connStr,
                //        opts => opts
                //            .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                //            .MigrationsHistoryTable("_migrations")
                //            .MigrationsAssembly($"{migrationProjectPrefix}{DbType.MSSQL}")
                //    );
                //    break;

                case DbType.PgSql:
                    bldr.UseNpgsql(connStr,
                        opts => opts
                            .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                            .MigrationsHistoryTable("_migrations")
                            .MigrationsAssembly($"{migrationProjectPrefix}{DbType.PgSql}")
                    );
                    break;

                //case DbType.MySql:
                //    bldr.UseMySql(connStr, ServerVersion.AutoDetect(connStr),
                //        opts => opts
                //            .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                //            .SchemaBehavior(MySqlSchemaBehavior.Ignore)
                //            .MigrationsHistoryTable("_migrations")
                //            .MigrationsAssembly($"{migrationProjectPrefix}{DbType.MySql}")
                //    );
                //    break;
            }

            if (detailedLogging)
            {
                bldr.EnableSensitiveDataLogging();
            }

            // Attention!
            // It possible to use or LazyLoading or NoTracking at one time
            // Together this features don't work

            bldr.UseLazyLoadingProxies(true);
            //bldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        };
    }
}
