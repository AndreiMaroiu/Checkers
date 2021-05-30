using joc_dame.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using joc_dame.Views;

namespace joc_dame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        const int TABLESIZE = 8;
        const int UNITSIZE = 400 / 8;

        private Window main;
        private Game game;
        private TableView tableView;
        private Player currentPlayer;
        private Statistics statistics;

        private Player first, second;

        private bool isTableOpen;
        private bool menuVisibility;
        private bool multipleJumps;

        public GameViewModel(Window main)
        {
            this.main = main;

            IsTableOpen = false;
            MenuVisibility = true;
            MultipleJumps = true;
            ResetCommand = new ButtonCommand(Reset, ButtonCommand.defaultCanExecute);
            NewGameCommnad = new ButtonCommand(NewGame, ButtonCommand.defaultCanExecute);
            ExitToMenu = new ButtonCommand(CloseGame, ButtonCommand.defaultCanExecute);
            OpenStatisticsCommand = new ButtonCommand(OpenStatistics, ButtonCommand.defaultCanExecute);
            SaveGameCommand = new ButtonCommand(SaveGame, ButtonCommand.defaultCanExecute);
            LoadGameCommand = new ButtonCommand(LoadGame, ButtonCommand.defaultCanExecute);
            OpenAboutCommand = new ButtonCommand(OpenAbout, ButtonCommand.defaultCanExecute);
            statistics = new Statistics("Statistics.txt");

            GetImagesPath("../../../imagesPaths.txt");

            game = new Game(TABLESIZE);
            tableView = new TableView(TABLESIZE);
            tableView.SetTable(game.Table);
        }

        public string TestPath
            => "/joc_dame;component/Images/dot.png";

        public Player CurrentPlayer
        {
            get => currentPlayer;

            set
            {
                currentPlayer = value;
                OnPropertyChanged();
            }
        }

        public TableView Table => tableView;

        public ButtonCommand ResetCommand
        {
            get; init;
        }

        public ButtonCommand NewGameCommnad
        {
            get; init;
        }

        public ButtonCommand ExitToMenu
        {
            get; init;
        }

        public ButtonCommand OpenStatisticsCommand
        {
            get; init;
        }

        public ButtonCommand SaveGameCommand
        {
            get; init;
        }

        public ButtonCommand LoadGameCommand
        {
            get; init;
        }

        public ButtonCommand OpenAboutCommand
        {
            get; init;
        }

        public bool IsTableOpen
        {
            get => isTableOpen;

            set
            {
                isTableOpen = value;
                OnPropertyChanged();
            }
        }

        public bool MenuVisibility
        {
            get => menuVisibility;

            set
            {
                menuVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool MultipleJumps
        {
            get => multipleJumps;

            set
            {
                multipleJumps = value;
                OnPropertyChanged();
            }
        }

        private void Reset(object o)
        {
            game = new Game(TABLESIZE);
            game.SetMultipleJumps(MultipleJumps);
            tableView.SetTable(game.Table);
            CurrentPlayer = game.CurrentPlayer;

            game.SetPlayers(first, second);
        }

        private void NewGame(object o)
        {
            PlayersVM playersVM = new();
            PlayerSelection playerSelection = new (playersVM);
            playerSelection.Owner = main;
            playersVM.Done = new ButtonCommand((object o) => playerSelection.Close(), ButtonCommand.defaultCanExecute);
            playerSelection.ShowDialog();

            game = new Game(TABLESIZE);
            tableView.SetTable(game.Table);
            first = new Player(playersVM.RedPlayer, PieceType.RED);
            second = new Player(playersVM.BlackPlayer, PieceType.DARK);

            if(first.Name == string.Empty || first.Name == null)
            {
                first.Name = "Red";
            }
            if(second.Name == string.Empty || second.Name == null)
            {
                second.Name = "Black";
            }

            game.SetPlayers(first, second);

            OpenGame();
        }

        private void OpenGame()
        {
            IsTableOpen = true;
            MenuVisibility = false;

            game.SetMultipleJumps(MultipleJumps);

            CurrentPlayer = game.CurrentPlayer;
        }

        private void OpenAbout(object o)
        {
            About about = new About();
            about.Show();
        }

        private void CloseGame(object o)
        {
            IsTableOpen = false;
            MenuVisibility = true;

            statistics.Write();
        }

        private void OpenStatistics(object o)
        {
            StatisticsWindow window = new(statistics);
            window.Show();
        }

        private void SaveGame(object o)
        {
            GameSaver.SaveGame(game.data);
        }

        private void LoadGame(object o)
        {
            game.LoadGame(GameSaver.LoadGame(TABLESIZE));
            tableView.SetTable(game.Table);

            OpenGame();
        }

        public void HandleClick(Point where)
        {
            Point point = new(where.Y / UNITSIZE, where.X / UNITSIZE);
            Position click = (Position)point;

            game.HandleClick(click);
            tableView.SetTable(game.Table);
            CurrentPlayer = game.CurrentPlayer;

            if (game.Winner is not null)
            {
                MessageBox.Show($"winner is: {game.Winner.Name}");
                statistics.UpdateStatistics(game.Winner.Type);
                statistics.Write();
            }
        }

        private void GetImagesPath(string path)
        {
            try
            {
                using StreamReader reader = new(path);

                List<string> paths = new();

                string line;

                while ((line = reader.ReadLine()) is not null)
                {
                    paths.Add(line);
                }

                PieceImages.Set(paths);
            }
            catch (IOException exception)
            {
                MessageBox.Show($"Exception: {exception.Message}");
            }
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
