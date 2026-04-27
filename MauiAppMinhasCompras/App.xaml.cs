using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDataBaseHelper _db;

        public static SQLiteDataBaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras_v2.db3"); // MUDAMOS O NOME AQUI PARA CRIAR UM BANCO NOVO

                    _db = new SQLiteDataBaseHelper(path);
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");

            MainPage = new NavigationPage(new MauiAppMinhasCompras.Views.ListaProduto());
        }
    }
}