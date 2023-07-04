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

            try
            {
                double resultInInches = EvaluateExpression(input);

                // Конвертуємо результат з дюймів у фути та дюйми
                int feet = (int)(resultInInches / 12);
                double inches = resultInInches % 12;

                // Виводимо результат
                ResultFeetLabel.Text = $"{feet}' {inches}\"";
                ResultInchesLabel.Text = $"{resultInInches:F4}\"";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private double EvaluateExpression(string input)
        {
            // Використовуємо регулярний вираз для вилучення чисел, операторів та вимірювань зі введеного рядка
            string pattern = @"(\d+\s*'\s*\d+(\s*\d*\/\d+)*|\d+\s*\.\s*\d+|[\+\-\*\/])";
            MatchCollection matches = Regex.Matches(input, pattern);

            double resultInInches = 0;
            string currentOperator = "+";

            foreach (Match match in matches)
            {
                string value = match.Value.Trim();

                if (IsMeasurement(value))
                {
                    double measurementInInches = ConvertToInches(value);
                    resultInInches = PerformOperation(resultInInches, measurementInInches, currentOperator);
                }
                else
                {
                    currentOperator = value;
                }
            }

            return resultInInches;
        }

        private bool IsMeasurement(string input)
        {
            // Перевіряємо, чи є введене значення вимірюванням (формат футів та дюймів)
            string pattern = @"(\d+\s*'\s*\d+(\s*\d*\/\d+)*)";
            return Regex.IsMatch(input, pattern);
        }

        private double ConvertToInches(string measurement)
        {
            // Конвертуємо вимірювання з формату футів та дюймів в дюйми
            string pattern = @"(\d+\s*'\s*\d+(\s*\d*\/\d+)*)";
            Match match = Regex.Match(measurement, pattern);

            if (match.Success)
            {
                string[] parts = match.Value.Split('\'');
                int feet = int.Parse(parts[0].Trim());
                string[] inchesParts = parts[1].Trim().Split(' ');
                int wholeInches = int.Parse(inchesParts[0].Trim());
                double fractionalInches = 0;

                if (inchesParts.Length > 1)
                {
                    string[] fractionParts = inchesParts[1].Split('/');
                    int numerator = int.Parse(fractionParts[0].Trim());
                    int denominator = int.Parse(fractionParts[1].Trim());
                    fractionalInches = numerator / (double)denominator;
                }

                return feet * 12 + wholeInches + fractionalInches;
            }

            throw new ArgumentException("Неправильний формат вимірювання.");
        }

        private double PerformOperation(double operand1, double operand2, string operatorSymbol)
        {
            // Виконуємо відповідну арифметичну операцію з двома операндами та оператором
            switch (operatorSymbol)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "*":
                    return operand1 * operand2;
                case "/":
                    return operand1 / operand2;
                default:
                    throw new ArgumentException("Невідомий оператор.");
            }
        }
    }
}

