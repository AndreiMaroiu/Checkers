using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using joc_dame.Models;
using System.IO;
using System.Windows;

namespace joc_dame.ViewModels
{
    public class Statistics
    {
        private int redWins = 0;
        private int darkWins = 0;
        private string path;

        public Statistics(string path)
        {
            ReadStatiscs(path);
            this.path = path;
        }

        public int RedWins => redWins;

        public int DarkWins => darkWins;

        public void Write()
        {
            WriteStatistics(path);
        }

        public void UpdateStatistics(PieceType type)
        {
            if (type is PieceType.RED)
            {
                ++redWins;
            }
            else if (type is PieceType.DARK)
            {
                ++darkWins;
            }
        }

        private void ReadStatiscs(string path)
        {
            try
            {
                if(!File.Exists(path))
                {
                    File.Create(path);
                    return;
                }

                using StreamReader reader = new(path);

                string line = reader.ReadLine();

                var values = line.Split(' ');

                if(values.Length >= 2)
                {
                    redWins = int.Parse(values[0]);
                    darkWins = int.Parse(values[1]);
                }

            }
            catch(IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void WriteStatistics(string path)
        {
            try
            {
                using StreamWriter writer = new (path);

                string line = $"{redWins} {darkWins}";

                writer.WriteLine(line);
            }
            catch(IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
