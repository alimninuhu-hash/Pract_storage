using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace WarehouseInventory
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        string connectionString =
            @"Server=DESKTOP-4DPV39S\SQLEXPRESS;
              Database=Tovary;
              Trusted_Connection=True;
              TrustServerCertificate=True;";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query =
                    @"INSERT INTO Users(Login, Password)
                      VALUES(@login, @password)";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@login", LoginBox.Text);

                command.Parameters.AddWithValue(
                    "@password", PasswordBox.Password);

                command.ExecuteNonQuery();

                MessageBox.Show("Аккаунт создан");

                Close();
            }
        }
    }
}