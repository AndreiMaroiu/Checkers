namespace joc_dame.Models
{
    public class Player
    {
        public Player(string name, PieceType type)
        {
            Name = name;
            Type = type;
        }

        public string Name
        {
            get; set;
        }

        public PieceType Type
        {
            get;
            init;
        }

        public override string ToString()
            => $"{Name} {Type}";
    }
}
