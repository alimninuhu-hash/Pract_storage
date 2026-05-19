using System;
using System.Data.SqlClient;
using System.Windows;

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

                command.Parameters.AddWithValue(
                    "@Name", NameBox.Text);

                command.Parameters.AddWithValue(
                    "@Category", CategoryBox.Text);

                command.Parameters.AddWithValue(
                    "@Quantity",
                    Convert.ToInt32(QuantityBox.Text));

                command.Parameters.AddWithValue(
                    "@Price",
                    Convert.ToDecimal(PriceBox.Text));

                command.Parameters.AddWithValue(
                    "@Supplier", SupplierBox.Text);

                command.Parameters.AddWithValue(
                    "@DeliveryDate",
                    DeliveryDatePicker.SelectedDate);

                if (ProductId != 0)
                {
                    command.Parameters.AddWithValue(
                        "@Id", ProductId);
                }

                command.ExecuteNonQuery();

                DialogResult = true;

                Close();
            }
        }
    }
}