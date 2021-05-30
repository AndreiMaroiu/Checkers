using System;
using System.Collections.Generic;
using System.Windows;

namespace joc_dame.Models
{
    public class Game
    {
        private Piece[][] table;
        private int size;
        private Player currentPlayer;
        private Player nextPlayer;
        private Piece currentPiece;
        private JumpManager jumpManager;

        private List<Piece> availableMoves;

        public Game(int size)
        {
            this.size = size;

            InitTable();
            PlacePieces();

            availableMoves = new List<Piece>();
            jumpManager = new JumpManager(table);

            currentPlayer = new Player("Achi", PieceType.RED);
            nextPlayer = new Player("Andrei", PieceType.DARK);
        }

        private void InitTable()
        {
            table = new Piece[size][];

            for (int i = 0; i < size; ++i)
            {
                table[i] = new Piece[size];
            }

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    table[i][j] = new Piece(new Position(i, j));
                }
            }
        }

        private void PlacePieces()
        {
            for (int i = size - 3; i < size; ++i)
            {
                for (int j = (i + 1) % 2; j < size; j += 2)
                {
                    table[i][j].Type = PieceType.RED;
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                for (int j = (i + 1) % 2; j < size; j += 2)
                {
                    table[i][j].Type = PieceType.DARK;
                }
            }

            //table[0][1].Type = PieceType.DARK;
            //table[1][2].Type = PieceType.DARK;
            //table[0][7].Type = PieceType.DARK;

            //table[7][6].Type = PieceType.RED;
        }

        public void HandleClick(Position where)
        {
            if (!CanClick(where))
            {
                return;
            }

            if(jumpManager.HasJumps)
            {
                WithMandatories(where);
            }
            else
            {
                NoMandatories(where);
            }

            ChecksForPieces();
            CheckForCrowns();
        }

        public void SetMultipleJumps(bool value)
        {
            jumpManager.MultipleJumps = value;
        }

        public void LoadGame(GameData data)
        {
            table = data.Table;
            currentPlayer = data.FirstPlayer;
            nextPlayer = data.SecondPlayer;
            jumpManager = new(table);
            jumpManager.MultipleJumps = data.MultipleJumps;
            HandleMandatories();
        }

        public void SetPlayers(Player first, Player second)
        {
            currentPlayer = first;
            nextPlayer = second;
        }

        private void WithMandatories(Position where)
        {
            jumpManager.HandleMandatories(where);

            if(jumpManager.CanSwitch)
            {
                SwitchPlayers();
                HandleMandatories();
            }
        }

        private void NoMandatories(Position where)
        {
            if (table[where.x][where.y].Type == currentPlayer.Type)
            {
                currentPiece = new Piece(table[where.x][where.y]);
                CalculateMoves();
            }
            else if (table[where.x][where.y].Type == PieceType.MOVABLE && currentPiece is not null)
            {
                table[currentPiece.Position.x][currentPiece.Position.y].Type = PieceType.NONE;
                table[where.x][where.y].Set(currentPiece);
                currentPiece = null;

                SwitchPlayers();
                ResetMoves();

                HandleMandatories();
            }
        }

        private void SwitchPlayers()
        {
            var copy = currentPlayer;

            currentPlayer = nextPlayer;
            nextPlayer = copy;
        }

        private void ResetMoves()
        {
            foreach (var move in availableMoves)
            {
                if (table[move.Position.x][move.Position.y].Type is PieceType.MOVABLE)
                {
                    table[move.Position.x][move.Position.y].Type = PieceType.NONE;
                }
            }

            availableMoves.Clear();
        }

        private void HandleMandatories()
        {
            var jumps = jumpManager.SearchForJumps(currentPlayer.Type, nextPlayer.Type);

            if (jumps is not null)
            {
                foreach (var jump in jumps)
                {
                    table[jump.where.x][jump.where.y].Type = PieceType.JUMP;
                }
            }
        }

        private bool CheckPosition(Position where)
        {
            if (where.x < 0 || where.x >= size)
            {
                return false;
            }
            if (where.y < 0 || where.y >= size)
            {
                return false;
            }

            if (table[where.x][where.y].Type is PieceType.NONE)
            {
                return true;
            }

            return false;
        }

        private void CalculateMove(Piece piece, List<Position> directions)
        {
            Position where = piece.Position;

            foreach(var direction in directions)
            {
                var pos = where + direction;
                if (CheckPosition(pos))
                {
                    availableMoves.Add(table[pos.x][pos.y]);
                    table[pos.x][pos.y].Type = PieceType.MOVABLE;
                }
            }
        }

        private void CalculateMoves()
        {
            ResetMoves();

            if (currentPiece.HasCrown)
            {
                CalculateMove(currentPiece, Directions.AllDirections);
            }
            else
            {
                CalculateMove(currentPiece, Directions.PieceDirections[currentPiece.Type]);
            }
        }

        private void CheckForCrowns()
        {
            for (int i = 0; i < size; ++i)
            {
                if(table[0][i].Type is PieceType.RED)
                {
                    table[0][i].HasCrown = true;
                }
                else if(table[size - 1][i].Type is PieceType.DARK)
                {
                    table[size - 1][i].HasCrown = true;
                }
            }
        }

        private void ChecksForPieces()
        {
            int red = 0;
            int dark = 0;

            for(int i = 0; i < size; ++i)
            {
                for(int j = 0; j < size; ++j)
                {
                    if(table[i][j].Type is PieceType.RED)
                    {
                        ++red;
                    }
                    else if(table[i][j].Type is PieceType.DARK)
                    {
                        ++dark;
                    }
                }
            }

            if(red is 0)
            {
                SetWinner(PieceType.DARK);
            }
            else if(dark is 0)
            {
                SetWinner(PieceType.RED);
            }

        }

        private void SetWinner(PieceType type)
        {
            if(currentPlayer.Type == type)
            {
                Winner = currentPlayer;
            }
            else
            {
                Winner = nextPlayer;
            }
        }

        public bool CanClick(Position where)
        {
            if (where.x % 2 == 0 && where.y % 2 == 0)
            {
                return false;
            }

            if (where.x % 2 == 1 && where.y % 2 == 1)
            {
                return false;
            }

            return true;
        }

        public Piece[][] Table 
        {
            get => table;
            set
            {
                if(value is not null)
                {
                    table = value;
                }
            }
        }

        public Player CurrentPlayer => currentPlayer;

        public Player NextPlayer => nextPlayer;

        public Player Winner
        {
            get; private set;
        } = null;

        public GameData data => new GameData(currentPlayer, nextPlayer, table, jumpManager.MultipleJumps);
    }
}
