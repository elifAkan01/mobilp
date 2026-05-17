namespace HesapMakinesiApp;

public partial class MainPage : ContentPage
{
    double ilkSayi = 0;
    string secilenIslem = "";
    bool yeniGiris = true;

    public MainPage()
    {
        InitializeComponent();
    }

    private void Rakam_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        if (lblSonuc.Text == "0" || yeniGiris)
        {
            lblSonuc.Text = btn.Text;
            yeniGiris = false;
        }
        else
        {
            lblSonuc.Text += btn.Text;
        }
    }

    private void Operator_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        ilkSayi = Convert.ToDouble(lblSonuc.Text);
        secilenIslem = btn.Text;
        lblIslem.Text = lblSonuc.Text + " " + secilenIslem;
        yeniGiris = true;
    }

    private void Esittir(object sender, EventArgs e)
    {
        double ikinciSayi = Convert.ToDouble(lblSonuc.Text);
        double sonuc = 0;

        switch (secilenIslem)
        {
            case "+":
                sonuc = ilkSayi + ikinciSayi;
                break;

            case "-":
                sonuc = ilkSayi - ikinciSayi;
                break;

            case "*":
                sonuc = ilkSayi * ikinciSayi;
                break;

            case "/":
                sonuc = ikinciSayi != 0 ? ilkSayi / ikinciSayi : 0;
                break;

            case "%":
                sonuc = ilkSayi % ikinciSayi;
                break;
        }

        lblSonuc.Text = sonuc.ToString();
        lblIslem.Text = "";
        yeniGiris = true;
    }

    private void Temizle(object sender, EventArgs e)
    {
        lblSonuc.Text = "0";
        lblIslem.Text = "";
        ilkSayi = 0;
        secilenIslem = "";
    }

    private void Sil(object sender, EventArgs e)
    {
        if (lblSonuc.Text.Length > 1)
            lblSonuc.Text = lblSonuc.Text.Remove(lblSonuc.Text.Length - 1);
        else
            lblSonuc.Text = "0";
    }
}