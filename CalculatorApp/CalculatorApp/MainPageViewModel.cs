using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Data;

namespace CalculatorApp;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _expressionDisplay = string.Empty;

    [ObservableProperty]
    private string _resultDisplay = string.Empty;

    [RelayCommand]
    public void HandleButtonPress(string buttonText)
    {
        switch (buttonText)
        {
            case "AC":
                ExpressionDisplay = string.Empty;
                ResultDisplay = string.Empty;
                break;

            case "DE":
                if (!string.IsNullOrEmpty(ExpressionDisplay))
                {
                    ExpressionDisplay = ExpressionDisplay.Substring(0, ExpressionDisplay.Length - 1);
                    UpdateResult();
                }
                break;

            case "=":
                try
                {
                    var result = new DataTable().Compute(ReplaceOperators(ExpressionDisplay), null);
                    ResultDisplay = result.ToString() ?? string.Empty;
                }
                catch
                {
                    ResultDisplay = "Error";
                }
                break;

            case "x":
            case "/":
            case "+":
            case "-":
            case "%":
                ExpressionDisplay += buttonText;
                UpdateResult();
                break;

            case "(   )":
                ExpressionDisplay += GetParenthesis();
                break;

            default: // sayılar ve "."
                ExpressionDisplay += buttonText;
                UpdateResult();
                break;
        }
    }

    private void UpdateResult()
    {
        try
        {
            if (!string.IsNullOrEmpty(ExpressionDisplay))
            {
                var result = new DataTable().Compute(ReplaceOperators(ExpressionDisplay), null);
                ResultDisplay = result.ToString() ?? string.Empty;
            }
        }
        catch
        {
            ResultDisplay = string.Empty;
        }
    }

    private string ReplaceOperators(string expr)
    {
        // XAML’de "x" var ama DataTable "*" bekler
        return expr.Replace("x", "*");
    }

    private string GetParenthesis()
    {
        int openCount = ExpressionDisplay.Count(c => c == '(');
        int closeCount = ExpressionDisplay.Count(c => c == ')');

        if (openCount > closeCount)
            return ")";
        else
            return "(";
    }
}
