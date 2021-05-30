using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using joc_dame.Models;

namespace joc_dame.ViewModels
{
    public class PieceView : INotifyPropertyChanged
    {
        private string imagePath;

        public PieceView()
        {
            imagePath = null;
        }

        public void SetImage(Piece piece)
        {
            if(piece.Type is PieceType.MOVABLE)
            {
                Image = PieceImages.MoveDot;
                return;
            }

            if(piece.Type is PieceType.JUMP)
            {
                Image = PieceImages.Jump;
                return;
            }

            if(!piece.HasCrown)
            {
                switch(piece.Type)
                {
                    case PieceType.RED:
                        Image = PieceImages.Red;
                        break;
                    case PieceType.DARK:
                        Image = PieceImages.Dark;
                        break;
                    default:
                        Image = PieceImages.None;
                        break;
                }
            }
            else
            {
                switch (piece.Type)
                {
                    case PieceType.RED:
                        Image = PieceImages.RedWithCrown;
                        break;
                    case PieceType.DARK:
                        Image = PieceImages.DarkWithCrown;
                        break;
                    default:
                        Image = PieceImages.None;
                        break;
                }
            }
        }

        public string Image
        {
            get => imagePath;

            set
            {
                imagePath = value;

                OnPropertyChanged();
            }
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
