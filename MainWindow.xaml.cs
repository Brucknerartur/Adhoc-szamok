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

            //TODO: Combo feltöltés
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
            List<string> styles = new List<string>();
            foreach (var szam in szamok)
            {
                ListBoxItem item = new ListBoxItem();
                item.Style = (Style)this.Resources["szamokListItem"];

                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)});
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Auto)});
                grid.Style = (Style)this.Resources["szamokListItemGrid"];


                Label label = new Label();
                label.Content = szam.Cim;
                label.Style = (Style)this.Resources["szamokListItemGridLabel"];
                Grid.SetColumn(label, 0);
                Grid.SetRow(label, 0);

                grid.Children.Add(label);

                Button b = new Button();
                b.Content = "Módosítás";
                b.Style = (Style)this.Resources["szamokListItemGridButton"];
                b.SetValue(Grid.ColumnProperty, 1);
                Grid.SetColumn(b, 1);
                Grid.SetRow(b, 0);
                grid.Children.Add(b);


                item.Content = grid;


                szamokList.Items.Add(item);

                foreach (var style in szam.Stilus!)
                {
                    if (!styles.Contains(style))
                    {
                        styles.Add(style);
                    }
                }
            }
            foreach (var style in styles)
            {
                //TODO
            }
        }
    }
}