using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WarehouseInventory
{
    public partial class AddEditWindow : Window
    {
        string connectionString =
            @"Server=DESKTOP-4DPV39S\SQLEXPRESS;
              Database=Tovary;
              Trusted_Connection=True;";

        public int ProductId = 0;

        public AddEditWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // НАЗВАНИЕ

            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show(
                    "Введите название товара");

                return;
            }

            // КАТЕГОРИЯ

            if (CategoryBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Выберите категорию");

                return;
            }

            // КОЛИЧЕСТВО

            if (!int.TryParse(
                    QuantityBox.Text,
                    out int quantity))
            {
                MessageBox.Show(
                    "Количество должно быть числом");

                return;
            }

            if (quantity < 0)
            {
                MessageBox.Show(
                    "Количество не может быть отрицательным");

                return;
            }

            // ЦЕНА

            if (!decimal.TryParse(
                    PriceBox.Text,
                    out decimal price))
            {
                MessageBox.Show(
                    "Цена должна быть числом");

                return;
            }

            if (price < 0)
            {
                MessageBox.Show(
                    "Цена не может быть отрицательной");

                return;
            }

            // ПОСТАВЩИК

            if (string.IsNullOrWhiteSpace(SupplierBox.Text))
            {
                MessageBox.Show(
                    "Введите поставщика");

                return;
            }

            // ДАТА

            if (DeliveryDatePicker.SelectedDate == null)
            {
                MessageBox.Show(
                    "Выберите дату поставки");

                return;
            }

            try
            {
                using (SqlConnection connection =
                       new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query;

                    // ДОБАВЛЕНИЕ

                    if (ProductId == 0)
                    {
                        query =
                        @"INSERT INTO Product
                        (
                            Name,
                            Category,
                            Quantity,
                            Price,

                            Supplier,
                            DeliveryDate
                        )
                        VALUES
                        (
                            @Name,
                            @Category,
                            @Quantity,
                            @Price,

                            @Supplier,
                            @DeliveryDate
                        )";
                    }

                    // РЕДАКТИРОВАНИЕ

                    else
                    {
                        query =
                        @"UPDATE Product
                        SET
                            Name = @Name,
                            Category = @Category,
                            Quantity = @Quantity,
                            Price = @Price,
                            Supplier = @Supplier,
                            DeliveryDate = @DeliveryDate
                        WHERE Id = @Id";
                    }

                    SqlCommand command =
                        new SqlCommand(query, connection);

                    // НАЗВАНИЕ

                    command.Parameters.Add(
                        "@Name",
                        System.Data.SqlDbType.NVarChar)
                        .Value = NameBox.Text;

                    // КАТЕГОРИЯ

                    command.Parameters.Add(
                        "@Category",
                        System.Data.SqlDbType.NVarChar)
                        .Value =
                        ((ComboBoxItem)
                        CategoryBox.SelectedItem)
                        .Content.ToString();

                    // КОЛИЧЕСТВО

                    command.Parameters.Add(
                        "@Quantity",
                        System.Data.SqlDbType.Int)
                        .Value = quantity;

                    // ЦЕНА

                    command.Parameters.Add(
                        "@Price",
                        System.Data.SqlDbType.Decimal)
                        .Value = price;

                    // ОБЩАЯ СТОИМОСТЬ

                

                    // ПОСТАВЩИК

                    command.Parameters.Add(
                        "@Supplier",
                        System.Data.SqlDbType.NVarChar)
                        .Value = SupplierBox.Text;

                    // ДАТА

                    command.Parameters.Add(
                        "@DeliveryDate",
                        System.Data.SqlDbType.Date)
                        .Value =
                        DeliveryDatePicker
                        .SelectedDate.Value;

                    // ID

                    if (ProductId != 0)
                    {
                        command.Parameters.Add(
                            "@Id",
                            System.Data.SqlDbType.Int)
                            .Value = ProductId;
                    }

                    command.ExecuteNonQuery();

                    MessageBox.Show(
                        "Данные успешно сохранены");

                    DialogResult = true;

                    Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка");
            }
        }
    }
}