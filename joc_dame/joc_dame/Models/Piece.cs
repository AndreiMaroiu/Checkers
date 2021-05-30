using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joc_dame.Models
{
    public class Piece
    {
        public Piece(Position pos, PieceType type = PieceType.NONE)
            => (Position, Type) = (pos, type);

        public Piece(Piece piece)
            => (Type, HasCrown, Position) = (piece.Type, piece.HasCrown, piece.Position);

        #region Properties

        public Position Position
        {
            get;
            set;
        }

        public bool HasCrown
        {
            get; set;
        } = false;

        public PieceType Type
        {
            get; set;
        } = PieceType.NONE;

        #endregion

        public void Set(PieceType type, bool hasCrown)
        {
            Type = type;
            HasCrown = hasCrown;
        }

        public void Set(Piece piece)
        {
            Type = piece.Type;
            HasCrown = piece.HasCrown;
        }

        public override string ToString()
        {
            if (HasCrown)
            {
                return Type.ToString() + " with crown";
            }

            return Type.ToString();
        }
    }
}
