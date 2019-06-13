using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RAS.Models.Models;

namespace RAS.DB
{
    public class Context : DbContext
    {
        public Context() : base("Data Source=ИЛЬЯ-ПК;Initial Catalog=ControlOfWaterDB;Integrated Security=False;User ID=Ilya;Password=Vecnfyu69")
        {
            // Инициализация БД
            Database.SetInitializer<Context>(new MigrateDatabaseToLatestVersion<Context, ConfigurationDB>());
        }
        public DbSet<House> Houses { get; set; }
        public DbSet<CounterOfWater> CountersOfWater { get; set; }
    }
}
