using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CalculatorGame;

public partial class MenuWindow : UserControl
{
    private readonly MainWindow _mainWindow = null!;
    
    private const string ScorePath = "scores.txt";
    private const string OptionPath = "options.txt";
    
    public MenuWindow()
    {
        InitializeComponent();
    }
    
    public MenuWindow(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        PutDatasToBox();
        ShowScores();
    }
    
    public void OnStart(object? sender, RoutedEventArgs e)
    {
        (bool, string, string)[] toSend = [((bool) Add.IsChecked!, AddS.Text, AddE.Text)!, 
            ((bool) Sub.IsChecked!, SubS.Text, SubE.Text)!, 
            ((bool) Mul.IsChecked!, MulS.Text, MulE.Text)!, 
            ((bool) Sqr.IsChecked!, SqrS.Text, SqrE.Text)!];

        foreach ((bool isChecked, string text1, string text2) in toSend)
        {
            if (!isChecked) continue;

            bool isTextNumeric = int.TryParse(text1, out int value1);
            if (!isTextNumeric) return;

            
            isTextNumeric = int.TryParse(text2, out int value2);
            if (!isTextNumeric) return;

            if (value1 >= value2) return;
        }

        bool isNumeric = int.TryParse(QAmount.Text ?? "0", out int amount);
        if (!isNumeric) return;
        
        SaveBoxDatas();
        _mainWindow.ShowGame(amount, toSend[0], toSend[1], toSend[2], toSend[3]);
    }
    
    private static string ReadToFile(string path)
    {
        using var outputFile = new StreamReader(path);
        string fileLines = outputFile.ReadToEnd();
        outputFile.Close();
        
        return fileLines;
    }

    private void PutDatasToBox()
    {
        string file = ReadToFile(OptionPath);

        string[] fileLines = file.Split("\n");

        if (fileLines.Length == 1) return;

        QAmount.Text = fileLines[0];

        string[] splittedLineAdd = fileLines[1].Split(" ");
        Add.IsChecked = splittedLineAdd[0] == "True";
        if (splittedLineAdd[1] != "") AddS.Text = splittedLineAdd[1];
        if (splittedLineAdd[2] != "") AddE.Text = splittedLineAdd[2];
        
        string[] splittedLineSub = fileLines[2].Split(" ");
        Sub.IsChecked = splittedLineSub[0] == "True";
        if (splittedLineSub[1] != "") SubS.Text = splittedLineSub[1];
        if (splittedLineSub[2] != "") SubE.Text = splittedLineSub[2];
        
        string[] splittedLineMul = fileLines[3].Split(" ");
        Mul.IsChecked = splittedLineMul[0] == "True";
        if (splittedLineMul[1] != "") MulS.Text = splittedLineMul[1];
        if (splittedLineMul[2] != "") MulE.Text = splittedLineMul[2];
        
        string[] splittedLineSqr = fileLines[4].Split(" ");
        Sqr.IsChecked = splittedLineSqr[0] == "True";
        if (splittedLineSqr[1] != "") SqrS.Text = splittedLineSqr[1];
        if (splittedLineSqr[2] != "") SqrE.Text = splittedLineSqr[2];
    }

    public void SaveBoxDatas()
    {
        string lineToWrite = $"{QAmount.Text}\n" +
                          $"{Add.IsChecked} {AddS.Text} {AddE.Text}\n" +
                          $"{Sub.IsChecked} {SubS.Text} {SubE.Text}\n" +
                          $"{Mul.IsChecked} {MulS.Text} {MulE.Text}\n" +
                          $"{Sqr.IsChecked} {SqrS.Text} {SqrE.Text}";
        WriteToFile(lineToWrite);
    }

    private void ShowScores()
    {
        string fileInfo = ReadToFile(ScorePath);

        string[] fileLines = fileInfo.Split("\n");
        
        if (fileLines.Length != 1) ListScore.Items.Clear();
        
        foreach (string line in fileLines)
        {
            string[] lineInfo = line.Split(" ");
            if (lineInfo.Length == 1) continue;

            float percentage = float.Parse(lineInfo[1]) / float.Parse(lineInfo[3]) * 100;
            var scoreItem = new ScoreItem(lineInfo[0], lineInfo[1], lineInfo[2], lineInfo[3], lineInfo[4], (int) percentage);
            ListScore.Items.Add(scoreItem);
        }
    }
    
    private static void WriteToFile(string toWrite)
    {
        using var outputFile = new StreamWriter(OptionPath, false);
        outputFile.WriteLine(toWrite);
        outputFile.Close();
    }
}