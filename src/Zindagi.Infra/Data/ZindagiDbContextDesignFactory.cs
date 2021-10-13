using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog.Extensions.Logging;

namespace Zindagi.Infra.Data
{
    public class ZindagiDbContextDesignFactory : IDesignTimeDbContextFactory<ZindagiDbContext>
    {
        public ZindagiDbContext CreateDbContext(string[] args)
        {
#if DEBUG
            const bool debugging = true;
#else
            const bool debugging = false;
#endif

            var optionsBuilder = new DbContextOptionsBuilder<ZindagiDbContext>();
#if SQLite
            optionsBuilder.UseSqlite($"Data Source=./store/zindagi.db;Cache=Shared;",
                                     sqlOptions => sqlOptions.MigrationsHistoryTable(ZindagiDbContext.MIGRATIONS).CommandTimeout(120).MaxBatchSize(10));
#else
            optionsBuilder.UseNpgsql("Server=localhost;Database=zindagi;Port=5432;User Id=temp;Password=temp;Ssl Mode=Require;",
                                       sqlOptions => sqlOptions.MigrationsHistoryTable(ZindagiDbContext.MIGRATIONS).CommandTimeout(120).MaxBatchSize(10));
#endif

            optionsBuilder.EnableDetailedErrors(debugging)
                .EnableSensitiveDataLogging(debugging)
                .UseLazyLoadingProxies()
                .UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture)
                .UseLoggerFactory(new SerilogLoggerFactory());

            return new ZindagiDbContext(optionsBuilder.Options, new NoMediator(), new NullLogger<ZindagiDbContext>());
        }

        private sealed class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => Task.CompletedTask;

            public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => Task.FromResult<TResponse>(default!);

            public Task<object?> Send(object request, CancellationToken cancellationToken = default) => Task.FromResult(default(object));
        }
    }
}
