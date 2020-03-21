using Microsoft.EntityFrameworkCore;
using PhoneBookWebApp.Data;


namespace PhoneBookWebApp.Test
{
    public static class DbContextMocker
    {
        public static PhoneBookContext GetPhoneBookDbContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<PhoneBookContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new PhoneBookContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
