using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char lastInput;
        private bool reset = true;

        public MainWindow()
        {
            InitializeComponent();
            calculatorNumbers.Text = "0";
        }

        /// <summary>
        /// When the user presses any of the button in the UI, this method is used.
        /// The method uses the button content/title in order to write up an arithmetic expression.
        /// Once the user presses "=" it will calculate the arithmetic expression and display the results in the calculator window.
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">event, not used</param>
        private void CalculatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button buttonObject = sender as Button; // get the button
            string button = buttonObject.Content.ToString(); // get the string from the button

            string input = calculatorNumbers.Text; // current input
            int length = input.Length;

            // execute a function depending on the button
            switch (button)
            {
                case "C":
                    calculatorNumbers.Text = "0";
                    reset = true;
                    break;
                case "R":
                    calculatorNumbers.Text = calculatorNumbers.Text.Remove(length - 1);
                    if (length <= 1 || calculatorNumbers.Text == "0" || reset == true) { goto case "C"; } // if input is 0 or resetted, goto case C.
                    break;
                case "=":
                    if (reset) break;
                    calculatorNumbers.Text = Calculate(input);
                    reset = true;
                    break;
                case "/":
                case "*":
                case "-":
                case "+":
                case ".":
                    if (!char.IsDigit(lastInput)) // if last input isn't a digit (an arithmetic sign) and the input isn't empty
                        calculatorNumbers.Text = calculatorNumbers.Text.Remove(length - 1); // remove the last input, and add the new arithmetic sign down below 
                    goto default;
                default:
                    if (reset)
                    {
                        reset = false;
                        calculatorNumbers.Text = button is "/" or "*" ? "0" + button : button; // if it's resetted and user presses / or *, add a zero in front in order to prevent arithmetic errors
                    }
                    else
                    {
                        calculatorNumbers.Text += button;
                    }

                    break;
            }

            lastInput = button.ToCharArray()[0]; // get current button (which becomes lastInput next time user presses button)
        }

        /// <summary>
        /// Calculates the arithmetic expression with the inbuilt Convert.ToDouble method.
        /// </summary>
        /// <param name="numbers">The arithmetic expression the user have created throughout the UI</param>
        /// <returns>the result from the arithmetic expression</returns>
        private string Calculate(string numbers)
        {
            if (!char.IsDigit(lastInput)) numbers = numbers.Remove(numbers.Length - 1); // if an arithmetic sign is at the end, remove it

            try
            {
                double result = Convert.ToDouble(new DataTable().Compute(numbers, null)); // convert string to double and calculate the result to a double
                return result.ToString();
            }
            catch (Exception)
            {
                return "Invalid expression";
            }
        }
    }
}
