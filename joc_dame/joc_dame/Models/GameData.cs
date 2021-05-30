using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joc_dame.Models
{
    public class GameData
    {
        private Player first;
        private Player second;
        private Piece[][] table;
        private bool multipleJumps;

        public GameData(Player first, Player second, Piece[][] table, bool multipleJumps)
            => (this.first, this.second, this.table, this.multipleJumps) = (first, second, table, multipleJumps);

        public GameData(List<string> lines, int size)
            => ReadFromStream(lines, size);

        public Player FirstPlayer => first;

        public Player SecondPlayer => second;

        public Piece[][] Table => table;

        public bool MultipleJumps => multipleJumps;

        public string[] ToFile()
        {
            string[] lines = new string[table.Length + 1];
            int index = 1;

            lines[0] = $"{first} {second} {multipleJumps}";

            foreach (var row in table)
            {
                StringBuilder builder = new();

                foreach (var piece in row)
                {
                    if (piece.Type is PieceType.NONE or PieceType.RED or PieceType.DARK)
                    {
                        builder.Append($"{piece.Type} {piece.HasCrown} ");
                    }
                    else
                    {
                        builder.Append($"{PieceType.NONE} {false} ");
                    }
                }

                lines[index++] = builder.ToString();
            }

            return lines;
        }

        public void ReadFromStream(List<string> lines, int size)
        {
            string line = lines[0];

            var items = line.Split(' ');

            first = new Player(items[0], (PieceType)Enum.Parse(typeof(PieceType), items[1]));
            second = new Player(items[2], (PieceType)Enum.Parse(typeof(PieceType), items[3]));
            multipleJumps = bool.Parse(items[4]);

            lines.RemoveAt(0);

            table = GetPieces(lines, size);
        }

        private static Piece[][] GetPieces(List<string> lines, int size)
        {
            Piece[][] table = CreateTable(size);

            for (int i = 0; i < lines.Count; ++i)
            {
                var items = lines[i].Split(' ');

                for (int j = 0; j < items.Length - 1; j += 2)
                {
                    PieceType type = (PieceType)Enum.Parse(typeof(PieceType), items[j]);
                    bool hasCrown = bool.Parse(items[j + 1]);

                    table[i][j / 2].Type = type;
                    table[i][j / 2].HasCrown = hasCrown;
                }
            }

            return table;
        }

        private static Piece[][] CreateTable(int size)
        {
            Piece[][] table = new Piece[size][];

            for (int i = 0; i < size; ++i)
            {
                table[i] = new Piece[size];

                for (int j = 0; j < size; ++j)
                {
                    table[i][j] = new Piece(new Position(i, j));
                }
            }

            return table;
        }
    }
}
