using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsApp.Data.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public Team HomeTeam { get; set; }

        public int HomeTeamId { get; set; }

        public Team AwayTeam { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }
    }
}
