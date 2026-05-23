using KitaRehberi.Models;

namespace KitaRehberi;

public partial class MainPage : ContentPage
{
    List<Kita> tumKitalar = new();

    public MainPage()
    {
        InitializeComponent();

        tumKitalar = KitalariGetir();

        cvKitalar.ItemsSource = tumKitalar;
    }

    private List<Kita> KitalariGetir()
    {
        return new List<Kita>
        {
            new Kita
            {
                Ad = "Avrupa",
                Nufus = 745000000,
                Aciklama = "Tarih, sanat ve kültürel çeşitlilik açısından oldukça zengin bir kıta.",
                Resim = "avrupa.jpg"
            },

            new Kita
            {
                Ad = "Asya",
                Nufus = 4760000000,
                Aciklama = "Dünyanın en büyük ve en kalabalık kıtası.",
                Resim = "asya.jpg"
            },

            new Kita
            {
                Ad = "Afrika",
                Nufus = 1300000000,
                Aciklama = "Doğal kaynakları ve kültürel çeşitliliğiyle dikkat çeker.",
                Resim = "afrika.jpg"
            },

            new Kita
            {
                Ad = "Kuzey Amerika",
                Nufus = 592000000,
                Aciklama = "Teknoloji ve ekonomi açısından güçlü ülkeler içerir.",
                Resim = "kuzeyamerika.jpg"
            },

            new Kita
            {
                Ad = "Güney Amerika",
                Nufus = 434000000,
                Aciklama = "Amazon ormanları ve eşsiz doğasıyla bilinir.",
                Resim = "guneyamerika.jpg"
            },

            new Kita
            {
                Ad = "Okyanusya",
                Nufus = 43000000,
                Aciklama = "Avustralya ve ada ülkelerinden oluşur.",
                Resim = "okyanusya.jpg"
            },

            new Kita
            {
                Ad = "Antarktika",
                Nufus = 5000,
                Aciklama = "Dünyanın en soğuk kıtası.",
                Resim = "antarktika.jpg"
            }
        };
    }

    // Arama
    private void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        string aranan = searchBar.Text?.ToLower() ?? "";

        var filtreliListe = tumKitalar
            .Where(x => x.Ad.ToLower().Contains(aranan))
            .ToList();

        cvKitalar.ItemsSource = filtreliListe;
    }

    // Favori
    private async void Favori_Clicked(object sender, EventArgs e)
    {
        SwipeItem item = (SwipeItem)sender;

        Kita secilenKita = item.BindingContext as Kita;

        if (secilenKita != null)
        {
            await DisplayAlert(
                "Favori",
                $"{secilenKita.Ad} favorilere eklendi ⭐",
                "Tamam");
        }
    }

    // Seçim
    private async void cvKitalar_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Kita secilenKita)
        {
            await DisplayAlert(
                secilenKita.Ad,
                secilenKita.Aciklama,
                "Kapat");
        }
    }
}