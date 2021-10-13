using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrazingView.Db.Entities;

using Microsoft.EntityFrameworkCore;

namespace CrazingView.Db
{
    public class CrazyContext : DbContext
    {
        public CrazyContext(DbContextOptions<CrazyContext> o) :base(o)
        {

        }


        public DbSet<Strategy> Strategies{ get; set; }
        public DbSet<StrategyInput> StrategyInputs { get; set; }
        public DbSet<Configuraiton> Configuraitons { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionLog> SessionLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
