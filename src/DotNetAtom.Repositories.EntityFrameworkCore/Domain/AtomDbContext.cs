using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DotNetAtom.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DotNetAtom;


public abstract class AtomDbContext : DbContext
{
    private readonly RelationalOptionsExtension? _relationExtension;

    protected AtomDbContext()
    {
    }

    protected AtomDbContext(DbContextOptions options)
        : base(options)
    {
        _relationExtension = options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();
    }

    public DbSet<AspNetApplication> AspNetApplication { get; set; } = null!;

    public DbSet<AspNetMembership> AspNetMembership { get; set; } = null!;

    public DbSet<AspNetUser> AspNetUser { get; set; } = null!;

    public DbSet<DesktopModule> DesktopModule { get; set; } = null!;

    public DbSet<HostSetting> HostSetting { get; set; } = null!;

    public DbSet<HtmlText> HtmlText { get; set; } = null!;

    public DbSet<Module> Module { get; set; } = null!;

    public DbSet<ModuleDefinition> ModuleDefinition { get; set; } = null!;

    public DbSet<ModulePermission> ModulePermission { get; set; } = null!;

    public DbSet<ModuleControl> ModuleControl { get; set; } = null!;

    public DbSet<Permission> Permission { get; set; } = null!;

    public DbSet<Portal> Portal { get; set; } = null!;

    public DbSet<PortalAlias> PortalAlias { get; set; } = null!;

    public DbSet<PortalDesktopModule> PortalDesktopModule { get; set; } = null!;

    public DbSet<PortalLocalization> PortalLocalization { get; set; } = null!;

    public DbSet<PortalSetting> PortalSetting { get; set; } = null!;

    public DbSet<Role> Role { get; set; } = null!;

    public DbSet<Tab> Tab { get; set; } = null!;

    public DbSet<TabModule> TabModule { get; set; } = null!;

    public DbSet<TabModuleSetting> TabModuleSetting { get; set; } = null!;

    public DbSet<TabPermission> TabPermission { get; set; } = null!;

    public DbSet<TabSetting> TabSetting { get; set; } = null!;

    public DbSet<User> User { get; set; } = null!;

    public DbSet<UserRole> UserRole { get; set; } = null!;

    public DbSet<Package> Package { get; set; } = null!;

    public DbSet<PackageType> PackageType { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AtomDbContext).Assembly);

        // If the database is SQL Server, fetch and ignore missing columns.
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer" &&
            _relationExtension?.ConnectionString is {} connectionString &&
            Type.GetType("Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient") is {} sqlConnectionType)
        {
            using var connection = (DbConnection)Activator.CreateInstance(sqlConnectionType, connectionString)!;
            connection.Open();
            IgnoreMissingColumnsSqlServer(modelBuilder, connection);
        }
    }

    /// <summary>
    /// Ignore missing columns in the database.
    /// </summary>
    /// <param name="modelBuilder">Model builder.</param>
    /// <param name="connection">Database connection.</param>
    private static void IgnoreMissingColumnsSqlServer(ModelBuilder modelBuilder, DbConnection connection)
    {
        var tableColumn = new Dictionary<(string Table, string Column), bool>();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT TABLE_NAME, COLUMN_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS";
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var key = (
                    reader.GetString(0).ToUpperInvariant(),
                    reader.GetString(1).ToUpperInvariant()
                );

                tableColumn.Add(key, reader.GetString(2) == "YES");
            }
        }

        var tableNames = tableColumn.Keys.Select(i => i.Table).Distinct().ToList();

        foreach (var t in modelBuilder.Model.GetEntityTypes().ToArray())
        {
            var tableNameDefault = t.GetTableName() ?? t.Name;
            var tableName = tableNameDefault.ToUpperInvariant();

            if (!tableNames.Contains(tableName))
            {
                modelBuilder.Ignore(t.ClrType);
                continue;
            }

            var builder = modelBuilder.Entity(t.ClrType);

            foreach (var p in t.GetProperties().ToArray())
            {
                var columnName = p.GetColumnName().ToUpperInvariant();

                if (!tableColumn.TryGetValue((tableName, columnName), out var isNullable))
                {
                    builder.Ignore(p.Name);
                    AtomDbContextColumnFixer.OnColumnIgnore(p, builder);
                    continue;
                }

                var property = builder.Property(p.Name);

                if (isNullable != property.Metadata.IsNullable)
                {
                    Debug.WriteLine($"Column {builder.Metadata.Name}.{property.Metadata.Name} does not match nullable (DB is null: {isNullable})");
                }
            }
        }
    }
}