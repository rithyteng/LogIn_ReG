using Microsoft.EntityFrameworkCore;

namespace login_and_reg.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> User {get;set;}
    }
}
