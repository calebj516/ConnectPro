using ContactPro.Data;
using Microsoft.EntityFrameworkCore;

namespace ConnectPro.Helpers
{
    public static class DataHelper
    {
        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            // Get an instance of the db application context
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            
            // Migration: this is equivalent to update-database
            await dbContextSvc.Database.MigrateAsync();
        }
    }
}
