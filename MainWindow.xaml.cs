using System.Data;
using System.Data.SqlClient;
using System.Windows;

/* 1 коммит создание базы данных
 * 2 коммит создание интерфейса главного экрана
 * 3 коммит реализация отображения базы данных
 * 4 коммит дополнительные окна(создание товара,редактирование товара)
 * 5 коммит реализация функций сортировки и фильтрации,создание и удаление товара,удаления и редактирования
 
 */
namespace WarehouseInventory
{
    public partial class MainWindow : Window
    {
        string connectionString =
            @"Server=DESKTOP-4DPV39S\SQLEXPRESS;
              Database=Tovary;
              Trusted_Connection=True;";

        public MainWindow()
        {
            InitializeComponent();

            LoadProducts();
        }

        private void LoadProducts()
        {
            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Product";

                SqlDataAdapter adapter =
                    new SqlDataAdapter(query, connection);

                DataTable table = new DataTable();

                adapter.Fill(table);

                ProductsGrid.ItemsSource =
                    table.DefaultView;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
