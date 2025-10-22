using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Leaderboard
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int UserId { get; set; }
        public int TotalPoints { get; set; }
    }
}
