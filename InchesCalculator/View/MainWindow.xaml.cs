using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InchesCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;

            // Використовуємо регулярний вираз для вилучення чисел та операторів зі введеного рядка
            string pattern = @"(\d+\s*\'+\s*\d+(\s*\d*\/\d+)*)";
            MatchCollection matches = Regex.Matches(input, pattern);

            double resultInInches = 0;

            foreach (Match match in matches)
            {
                double value = ParseValue(match.Value);
                string operatorSymbol = GetOperatorSymbol(match.Value);

                if (operatorSymbol == "+")
                {
                    resultInInches += value;
                }
                else if (operatorSymbol == "-")
                {
                    resultInInches -= value;
                }
                else if (operatorSymbol == "*")
                {
                    resultInInches *= value;
                }
                else if (operatorSymbol == "/")
                {
                    resultInInches /= value;
                }
            }

            // Конвертуємо результат з дюймів у фути та дюйми
            int feet = (int)(resultInInches / 12);
            double inches = resultInInches % 12;

            // Виводимо результат
            ResultFeetLabel.Text = $"{feet}' {inches}\"";
            ResultInchesLabel.Text = $"{resultInInches}\"";
        }

        private double ParseValue(string input)
        {
            // Використовуємо регулярний вираз для вилучення числових значень зі строки
            string pattern = @"(\d+)";
            MatchCollection matches = Regex.Matches(input, pattern);

            double value = 0;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    double number = double.Parse(match.Value);
                    value += number;
                }
            }

            return value;
        }

        private string GetOperatorSymbol(string input)
        {
            // Використовуємо регулярний вираз для вилучення операторів (+, -, *, /) зі строки
            string pattern = @"([+\-*/])";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }
    }
}
