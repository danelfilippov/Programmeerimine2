using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
   
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> tournaments { get; set; }
        public DbSet<Match> Matchs { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
    }
}
