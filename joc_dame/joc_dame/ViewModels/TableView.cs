using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using joc_dame.Models;

namespace joc_dame.ViewModels
{
    public class TableView
    {
        private PieceView[][] table;
        private int size;

        public TableView(int size)
        {
            this.size = size;

            InitTable();
        }

        public PieceView[][] Pieces => table;

        public void SetTable(Piece[][] pieces)
        {
            for(int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    table[i][j].SetImage(pieces[i][j]);
                }
            }
        }

        private void InitTable()
        {
            table = new PieceView[size][];

            for (int i = 0; i < size; ++i)
            {
                table[i] = new PieceView[size];
            }

            foreach (var row in table)
            {
                for (int i = 0; i < size; ++i)
                {
                    row[i] = new PieceView();
                }
            }
        }
    }
}
