using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using ILogger = Serilog.ILogger;

namespace Aprecia.OnBoarding.Gateway.Api.Config.Serilog;

public static class ConfigurationSerilog
{
    public static ILogger AddLog(this IServiceCollection services, IConfiguration configuration, ConfigureHostBuilder host)
    {
        var table = configuration["TableLogs"];
        // 🔹 Configuración corregida de Serilog
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("serilog-debug.log", rollingInterval: RollingInterval.Day)
            .WriteTo.MSSqlServer(
                connectionString: configuration.GetConnectionString("OnboardingGateway"),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = table,
                    BatchPostingLimit = 1,  // 🔹 Guarda los logs de inmediato
                    SchemaName = "dbo",
                    AutoCreateSqlTable = true
                },
                columnOptions: new ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn>
                    {
                        new SqlColumn { ColumnName = "Method", DataType = SqlDbType.NVarChar, AllowNull = true, DataLength = 255 },
                        new SqlColumn { ColumnName = "Url", DataType = SqlDbType.NVarChar, AllowNull = true, DataLength = 2048 },  // 🔹 Aumentamos la URL a 2048 caracteres
                        new SqlColumn { ColumnName = "RequestBody", DataType = SqlDbType.NVarChar, AllowNull = true, DataLength = -1 }, // 🔹 NVARCHAR(MAX)
                        new SqlColumn { ColumnName = "ResponseBody", DataType = SqlDbType.NVarChar, AllowNull = true, DataLength = -1 }, // 🔹 NVARCHAR(MAX)
                        new SqlColumn { ColumnName = "StatusCode", DataType = SqlDbType.Int, AllowNull = true }
                    }
                })
            .CreateLogger();

        host.UseSerilog(logger);

        return logger;
    }
}
