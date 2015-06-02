using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace NewsReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush _activeColor;
        public ObservableCollection<TabItem> TabItems { get; set; }

        public MainWindow()
        {
            TabItems = new ObservableCollection<TabItem>();
            
            _activeColor = Brushes.White;
            LoadTabsData();
            InitializeOptionsTab();
            InitializeComponent();
            DataContext = this;
        }

        private void LoadTabsData(int itemsCount = 20)
        {
            InitializeTab(Links.Home, "Główna", itemsCount);
            InitializeTab(Links.Sport, "Sport", itemsCount);
            InitializeTab(Links.Business, "Biznes", itemsCount);
            InitializeTab(Links.Weather, "Pogoda", itemsCount);
            InitializeTab(Links.Technology, "Technologia", itemsCount);
        }

        private void InitializeOptionsTab()
        {
            var tabItem = new TabItem() {Header = "Opcje"};
            var content = new Options();
            content.ComboBox.SelectionChanged += ComboBoxOnSelectionChanged;
            content.SetNewsCount.Click += SetNewsCountOnClick;
            tabItem.Content = content;
            TabItems.Add(tabItem);
            ChangeColor(_activeColor);
        }

        private void SetNewsCountOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var button = (Button) sender;
            var parent = (StackPanel)button.Parent;
            var box = (TextBox)parent.FindName("NewsCount");
            var count = box.Text;

            if (!string.IsNullOrWhiteSpace(count))
            {
                TabItems.Clear();
                LoadTabsData(int.Parse(count));                
                InitializeOptionsTab();
            }
        }

        private void ComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var combo = (ComboBox) sender;
            var item = (TextBlock)combo.SelectedItem;

            switch (item.Text)
            {
                case "Czerwone":
                    ChangeColor(Brushes.Red);
                    break;
                case "Zielone":
                    ChangeColor(Brushes.LightGreen);
                    break;
                case "Niebieskie":
                    ChangeColor(Brushes.DeepSkyBlue);
                    break;
                default:
                    ChangeColor(Brushes.White);
                    break;
            }
        }

        private void ChangeColor(Brush color)
        {
            _activeColor = color;
            Background = color;
            foreach (var tabItem in TabItems)
            {
                tabItem.Background = color;
                var feed = (UserControl)tabItem.Content;
                if (tabItem.Content is RSSFeed)
                {
                    var rss = (RSSFeed) tabItem.Content;
                    rss.List.Background = color;
                }
                feed.Background = color;
            }
        }

        private void InitializeTab(string url, string title, int itemsCount = 20)
        {
            var reader = new MyXmlReader(url);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();           
            
            if (feed == null) return;

            var tab = new TabItem {Header = title};

            var content = new RSSFeed {DataContext = new {Feeds = feed.Items.Take(itemsCount)}};
            tab.Content = content;
            TabItems.Add(tab);
        }
    }
}
