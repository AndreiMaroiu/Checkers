using System.Windows;
using System.Windows.Input;

namespace joc_dame.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModels.GameViewModel gameView;

        public MainWindow()
        {
            InitializeComponent();
            gameView = new (this);
            this.DataContext = gameView;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gameView.HandleClick(e.GetPosition(tableGrid));
        }
    }
}
