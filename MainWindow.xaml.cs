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
            table.DefaultView.RowFilter =
                $"Name LIKE '%{SearchBox.Text}%'";
        }

        // ФИЛЬТР

        private void CategoryFilterBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (CategoryFilterBox.SelectedItem is ComboBoxItem item)
            {
                string category = item.Content.ToString();

                if (category == "Все категории")
                {
                    table.DefaultView.RowFilter = "";
                }
                else
                {
                    table.DefaultView.RowFilter =
                        $"Category = '{category}'";
                }
            }
        }

        // СОРТИРОВКА

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            table.DefaultView.Sort =
                "Name ASC";
        }

        // ВЫХОД

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
