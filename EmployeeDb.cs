using Microsoft.EntityFrameworkCore;

class EmployeeDb : DbContext
{
    public EmployeeDb(DbContextOptions<EmployeeDb> options)
        : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
}