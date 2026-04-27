using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDataBaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDataBaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Categoria=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Categoria, p.Quantidade, p.Preco, p.Id);
        }

        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // NOVO: Busca apenas produtos de uma categoria específica
        public Task<List<Produto>> SearchByCategoria(string categoria)
        {
            return _conn.Table<Produto>().Where(p => p.Categoria == categoria).ToListAsync();
        }

        // NOVO: Calcula o total gasto em uma categoria específica
        public async Task<double> GetTotalGastoPorCategoria(string categoria)
        {
            var lista = await SearchByCategoria(categoria);
            return lista.Sum(p => p.Total);
        }
    }
}