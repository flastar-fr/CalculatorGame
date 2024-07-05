using Avalonia.Controls;

namespace CalculatorGame;

public partial class ScoreItem : UserControl
{
    public ScoreItem()
    {
        InitializeComponent();
    }
    
    public ScoreItem(string time, string amountCa, string meanTime, string amount, string date, int percentage)
    {
        InitializeComponent();
        Time.Text = time + "s";
        AmountCa.Text = amountCa;
        MeanTime.Text = meanTime + "s";
        Amount.Text = amount;
        Date.Text = date;
        Percentage.Text = percentage + "%";
    }
}