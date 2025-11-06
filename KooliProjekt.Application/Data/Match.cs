using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Match
    {
        [Required]
        [Unicode]
        public int Id { get; set; }
        [Required]
        public int TournamentId { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [Required]
        public int GuestTeamId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; }
        [MaxLength(50)]
        public string Stage { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public int HomeScore { get; set; }
        public int GuestScore { get; set; }
    }
}
