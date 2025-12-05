using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Leaderboard : Entity
    {
        [Required]
        [Unicode]
        public int Id { get; set; }
        [Required]
        public int TournamentId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TotalPoints { get; set; }
    }
}
