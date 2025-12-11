using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Repository.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ShutdownCleanupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ShutdownCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // nothing to do here
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // This runs when the application is stopping
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Your wipe/reset logic here
                context.Database.ExecuteSqlRaw("DELETE FROM Animes");
            }
        }
    }

}
