using System.Collections.Generic;
using Football_App.Model;

namespace Football_App.Utilities
{
    public class ClubIdComparer : IEqualityComparer<Club>
    {
        public int GetHashCode(Club co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.ClubId.GetHashCode();
        }

        public bool Equals(Club x1, Club x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.ClubId== x2.ClubId;
        }
    }
}