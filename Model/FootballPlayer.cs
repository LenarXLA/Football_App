using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Football_App.Model
{
    public class FootballPlayer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PlayerId { get; set; }

        [Required]
        public string PlayerName { get; set; }
        public string PlayerBirthdate { get; set; }
        public string PlayerSNILS { get; set; }

        // Ссылка один к многим (к клубу)
        public int? ClubId { get; set; }
        public Club Club { get; set; }
    }
}
