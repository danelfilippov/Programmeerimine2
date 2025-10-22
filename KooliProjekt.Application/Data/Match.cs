using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Match
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; }
        public string Stage { get; set; }
        public string Description { get; set; }
        public int HomeScore { get; set; }
        public int GuestScore { get; set; }
    }
}
