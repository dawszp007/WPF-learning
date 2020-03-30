using System;
using System.Collections.Generic;
using System.Data;
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


namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DigitBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (isNumber(ResultBox.Text))
            {
                if (ResultBox.Text == "0")
                {
                    ResultBox.Text = button.Content.ToString();
                }
                else
                {
                    ResultBox.Text += button.Content.ToString();
                }
            }
        }
        private void SignBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (isNumber(ResultBox.Text) && !ResultBox.Text.EndsWith("."))
            {


                if (ResultBox.Text != "0" && CountBox.Text == "0")
                {
                    CountBox.Text = ResultBox.Text + " " + button.Content.ToString();
                    ResultBox.Text = 0.ToString();
                }
                else if (ResultBox.Text != "0" && CountBox.Text != "0")
                {
                    CountBox.Text += " " + ResultBox.Text + " " + button.Content.ToString();
                    ResultBox.Text = 0.ToString();
                }
            }
        }
        private void ChangeSignBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ResultBox.Text != "0" && !ResultBox.Text.Contains("-"))
            {
                ResultBox.Text = ResultBox.Text.Insert(0, "-");
            }
            else if (ResultBox.Text.Contains("-"))
            {
                ResultBox.Text = ResultBox.Text.Remove(0, 1);
            }
        }
        private void DotBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ResultBox.Text.Contains(".")) 
            {
                ResultBox.Text += ".".ToString();
            }
        }
        private void EqualBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isNumber(ResultBox.Text) && !ResultBox.Text.EndsWith("."))
            {
                string math = (CountBox.Text += ResultBox.Text);
                CountBox.Text = "0";
                ResultBox.Text = calculate(math);
            }

        }
        private void BackspaceBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button.Name == "BackspaceButton")
            {
                if (ResultBox.Text != "0" && isNumber(ResultBox.Text))
                {
                    if (ResultBox.Text.Length == 1)
                    {
                        ResultBox.Text = "0";
                    }
                    else
                    {
                        ResultBox.Text = ResultBox.Text.Remove(ResultBox.Text.Length - 1, 1);
                    }

                }
            }
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button.Name == "ClearButton")
            {
                CountBox.Text = "0";
                ResultBox.Text = "0";
            }
            else if (button.Name == "AcButton")
            {
                ResultBox.Text = "0";
            }
        }
        private bool isNumber(string text)
        {
            double temp;
            if (double.TryParse(text, out temp))
            {
                return true;
            }
            return false;
        }
        private string calculate(string math)
        {
            math = Regex.Replace
                (
                    math, @"\d+(\.\d+)?", m =>
                    {
                        var x = m.ToString();
                        return x.Contains(".") ? x : string.Format("{0}.0", x);
                    }
                );
            try
            {
                double value = Math.Round(Convert.ToDouble(new DataTable().Compute(math, string.Empty)), 8);
                return (value < -9999999999 || value > 9999999999) ? "out of range" : value.ToString();
            }
            catch (DivideByZeroException)
            {
                return "do not divide by zero";
            }

        }
        
    }
}