using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int SIZE_OF_FIELD = 10;
        private int IDX = 0;
        private int MINE_PERCENTAGE = 10; //Percentage of mine to empty field ratio

        private ObservableCollection<MineSweepButton>? itemList;

        public MainWindow()
        {
            InitializeComponent();

            Reset_Game();
        }

        private void Reset_Game()
        {
            IDX = 0;
            Random random = new Random();

            itemList = new ObservableCollection<MineSweepButton>();

            for (int i = 0; i < Math.Pow(SIZE_OF_FIELD, 2); i++)
            {
                itemList.Add(new MineSweepButton() { Mine = random.Next(100) < MINE_PERCENTAGE ? "x" : "0", Visible = Visibility.Hidden, Enabled = true});
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Mine == "x") continue;
                int mineCount = 0;
                int row = i % SIZE_OF_FIELD, column = i / SIZE_OF_FIELD;

                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (row + x >= 0 && row + x < SIZE_OF_FIELD && column + y >= 0 && column + y < SIZE_OF_FIELD && itemList[(row + x) + (column + y) * SIZE_OF_FIELD].Mine == "x")
                        {
                            mineCount++;
                        }
                    }
                }
                itemList[i].Mine = mineCount.ToString();
            }

            MainItemControl.ItemsSource = itemList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Button x = (Button)sender;

            int BtnID = Int32.Parse(x.Name.Split("_", 2)[1]);
            Click_On_Mine(BtnID);

            MainItemControl.ItemsSource = itemList;
        }

        public void Click_On_Mine(int BtnID)
        {
            if (itemList == null) { return; }

            MineSweepButton MineItem = itemList[BtnID];
            MineItem.Visible = Visibility.Visible;
            MineItem.Enabled = false;

            if (MineItem.Mine == "x") { GameOver(); return; }

            if (MineItem.Mine == "0")
            {
                for(int i = -1; i <= 1; i++)
                {
                    for (int k = -1; k <= 1; k++) 
                    {
                        int row = BtnID / SIZE_OF_FIELD, column = BtnID % SIZE_OF_FIELD;
                        int index = (row + i) * SIZE_OF_FIELD + k + column;

                        if ((i != 0 || k != 0)
                            && row + i >= 0 && column + k >= 0 && column + k < SIZE_OF_FIELD && row + i < SIZE_OF_FIELD
                            && itemList[index].Visible == Visibility.Hidden
                            )
                        { 
                            Click_On_Mine(index);
                        }
                    }
                }
            }
        }

        private void GameOver()
        {
            foreach(MineSweepButton i in itemList)
            {
                i.Visible = Visibility.Visible;
                i.Enabled = false;
            }
            MessageBox.Show("Game Over!");
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.Name = $"MenuBtn_{IDX}";
                IDX++;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset_Game();
        }
        public class MineSweepButton : INotifyPropertyChanged
        {
            private string? mine;
            public string? Mine
            {
                get { return mine; }
                set
                {
                    if(this.mine != value) 
                    {
                        this.mine = value;
                        this.NotifyPropertyChanged("Mine");
                    }
                }
            }
            private Visibility visible;
            public Visibility Visible
            {
                get { return visible; }
                set
                {
                    if (this.visible != value)
                    {
                        this.visible = value;
                        this.NotifyPropertyChanged("Visible");
                    }
                }
            }
            private bool enabled;
            public bool Enabled
            {
                get { return enabled; }
                set
                {
                    if (this.enabled != value)
                    {
                        this.enabled = value;
                        this.NotifyPropertyChanged("Enabled");
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
