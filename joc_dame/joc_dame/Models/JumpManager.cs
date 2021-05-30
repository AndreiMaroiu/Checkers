using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace joc_dame.Models
{
    class JumpManager
    {
        private Piece[][] table;
        private int size;
        private bool multipleJumps;

        private Dictionary<Position, Jump> jumps;
        private HashSet<Position> starts = new();

        public JumpManager(Piece[][] table)
            => (this.table, size) = (table, table.Length);

        public bool HasJumps
        {
            get; private set;
        } = false;

        public bool CanSwitch
        {
            get; private set;
        } = false;

        private bool IsInTable(Position position)
        {
            if(position.x < 0 || position.x >= size)
            {
                return false;
            }

            if(position.y < 0 || position.y >= size)
            {
                return false;
            }

            return true;
        }

        private Jump SearchDirection(Position start, Position direction, PieceType opponent)
        {
            Position current = start + direction;

            if(!IsInTable(current))
            {
                return null;
            }
            
            if(table[current.x][current.y].Type == opponent)
            {
                Position next = current + direction;

                if(!IsInTable(next))
                {
                    return null;
                }

                if(table[next.x][next.y].Type == PieceType.NONE)
                {
                    return new Jump(start, current, next);
                }
            }

            return null;
        }

        private List<Jump> SearchPiece(Piece piece, PieceType oppenent, int iter)
        {
            if(iter > 5)
            {
                return new List<Jump>();
            }

            List<Jump> jumps = new();
            List<Position> directions;

            if(piece.HasCrown)
            {
                directions = Directions.AllDirections;
            }
            else
            {
                directions = Directions.PieceDirections[piece.Type];
            }

            foreach(var direction in directions)
            {
                var result = SearchDirection(piece.Position, direction, oppenent);

                if(result is not null)
                {
                    jumps.Add(result);
                }
            }

            if (multipleJumps)
            {
                int lenght = jumps.Count;

                for (int i = 0; i < lenght; ++i)
                {
                    var tempPiece = new Piece(table[jumps[i].where.x][jumps[i].where.y]);
                    tempPiece.Type = piece.Type;
                    tempPiece.HasCrown = piece.HasCrown;
                    var aux = SearchPiece(tempPiece, oppenent, iter + 1);

                    jumps[i].next.AddRange(aux);

                    foreach (var jump in aux)
                    {
                        jump.previous = jumps[i];
                    }

                    if (aux.Count > 0)
                    {
                        jumps.AddRange(aux);
                    }
                }
            }

            return jumps;
        }

        public List<Jump> SearchForJumps(PieceType current, PieceType oppenent)
        {
            jumps = new Dictionary<Position, Jump>();
            starts = new HashSet<Position>();

            for(int i = 0; i < size; ++i)
            {
                for(int j = 0; j < size; ++j)
                {
                    if (table[i][j].Type == current)
                    {
                        var list = SearchPiece(table[i][j], oppenent, 0);

                        if(list.Count > 0)
                        {
                            starts.Add(table[i][j].Position);

                            foreach(var item in list)
                            {
                                if (!jumps.ContainsKey(item.where))
                                {
                                    jumps.Add(item.where, item);
                                }
                            }

                        }
                    }
                }
            }

            if(jumps.Count > 0)
            {
                HasJumps = true;
                return jumps.Values.ToList();
            }

            HasJumps = false;
            return null;
        }

        private void ClearTable()
        {
            foreach(var pair in jumps)
            {
                if (table[pair.Key.x][pair.Key.y].Type is PieceType.JUMP)
                {
                    table[pair.Key.x][pair.Key.y].Type = PieceType.NONE;
                }
            }
        }

        private void RecalculateJumps(Jump start)
        { 
            jumps = new Dictionary<Position, Jump>();
            starts = new HashSet<Position>();
            starts.Add(start.from);

            Queue<Jump> queue = new();
            queue.Enqueue(start);

            foreach (var jump in start.next)
            {
                jump.previous = null;
            }

            while (queue.Count > 0)
            {
                Jump peek = queue.Dequeue();

                foreach (var jump in peek.next)
                {
                    if (!jumps.ContainsKey(jump.where))
                    {
                        jumps.Add(jump.where, jump);
                        queue.Enqueue(jump);
                    }
                }
            }

            foreach(var pair in jumps)
            {
                table[pair.Key.x][pair.Key.y].Type = PieceType.JUMP;
            }
        }

        public void HandleMandatories(Position where)
        {
            CanSwitch = false;

            if(table[where.x][where.y].Type is PieceType.JUMP)
            {
                var jump = jumps[where];
                var initialJump = jump;

                do
                {
                    table[where.x][where.y].Set(table[jump.from.x][jump.from.y]);
                    table[jump.over.x][jump.over.y].Type = PieceType.NONE;
                    table[jump.from.x][jump.from.y].Type = PieceType.NONE;

                    jump = jump.previous;
                }
                while (jump is not null);


                if(initialJump.next.Count > 0)
                {
                    CanSwitch = false;
                    ClearTable();
                    RecalculateJumps(initialJump);
                }
                else
                {
                    CanSwitch = true;
                    ClearTable();
                }

            }
        }

        public bool MultipleJumps
        {
            get => multipleJumps;
            set => multipleJumps = value;
        }
    }
}
