using System;
using System.Data;

using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace WarehouseInventory
{
    public partial class MainWindow : Window
    {
        string connectionString =
            @"Server=DESKTOP-4DPV39S\SQLEXPRESS;
              Database=Tovary;
              Trusted_Connection=True;";

        DataTable table = new DataTable();

        public MainWindow()
        {
            InitializeComponent();

            LoadProducts();
        }

        // ЗАГРУЗКА ТОВАРОВ

        private void LoadProducts()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    "SELECT * FROM Product";

                SqlDataAdapter adapter =
                    new SqlDataAdapter(query, connection);

                table.Clear();

                adapter.Fill(table);

                ProductsGrid.ItemsSource =
                    table.DefaultView;

                UpdateStatistics(table);
            }
        }

        // ОБНОВЛЕНИЕ СТАТИСТИКИ

        private void UpdateStatistics(DataTable table)
        {
            int totalProducts =
                table.Rows.Count;

            int totalQuantity = 0;

            decimal totalPrice = 0;

            decimal averagePrice = 0;

            foreach (DataRow row in table.Rows)
            {
                totalQuantity +=
                    Convert.ToInt32(row["Quantity"]);

                totalPrice +=
                    Convert.ToDecimal(row["TotalPrice"]);

                averagePrice +=
                    Convert.ToDecimal(row["Price"]);
            }

            if (table.Rows.Count > 0)
            {
                averagePrice =
                    averagePrice / table.Rows.Count;
            }

            TotalProductsText.Text =
                totalProducts.ToString();

            TotalQuantityText.Text =
                totalQuantity.ToString();

            TotalPriceText.Text =
                $"{totalPrice:N2} ₽";

        }

        // ДОБАВЛЕНИЕ

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow window =
                new AddEditWindow();

            if (window.ShowDialog() == true)
            {
                LoadProducts();
            }
        }

        // РЕДАКТИРОВАНИЕ

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите товар");

                return;
            }

            DataRowView row =
                (DataRowView)ProductsGrid.SelectedItem;

            AddEditWindow window =
                new AddEditWindow();

            window.ProductId =
                Convert.ToInt32(row["Id"]);

            window.NameBox.Text =
                row["Name"].ToString();

            window.CategoryBox.Text =
                row["Category"].ToString();

            window.QuantityBox.Text =
                row["Quantity"].ToString();

            window.PriceBox.Text =
                row["Price"].ToString();

            window.SupplierBox.Text =
                row["Supplier"].ToString();

            window.DeliveryDatePicker.SelectedDate =
                Convert.ToDateTime(row["DeliveryDate"]);

            if (window.ShowDialog() == true)
            {
                LoadProducts();
            }
        }

        // УДАЛЕНИЕ

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите товар");

                return;
            }

            DataRowView row =
                (DataRowView)ProductsGrid.SelectedItem;

            int id =
                Convert.ToInt32(row["Id"]);

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    "DELETE FROM Product WHERE Id = @Id";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@Id", id);

                command.ExecuteNonQuery();
            }

            LoadProducts();
        }

        // ПОИСК

        private void SearchBox_TextChanged(
      object sender,
      TextChangedEventArgs e)
        {
            ApplyFilters();
        }
        // ФИЛЬТР

        private void ApplyFilters()
        {
            string filter = "";

            // ПОИСК

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string search =
                    SearchBox.Text
                    .Replace("'", "''");

                filter =
                    $"Name LIKE '%{search}%'";
            }

            // КАТЕГОРИЯ

            if (CategoryFilterBox.SelectedItem
                is ComboBoxItem item)
            {
                string category =
                    item.Content.ToString();

                if (category != "Все категории")
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " AND ";
                    }

                    category =
                        category.Replace("'", "''");

                    filter +=
                        $"Category = '{category}'";
                }
            }

            table.DefaultView.RowFilter = filter;
        }

        // СОРТИРОВКА

        private void SortBox_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
        {
            if (SortBox.SelectedItem is ComboBoxItem item)
            {
                string sort =
                    item.Content.ToString();

                switch (sort)
                {
                    case "Без сортировки":
                        table.DefaultView.Sort = "";
                        break;


                    case "Название ↑":
                        table.DefaultView.Sort =
                            "Name DESC";
                        break;

                    case "Название ↓":
                        table.DefaultView.Sort =
                            "Name ASC";
                        break;


                    case "Количество ↑":
                        table.DefaultView.Sort =
                            "Quantity DESC";
                        break;

                    case "Количество ↓":
                        table.DefaultView.Sort =
                            "Quantity ASC";
                        break;

                    case "Цена ↑":
                        table.DefaultView.Sort =
                            "Price DESC";
                        break;

                    case "Цена ↓":
                        table.DefaultView.Sort =
                            "Price ASC";
                        break;

                    case "Дата ↑":
                        table.DefaultView.Sort =
                            "DeliveryDate DESC";
                        break;

                    case "Дата ↓":
                        table.DefaultView.Sort =
                            "DeliveryDate ASC";
                        break;


                    case "Категория ↑":
                        table.DefaultView.Sort =
                            "Category DESC";
                        break;

                    case "Категория ↓":
                        table.DefaultView.Sort =
                            "Category ASC";
                        break;
                }
            }
        }

        // ВЫХОД

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CategoryFilterBox_SelectionChanged(
     object sender,
     SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
