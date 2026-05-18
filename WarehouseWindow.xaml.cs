using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using WarehouseInventory;

namespace WarehouseInventory
{
    public partial class WarehouseWindow : Window
    {
        string connectionString =
            @"Server=DESKTOP-4DPV39S\SQLEXPRESS;
              Database=Tovary;
              Trusted_Connection=True;
              TrustServerCertificate=True;";

        public WarehouseWindow() => InitializeComponent();

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Password;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    @"SELECT COUNT(*)
                      FROM Users
                      WHERE Login = @login
                      AND Password = @password";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@login", login);

                command.Parameters.AddWithValue(
                    "@password", password);

                int count =
                    (int)command.ExecuteScalar();

                if (count > 0)
                {
                    MainWindow window = new MainWindow();
             

                    window.Show();

                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Неверный логин или пароль");
                }
            }
        }
    }
}