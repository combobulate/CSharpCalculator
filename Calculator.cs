using System;
using System.Windows.Forms;

namespace CSharpCalculator
{
    public partial class Calculator : Form
    {
        double total = 0;
        string operation = "=";
        bool lastPressOperation = true;

        public Calculator()
        {
            InitializeComponent();
            resetResult();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numberButton(button1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numberButton(button2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            numberButton(button3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numberButton(button4.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            numberButton(button5.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            numberButton(button6.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            numberButton(button7.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            numberButton(button8.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            numberButton(button9.Text);
        }

        private void button0_Click(object sender, EventArgs e)
        {
            numberButton(button0.Text);
        }

        private void buttonPeriod_Click(object sender, EventArgs e)
        {
            numberButton(buttonPeriod.Text);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            // Resets display and stored value to 0
            resetResult();
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            calculate(operation);
            operation = "=";
            lastPressOperation = true;
        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            operationButton(buttonPlus.Text);
        }

        private void buttonMinus_Click(object sender, EventArgs e)
        {
            operationButton(buttonMinus.Text);
        }

        private void buttonTimes_Click(object sender, EventArgs e)
        {
            operationButton(buttonTimes.Text);
        }

        private void buttonDivides_Click(object sender, EventArgs e)
        {
            operationButton(buttonDivides.Text);
        }

        private void resetResult()
        {
            resultText.Text = displayResult("0");
            operation = "=";
        }

        private void calculate(string operation)
        {
            // Calculates the new total and displayed result based on the last operation button pressed.
            switch (operation)
            {
                case "+":
                    total += double.Parse(resultText.Text);
                    break;
                case "-":
                    total -= double.Parse(resultText.Text);
                    break;
                case "*":
                    total *= double.Parse(resultText.Text);
                    break;
                case "/":
                    total /= double.Parse(resultText.Text);
                    break;
                case "=":
                    total = double.Parse(resultText.Text);
                    break;
            }
            resultText.Text = displayResult(total.ToString());
        }

        private void numberButton(string buttonValue)
        {
            if (lastPressOperation)
                // If last button pressed before this number was an operation, new button press
                // starts a new number
            {
                if (buttonValue != ".")
                    resultText.Text = displayResult(buttonValue);
                else
                    // if the new button press was a decimal, prepend the 0 for the new number
                    resultText.Text = displayResult("0" + buttonValue);
                lastPressOperation = false;
            }
            else if (resultText.Text.Replace(".","").Length < 9)
                // Only accept new characters on the number if less than 10 digits, excluding the decimal
            {
                if (buttonValue != ".")
                    // Otherwise, if the new input is numeric:
                    if (resultText.Text == "0")
                        // Use as the start of the new number if current number is 0
                        resultText.Text = displayResult(buttonValue);
                    else
                        // Append to current number if current number is not 0
                        resultText.Text = displayResult(resultText.Text + buttonValue);
                else if (!resultText.Text.Contains("."))
                    // If new input is a decimal, append to current number only if there isn't one already
                    resultText.Text = displayResult(resultText.Text + buttonPeriod.Text);
            }
        }

        private void operationButton (string opButtonValue)
        {
            if (operation != "=")
                calculate(operation);
            else
            {
                // Store the current value for math
                total = double.Parse(resultText.Text);
                // Display the next operation at the end of the display string
                resultText.Text += opButtonValue;
            }
                
            operation = opButtonValue;
            lastPressOperation = true;
        }

        private string displayResult(string input)
        {
            if (input.Length > 9)
                if (input.Contains("."))
                    return input.Substring(0, 10);
                else
                    return input.Substring(0, 9);
            else
                return input;
        }
    }
}
