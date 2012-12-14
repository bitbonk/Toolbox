namespace Adornment
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string Data
        {
            get { return "data from DataContext"; }
        }
    }
}