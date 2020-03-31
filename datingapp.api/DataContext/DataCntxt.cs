using datingapp.api;
using datingapp.api.Models;
using Microsoft.EntityFrameworkCore;

namespace datingapp.api.DataContext
{
   public class DataCntxt:DbContext
        {
            public DataCntxt(DbContextOptions<DataCntxt> options):base(options){}

            public DbSet<Values> Values { get; set; }
        }
    
}