using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DarkTradeManager
{
    public partial class DashboardWindow : Window
    {
        public ObservableCollection<ItemEntity> AllItems { get; set; }
        public ObservableCollection<ItemEntity> FilteredItems { get; set; }
        public UserEntity CurrentUser { get; set; }

        public DashboardWindow(UserEntity user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
            LoadItems();
            LoadManufacturers();
        }

        private void LoadItems()
        {
            using (var context = new DarkDbContext())
            {
                var items = context.Items.ToList();
                AllItems = new ObservableCollection<ItemEntity>(items);
            }
            FilterItems();
        }

        private void LoadManufacturers()
        {
            var manufacturers = AllItems.Select(i => i.Manufacturer).Distinct();
            foreach (var m in manufacturers)
            {
                CmbManufacturer.Items.Add(new ComboBoxItem { Content = m });
            }
        }

        private void FilterItems()
        {
            string searchTerm = TxtSearch.Text.ToLower();
            string selectedManu = (CmbManufacturer.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Все производители";

            var filtered = AllItems.Where(i =>
                (string.IsNullOrEmpty(searchTerm) ||
                 i.Name.ToLower().Contains(searchTerm) ||
                 i.Description.ToLower().Contains(searchTerm) ||
                 i.Manufacturer.ToLower().Contains(searchTerm)) &&
                (selectedManu == "Все производители" || i.Manufacturer == selectedManu)
            );

            FilteredItems = new ObservableCollection<ItemEntity>(filtered);
            DgItems.ItemsSource = FilteredItems;
            TxtItemCount.Text = $"{FilteredItems.Count} из {AllItems.Count}";
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItems();
        }

        private void CmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterItems();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow auth = new AuthWindow();
            auth.Show();
            this.Close();
        }
    }
}
