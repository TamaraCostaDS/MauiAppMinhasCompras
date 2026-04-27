using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista_produtos = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista_produtos;
    }

    protected override async void OnAppearing()
    {
        try
        {
            lista_produtos.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            tmp.ForEach(i => lista_produtos.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // BOTÃO ADICIONAR (ToolbarItem_Clicked)
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // BOTÃO SOMAR (ToolBarItem_Clicked_1) - O Relatório!
    private async void ToolBarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            string categoria = await DisplayActionSheet("Relatório de Gastos", "Cancelar", null, "Alimentos", "Limpeza", "Higiene", "Outros");

            if (categoria != "Cancelar" && categoria != null)
            {
                double total = await App.Db.GetTotalGastoPorCategoria(categoria);
                await DisplayAlert("Total Gasto", $"Na categoria {categoria}, o total é: {total:C}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string busca = e.NewTextValue;
            lista_produtos.Clear();
            List<Produto> tmp;

            if (string.IsNullOrEmpty(busca))
                tmp = await App.Db.GetAll();
            else
                tmp = await App.Db.SearchByCategoria(busca);

            tmp.ForEach(i => lista_produtos.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Apenas para evitar erro de seleção, mas o relatório principal está no botão Somar
        ((ListView)sender).SelectedItem = null;
    }

    private void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        OnAppearing();
        lst_produtos.IsRefreshing = false;
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem m = sender as MenuItem;
            Produto p = m.BindingContext as Produto;

            if (await DisplayAlert("Confirmação", $"Deseja remover {p.Descricao}?", "Sim", "Não"))
            {
                await App.Db.Delete(p.Id);
                lista_produtos.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}