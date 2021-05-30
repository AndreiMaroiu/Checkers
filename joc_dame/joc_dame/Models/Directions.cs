using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joc_dame.Models
{
    static class Directions
    {
        public static readonly Position UPPERLEFT = new (-1, -1);
        public static readonly Position UPPERRIGHT = new (-1, 1);
        public static readonly Position LOWERLEFT = new (1, -1);
        public static readonly Position LOWERRIGHT = new (1, 1);

        public static readonly Dictionary<PieceType, List<Position>> PieceDirections = new()
        {
            { PieceType.RED, new List<Position>(2) { UPPERLEFT, UPPERRIGHT } },
            { PieceType.DARK, new List<Position>(2) { LOWERLEFT, LOWERRIGHT } }
        };

        public static readonly List<Position> AllDirections = new()
        {
            UPPERLEFT,
            UPPERRIGHT,
            LOWERLEFT,
            LOWERRIGHT
        };
    }
}
