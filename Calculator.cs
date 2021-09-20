using System;
using System.Windows.Forms;

namespace CSharpCalculator
{
    public partial class Calculator : Form
    {
        double total = 0;
        string operation = "=";
        bool lastPressOperation = false;
        bool showHistory = true;

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
            operationButton(buttonEquals.Text);
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
            resultText.Text = "0";
            total = 0;
            operation = "=";
            lastPressOperation = false;
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
            else
            {
                string testLen = resultText.Text.Replace(".", "");
                testLen = testLen.Replace(",", "");
                testLen = testLen.Replace("-", "");

                if (testLen.Length < 16)
                    // Only accept new characters on the number if less than 15 significant digits
                {
                    if (buttonValue != ".")
                        // If the new input is numeric:
                        if (resultText.Text == "0")
                            // If current display number is 0, replace with input number
                            resultText.Text = displayResult(buttonValue);
                        else
                            // If current display number is not 0, append input number
                            resultText.Text = displayResult(resultText.Text + buttonValue);
                    else if (!resultText.Text.Contains("."))
                        // If new input is a decimal, and current display number does not have a decimal
                        // already, append decimal to current display number
                        resultText.Text = displayResult(resultText.Text + buttonPeriod.Text);
                }
            }
                
        }

        private void operationButton (string opButtonValue)
        {
            if(!lastPressOperation | operation == "=")
                // If the previous button pushed was a mathematical operation button, treat this as a
                // misclick. So, only perform this behavior if the previous button was a number button,
                // or if the previous button was an equals. For example, if the user pushes "1 + 2 = -"
                // then this should be understood as, after pressing =, the result shows 3, so pressing -
                // should begin a "3 - ?" operation.
            {
                if (operation != "=" | opButtonValue == "=")
                    // If the previous operation button pressed is an arithmetic operation, or if the
                    // current operation button pressed is the equal sign, do it. For the first case,
                    // when someone presses "1 + 2 -", then + is the stored operation value and - is the
                    // input opButtonValue. Then when pressing -, this will perform the "1 + 2" operation
                    // for storage for a "3 - ?" operation. For the second case, "1 + 2 =" again has +
                    // as the stored operation value, so this simply performs the 1 + 2 operation and
                    // makes the result available for a future operation set by the next button press.
                    calculate(operation);
                else
                {
                    // Otherwise, just store the current value for math
                    total = double.Parse(resultText.Text);
                    // Display the next operation at the end of the display string
                    resultText.Text += opButtonValue;
                }

                operation = opButtonValue;
                lastPressOperation = true;
            }
        }

        private void calculate(string operation)
        {
            // Calculates the new total and displayed result based on the last operation button pressed.
            if (operation == "=")
                historyView.Items.Add(resultText.Text + " =");
            else
                historyView.Items.Add(displayResult(total.ToString()) + operation + resultText.Text + " =");
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
            historyView.Items.Add("             " + resultText.Text);
            historyView.Items[historyView.Items.Count - 1].EnsureVisible();
        }

        private string displayResult(string result)
        {
            // Takes a string input which may have negative signs and/or periods, and which may have missing or
            // misplaced commas. Returns a string output with the same negative signs and periods, but appropriately
            // placed commas.

            // Strip those commas
            result = result.Replace(",", "");

            // Strip the leading minus sign if present, and flag that we'll need to return it later
            bool isNegative = result[0].Equals("-");
            if (isNegative)
                result = result.Substring(1);

            // Figure out where the decimal value is. If it's the last value in the string, flag it, since
            // converting to number and back to string will lose it
            int decimalPosition = result.IndexOf(".");
            bool noDecimal = (decimalPosition == -1);

            // If there wasn't a decimal, set its "position" at the end of the number string. Number input
            // currently prevents overlong integers from being entered, but the computer's built in scientific
            // notation is currently allowed in output to manage overlong results
            if (noDecimal)
                decimalPosition = result.Length;

            // Copy pre-decimal characters to a new string, adding commas every 3 characters before the decimal
            string newResult = "";
            int commaPosition = decimalPosition % 3;

            for (int i = 0; i < decimalPosition; i++)
            {
                newResult += result[i];

                if (decimalPosition - i > 3 & commaPosition == 1)
                    newResult += ",";

                // Counts down 2, 1, 0, 2, 1, 0 ... to place commas at every 1.
                commaPosition = (commaPosition + 2) % 3;
            }

            if (!noDecimal)
                newResult += result.Substring(decimalPosition);

            if (isNegative)
                newResult = "-" + newResult;

            return newResult;

        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (!showHistory)
            {
                this.Width = 600;
                showHistory = true;
                buttonHistory.Text = "Hide History";
            }
            else
            {
                this.Width = 365;
                showHistory = false;
                buttonHistory.Text = "Show History";
            }
        }

        private void historyView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
