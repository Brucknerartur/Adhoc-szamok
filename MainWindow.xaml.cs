using System.IO;
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
        const string filename = "adhocszamok.json";
        List<Szamok> szamok = LoadFromJson(filename);
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public MainWindow()
        {
            InitializeComponent();

            //TODO: Combo feltöltés
            //Onclick események (started)
            //Új/módosítás (started)

        }


        public static List<Szamok> LoadFromJson(string filename)
        {
            var jsonContent = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            var szamok = JsonSerializer.Deserialize<List<Szamok>>(jsonContent, options);
            return szamok ?? new List<Szamok>();

            //szamokList
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> styles = new List<string>();
            szamokList.Items.Clear();
            foreach (var szam in szamok)
            {
                ListBoxItem item = new ListBoxItem();
                item.Style = (Style)FindResource("szamokListItem");

                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)});
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Auto)});
                grid.Style = (Style)FindResource("szamokListItemGrid");


                Label label = new Label();
                label.Content = szam.Cim;
                label.Style = (Style)FindResource("szamokListItemGridLabel");
                    Grid.SetColumn(label, 0);
                Grid.SetRow(label, 0);

                grid.Children.Add(label);

                Button b = new Button();
                b.Content = "Módosítás";
                b.Style = (Style)FindResource("szamokListItemGridButton");
                b.SetValue(Grid.ColumnProperty, 1);
                b.Click += Modify;
                Grid.SetColumn(b, 1);
                Grid.SetRow(b, 0);
                grid.Children.Add(b);


                item.Content = grid;
                item.Name = szam.Cim?.Replace(" ", "_").Replace(".", "").Replace("(", "").Replace(")", "");

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
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = style;
                newItem.Name = $"{style[0]}{style[1]}{style[2]}{style[3]}";
                comboStilus.Items.Add(newItem);
            }
            selectedSongLenght.Text = "00:00";
        }

        private void Modify(object sender, RoutedEventArgs e)
        {
            saveButton.Content = "Szám módosítása";
            displaySongInfo();
            if (sender is Button btn)
            {
                var parentGrid = VisualTreeHelper.GetParent(btn);

                while (parentGrid != null && !(parentGrid is ListBoxItem))
                {
                    parentGrid = VisualTreeHelper.GetParent(parentGrid);
                }

                if (parentGrid is ListBoxItem lbi)
                {
                    lbi.IsSelected = true;
                }
                enableInputs(true);
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (szamokList.SelectedItem == null)
            {
                if (LenghtErrorCheck(selectedSongLenght.Text))
                {
                    Szamok sz = new Szamok();
                    sz.Cim = selectedSongTitle.Text;
                    sz.Szerzo = selectedSongInstrumentAuthor.Text;
                    sz.Keletkezes = int.Parse(selectedSongOrigin.Text);
                    sz.Szovegiro = selectedSongLyricsAuthor.Text;
                    sz.Kiadva = (bool)selectedSongIsItOut.IsChecked!;
                    sz.Hossz = double.Parse(selectedSongLenght.Text.Replace(":", ","));
                    using var file = File.Create(filename);
                    JsonSerializer.Serialize(file, szamok, options);
                }
            }
            else
            {
                if (szamokList.SelectedItem is ListBoxItem item)
                {
                    foreach (var sz in szamok)
                    {
                        if (sz.Cim?.Replace(" ", "_").Replace(".", "").Replace("(", "").Replace(")", "") == item.Name)
                        {
                            if (LenghtErrorCheck(selectedSongLenght.Text))
                            {
                                sz.Cim = selectedSongTitle.Text;
                                sz.Szerzo = selectedSongInstrumentAuthor.Text;
                                sz.Keletkezes = int.Parse(selectedSongOrigin.Text);
                                sz.Szovegiro = selectedSongLyricsAuthor.Text;
                                sz.Kiadva = (bool)selectedSongIsItOut.IsChecked!;
                                sz.Hossz = double.Parse(selectedSongLenght.Text.Replace(":", ","));
                            }
                        }
                    }
                    using var file = File.Create(filename);
                    JsonSerializer.Serialize(file, szamok, options);
                }
            }
        }

        private bool LenghtErrorCheck(string data)
        {
            var split = data.Split(':');
            if (!(int.TryParse(split[0], out var m)) || !(int.TryParse(split[1], out var s)))
            {
                var asd = new Setter();
                //selectedSongLenght.Style.Setters.Add();
                return false; // lenght format error
            }
            return true;
        }

        private bool NotFilledErrorCheck(Grid grid)
        {
            foreach (var item in grid.Children)
            {
                if (item is TextBox textBox && !string.IsNullOrEmpty(textBox.Text))
                {
                    return false; // not everything filled in
                }
            }
            return true;
        }

        private void NewSong(object sender, RoutedEventArgs e)
        {
            szamokList.SelectedItem = null;
            enableInputs(true);
            saveButton.Content = "Új szám mentése";
            selectedSongTitle.Text = null;
            selectedSongLenght.Text = "00:00";
            selectedSongInstrumentAuthor.Text = null;
            selectedSongLyricsAuthor.Text = null;
            selectedSongOrigin.Text = null;
            selectedSongIsItOut.IsChecked = false;
        }

        private void enableInputs(bool value)
        {
            selectedSongTitle.IsEnabled = value;
            selectedSongLenght.IsEnabled = value;
            selectedSongInstrumentAuthor.IsEnabled = value;
            selectedSongLyricsAuthor.IsEnabled = value;
            selectedSongOrigin.IsEnabled = value;
            selectedSongIsItOut.IsEnabled = value;
            saveButton.IsEnabled = value;
        }

        private void szamokList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displaySongInfo();
            enableInputs(false);
            saveButton.Content = "Mentés";
        }

        private void displaySongInfo()
        {
            if (szamokList.SelectedItem is ListBoxItem item)
            {
                foreach (var sz in szamok)
                {
                    if (sz.Cim?.Replace(" ", "_").Replace(".", "").Replace("(", "").Replace(")", "") == item.Name)
                    {
                        selectedSongTitle.Text = sz.Cim;
                        selectedSongLenght.Text = sz.Hossz.ToString().Replace(",", ":");
                        selectedSongInstrumentAuthor.Text = sz.Szerzo;
                        selectedSongLyricsAuthor.Text = sz.Szovegiro;
                        selectedSongOrigin.Text = sz.Keletkezes.ToString();
                        selectedSongIsItOut.IsChecked = sz.Kiadva;
                    }
                }
            }
        }

        private void comboStilus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StyleStackPanel != null)
            {
                StyleStackPanel.Children.Clear();
                comboStilus.Items.Remove(DefComb);
                ComboBoxItem selected = (ComboBoxItem)comboStilus.SelectedItem;
                string name = selected.Name as string;

                foreach (var sz in szamok)
                {
                    TextBlock selectedGenre = new TextBlock();
                    selectedGenre.Style = (Style)FindResource("searchSongTextblock");
                    selectedGenre.Text = "";
                    foreach (var stilo in sz.Stilus!)
                    {
                        if (stilo != null && $"{stilo[0]}{stilo[1]}{stilo[2]}{stilo[3]}" == name)
                        {
                            selectedGenre.Text = $"- {sz.Cim}";
                        }
                    }
                    if(selectedGenre.Text != "")
                    {
                        StyleStackPanel.Children.Add(selectedGenre);    
                    }

                }
                
            }
            
        }
    }
}