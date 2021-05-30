using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joc_dame.ViewModels
{
    public class PlayersVM
    {
        public string RedPlayer
        {
            get; set;
        }

        public string BlackPlayer
        {
            get; set;
        }

        public ButtonCommand Done
        {
            get; set;
        }
    }
}
