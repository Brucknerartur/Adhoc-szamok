using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Adhoc_szamok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Szamok> szamok = LoadFromJson("adhocszamok.json");
        public MainWindow()
        {
            InitializeComponent();

            //TODO: Listbox + Combo feltöltés
            //Onclick események
            //Új/módosítás

        }


        public static List<Szamok> LoadFromJson(string filename)
        {
            var jsonContent = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            var szamok = JsonSerializer.Deserialize<List<Szamok>>(jsonContent, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return szamok ?? new List<Szamok>();

            //szamokList
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var szam in szamok)
            {

                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)});
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Auto)});
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.Width = 280;


                Label label = new Label();
                label.Content = szam.Cim;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, 0);
                grid.Children.Add(label);


                Button b = new Button();
                b.Content = "Módosítás";
                b.VerticalAlignment = VerticalAlignment.Center;
                b.Padding = new Thickness(3, 3, 3, 3);
                b.SetValue(Grid.ColumnProperty, 1);
                Grid.SetColumn(b, 1);
                Grid.SetRow(b, 0);
                grid.Children.Add(b);

                

                SzamokList.Items.Add(grid);
                //item.Children.Add(szam.Cim);
                  
            }
        }
    }
}