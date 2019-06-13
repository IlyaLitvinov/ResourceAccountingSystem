using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace RAS.DB
{
    public class ConfigurationDB : DbMigrationsConfiguration<Context>
    {
        public ConfigurationDB()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(Context context)
        {
            // Наполнение БД по умолчанию
            base.Seed(context);
        }
    }
}
