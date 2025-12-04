using System.Globalization;
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
        bool newSong = false;
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public string SongLenght { get; set; } = "";
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
            LoadData();
        }

        private void LoadData()
        {
            List<string> styles = new List<string>();
            szamokList.Items.Clear();
            if (comboStilus.Items.Count > 1)
            {
                comboStilus.Items.Clear();
            }

            //ComboBoxItem def = new ComboBoxItem();
            //def.Content = "Kérem válasszon egy műfajt";
            //def.Name = "DefComb";
            //comboStilus.Items.Add(def);
            foreach (var szam in szamok)
            {
                ListBoxItem item = new ListBoxItem();
                item.Style = (Style)FindResource("szamokListItem");

                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Auto) });
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

            for (int i = 0; i < styles.Count(); i++)
            {
                var style = styles[i];
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = style;
                if (style.Length < 5)
                {
                    style += "aaa";
                }
                newItem.Name = $"{style[0]}{style[1]}{style[2]}{style[3]}";
                comboStilus.Items.Add(newItem);
            }
            selectedSongLenght.Text = "00:01";
            selectedStyle.Text = "stilus1,stilus2,stilus3";
        }

        private void Modify(object sender, RoutedEventArgs e)
        {
            saveButton.Content = "Szám módosítása";
            displaySongInfo();
            if (sender is Button btn)
            {
                var parentGrid = VisualTreeHelper.GetParent(btn);

                parentGrid = getCointainingGrid(parentGrid);
                enableInputs(true);
            }
        }

        private static DependencyObject? getCointainingGrid(DependencyObject? parentGrid)
        {
            while (parentGrid != null && !(parentGrid is ListBoxItem))
            {
                parentGrid = VisualTreeHelper.GetParent(parentGrid);
            }

            if (parentGrid is ListBoxItem lbi)
            {
                lbi.IsSelected = true;
            }

            return parentGrid;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (newSong)
            {
                if (LenghtErrorCheck(selectedSongLenght))
                {
                    Szamok sz = new Szamok();
                    sz.Cim = selectedSongTitle.Text;
                    sz.Szerzo = selectedSongInstrumentAuthor.Text;
                    sz.Keletkezes = int.Parse(selectedSongOrigin.Text);
                    sz.Szovegiro = selectedSongLyricsAuthor.Text;
                    sz.Kiadva = (bool)selectedSongIsItOut.IsChecked!;
                    sz.Hossz = double.Parse(selectedSongLenght.Text.Replace(":", ","));
                    if (selectedStyle.Text.Contains(","))
                    {
                       var asd = new List<string>();
                        foreach (var item in selectedStyle.Text.Split(","))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                asd.Add(item.Trim()) ;
                            }
                        }
                        sz.Stilus = asd;
                    }
                    else
                    {
                        var asd = new List<string>();
                        asd.Add(selectedStyle.Text);
                        sz.Stilus = asd;
                    }
                    szamok.Add(sz);
                    newSong = false;
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
                            if (LenghtErrorCheck(selectedSongLenght))
                            {
                                sz.Cim = selectedSongTitle.Text;
                                sz.Szerzo = selectedSongInstrumentAuthor.Text;
                                sz.Keletkezes = int.Parse(selectedSongOrigin.Text);
                                sz.Szovegiro = selectedSongLyricsAuthor.Text;
                                sz.Kiadva = (bool)selectedSongIsItOut.IsChecked!;
                                sz.Hossz = double.Parse(selectedSongLenght.Text.Replace(":", ","));
                                if (selectedStyle.Text.Contains(","))
                                {
                                    var asd = new List<string>();
                                    foreach (var qwe in selectedStyle.Text.Split(","))
                                    {
                                        if (!string.IsNullOrEmpty(qwe))
                                        {
                                            asd.Add(qwe.Trim());
                                        }
                                    }
                                    sz.Stilus = asd;
                                }
                                else
                                {
                                    var asd = new List<string>();
                                    asd.Add(selectedStyle.Text);
                                    sz.Stilus = asd;
                                }
                            }
                        }
                    }
                }
            }
            using var file = File.Create(filename);
            JsonSerializer.Serialize(file, szamok, options);
            file.Close();
            szamok = LoadFromJson(filename);
            resetInputs();
            LoadData();
        }

        private bool LenghtErrorCheck(TextBox data)
        {
            var split = data.Text.Split(':');
            if (!(int.TryParse(split[0], out var m)) || !(int.TryParse(split[1], out var s)))
            {
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

        private void DeleteSong(object sender, RoutedEventArgs e)
        {
            if (szamokList.SelectedItem is ListBoxItem item)
            {
                for (global::System.Int32 i = 0; i < szamok.Count(); i++)
                {
                    var sz = szamok[i];
                    if (sz.Cim?.Replace(" ", "_").Replace(".", "").Replace("(", "").Replace(")", "") == item.Name)
                    {
                        szamok.Remove(sz);
                        using var file = File.Create(filename);
                        JsonSerializer.Serialize(file, szamok, options);
                        resetInputs();
                        LoadData();
                    }
                }
            }

        }

        private void NewSong(object sender, RoutedEventArgs e)
        {
            enableInputs(true);
            resetInputs();
        }

        private void resetInputs()
        {
            newSong = true;
            szamokList.SelectedItem = string.Empty;
            saveButton.Content = "Új szám mentése";
            selectedSongTitle.Text = string.Empty;
            selectedSongLenght.Text = "00:01";
            selectedSongInstrumentAuthor.Text = string.Empty;
            selectedSongLyricsAuthor.Text = string.Empty;
            selectedSongOrigin.Text = string.Empty;
            selectedStyle.Text = "stilus1,stilus2,stilus3";
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
            selectedStyle.IsEnabled = value;
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
                        selectedStyle.Text = "";
                        foreach (var item1 in sz.Stilus!)
                        {
                            selectedStyle.Text += item1.ToString() + ",";
                        }
                    }
                }
            }
        }

        private void comboStilus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = (ComboBoxItem)comboStilus.SelectedItem;
            if (StyleStackPanel != null && selected != null )
            {
                if (selected == DefComb)
                {
                    StyleStackPanel.Children.Clear();
                    string name = selected.Name as string;
                    writeStylesOut(name);
                }
                else
                {
                    StyleStackPanel.Children.Clear();
                    string name = selected.Name as string;

                    writeStylesOut(name);
                    comboStilus.Items.Remove(DefComb);
                }

            }

        }

        private void writeStylesOut(string name)
        {
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
                if (selectedGenre.Text != "")
                {
                    StyleStackPanel.Children.Add(selectedGenre);
                }

            }
        }
    }
}