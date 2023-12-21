using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Football_App.Model
{
    public class Club
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClubId { get; set; }

        [Required]
        public string ClubName { get; set; }

        public string ClubCity { get; set; }


        // Ссылка один к многим (к футболистам)
        public ICollection<FootballPlayer> FootballPlayers { get; set; }

        // Ссылка многие к многим (к болельщикам)
        public IList<FanClub> FanClubs { get; set; }
    }
}
