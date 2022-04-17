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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int SIZE_OF_FIELD = 10;
        private int IDX = 0;

        private List<MineSweepButton>? itemList;

        public MainWindow()
        {
            InitializeComponent();

            Reset_Game();
        }

        private void Reset_Game()
        {
            Random random = new Random();

            itemList = new List<MineSweepButton>();

            for (int i = 0; i < Math.Pow(SIZE_OF_FIELD, 2); i++)
            {
                itemList.Add(new MineSweepButton() { Mine = random.Next(100) > 90 ? "x" : "0" });
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
            Grid x_grid = (Grid)x.Content;
            TextBlock txtblock = (TextBlock)x_grid.Children[0];
            txtblock.Visibility = Visibility.Visible;

            int BtnID = Int32.Parse(x.Name.Split("_" , 2)[1]);
        }
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
    }

    public class MineSweepButton
    {
        public string? Mine { get; set; }
    }
}
