using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using joc_dame.Models;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace joc_dame.ViewModels
{
    static class GameSaver
    {
        public static void SaveGame(GameData data)
        {
            SaveFileDialog dialog = new();
            dialog.Filter = "Text file (*.txt)|*.txt";

            if (dialog.ShowDialog() is true)
            {
                try
                {
                    using StreamWriter writer = new StreamWriter(dialog.FileName);

                    foreach(var line in data.ToFile())
                    {
                        writer.WriteLine(line);
                    }
                }
                catch(IOException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static GameData LoadGame(int size)
        {
            OpenFileDialog dialog = new();

            if(dialog.ShowDialog() is true)
            {
                try
                {
                    using StreamReader reader = new(dialog.FileName);

                    List<string> lines = new();

                    for(int i = 0; i <= size; ++i)
                    {
                        lines.Add(reader.ReadLine());
                    }

                    return new GameData(lines, size);
                    
                }
                catch(IOException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            return null;
        }

    }
}
