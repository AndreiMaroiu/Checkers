using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace joc_dame.ViewModels
{
    static class PieceImages
    {
        public static string None
        {
            get;
            private set;
        }

        public static string Red
        {
            get;
            private set;
        }

        public static string Dark
        {
            get;
            private set;
        }

        public static string RedWithCrown
        {
            get;
            private set;
        }

        public static string DarkWithCrown
        {
            get;
            private set;
        }

        public static string MoveDot
        {
            get;
            private set;
        }

        public static string Jump
        {
            get;
            private set;
        }

        internal static void Set(List<string> paths)
        {
            const string resources = "/joc_dame;component/Images/";

            if (paths.Count >= 7)
            {
                None = resources + paths[0];

                Red = resources + paths[1];
                Dark = resources + paths[2];

                RedWithCrown = resources + paths[3];
                DarkWithCrown = resources + paths[4];

                MoveDot = resources + paths[5];
                Jump = resources + paths[6];
            }
        }
    }
}
