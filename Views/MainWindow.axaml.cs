using Avalonia.Controls;
using Echorium.Utils;

namespace Echorium.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowsManager.MainWindowRef = this;
        }
    }
}