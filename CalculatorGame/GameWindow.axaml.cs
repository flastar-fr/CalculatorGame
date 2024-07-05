using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CalculatorGame;

public partial class GameWindow : UserControl
{
    private readonly Stopwatch _stopwatch = new();
    private readonly MainWindow _mainWindow = null!;
    
    private const string ScorePath = "scores.txt";

    private readonly Random _random;
    
    private int _currentAmount;
    private int? _currentResult;

    private int _goodAnswer;

    private readonly int _amount;
    private readonly (bool isChecked, string text1, string text2) _add;
    private readonly (bool isChecked, string text1, string text2) _sub;
    private readonly (bool isChecked, string text1, string text2) _mul;
    private readonly (bool isChecked, string text1, string text2) _sqr;
    
    public GameWindow()
    {
        InitializeComponent();
        _random = new Random();
    }
    
    public GameWindow(MainWindow mainWindow,
        int amountP,
        (bool, string, string) addP,
        (bool, string, string) subP,
        (bool, string, string) mulP,
        (bool, string, string) sqrP)
    {
        InitializeComponent();
        
        _mainWindow = mainWindow;
        _random = new Random();
        _amount = amountP;
        _add = addP;
        _sub = subP;
        _mul = mulP;
        _sqr = sqrP;
        
        StartGame();
    }

    public void GetToMenu(object? sender, RoutedEventArgs e)
    {
        _mainWindow.ShowMenu();
    }

    public void StartGame()
    {
        _currentAmount = 0;
        _currentResult = null;
        _stopwatch.Reset();
        _stopwatch.Start();
        GameStep();
    }
    
    private void InputTextBox_KeyDown(object? _, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        GameStep();
        e.Handled = true;
    }

    private void GameStep()
    {
        if (_currentResult != null)
        {
            bool isNumeric = int.TryParse(Result.Text, out int value);
            if (isNumeric)
            {
                if (value == _currentResult) _goodAnswer++;
            }

            Result.Text = "";
        }
        
        if (_currentAmount >= _amount)
        {
            ExprBlock.Text = "";
            EndGame();
            return;
        }
        
        List<(string expr, int result)> toChoseIn = []; 
            
        if (_add.isChecked)
        {
            int value1 = int.Parse(_add.text1);
            int value2 = int.Parse(_add.text2);

            int rValue1 = _random.Next(value1, value2);
            int rValue2 = _random.Next(value1, value2);
                
            toChoseIn.Add(($"{rValue1} + {rValue2}", rValue1 + rValue2));
        }

        if (_sub.isChecked)
        {
            int value1 = int.Parse(_sub.text1);
            int value2 = int.Parse(_sub.text2);

            int rValue1 = _random.Next(value1, value2);
            int rValue2 = _random.Next(value1, value2);
                
            toChoseIn.Add(($"{rValue1} - {rValue2}", rValue1 - rValue2));
        }
            
        if (_mul.isChecked)
        {
            int value1 = int.Parse(_mul.text1);
            int value2 = int.Parse(_mul.text2);

            int rValue1 = _random.Next(value1, value2);
            int rValue2 = _random.Next(value1, value2);
                
            toChoseIn.Add(($"{rValue1} \u00d7 {rValue2}", rValue1 * rValue2));
        }

        if (_sqr.isChecked)
        {
            int value1 = int.Parse(_sqr.text1);
            int value2 = int.Parse(_sqr.text2);

            int rValue = _random.Next(value1, value2);
            
            if (rValue == 0) rValue++;
                
            toChoseIn.Add(($"√{rValue * rValue}", rValue));
        }

        _currentAmount++;

        int rIndex = _random.Next(toChoseIn.Count);
        ExprBlock.Text = toChoseIn[rIndex].expr;

        _currentResult = toChoseIn[rIndex].result;
    }

    private void EndGame()
    {
        _stopwatch.Stop();
        
        long elaspedTime = _stopwatch.ElapsedMilliseconds;
        float meanTime = float.Round((float)elaspedTime / _amount / 1000, 2);
        var culture = new CultureInfo("en-GB");
        DateTime thisDay = DateTime.Today;
        var dateOnly = thisDay.ToString("d", culture);
        
        var lineToWrite = $"\n{elaspedTime / 1000} {_goodAnswer} {meanTime} {_amount} {dateOnly}";
        WriteToFile(lineToWrite);

        Result.KeyDown -= InputTextBox_KeyDown;

        ResultText.Text =
            $"Time : {elaspedTime / 1000}s, Good Answers : {_goodAnswer}, Average Time : {meanTime}s, Amount : {_amount}";
        ResultText.IsVisible = true;

        ToMenu.IsVisible = true;
        ToMenu.IsEnabled = true;
    }
    
    private static void WriteToFile(string toWrite)
    {
        using var outputFile = new StreamWriter(ScorePath, true);
        outputFile.WriteLine(toWrite);
        outputFile.Close();
    }
}