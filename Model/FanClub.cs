
namespace Football_App.Model
{
    public class FanClub
    {
        public int? ClubId { get; set; }
        public Club Club { get; set; }

        public int? FanId { get; set; }
        public Fan Fan { get; set; }
    }
}
