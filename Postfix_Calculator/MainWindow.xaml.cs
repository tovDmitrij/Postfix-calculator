using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace Postfix_Calculator
{
    public partial class MainWindow : Window
    {
        private Dictionary<char, bool> values = new Dictionary<char, bool>();

        public MainWindow()
        {
            InitializeComponent();
            char[] tmp = new char[] { 'A', 'B', 'C', 'D' };
            for (int i = 0; i < 4; i++)
            {
                values.Add(tmp[i], false);
            }
        }

        /// <summary>
        /// Нажатие кнопок на панели
        /// </summary>
        private void ButtonPress(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content)
            {
                case "Очистить":
                    textBoxInfix.Text = "";
                    return;
                case "BackSpace":
                    try
                    {
                        textBoxInfix.Text = textBoxInfix.Text.Remove(textBoxInfix.Text.Length - 1);
                        return;
                    }
                    catch
                    {
                        return;
                    }
                default:
                    textBoxInfix.Text += ((Button)sender).Content;
                    return;
            }
        }

        /// <summary>
        /// Подтверждение
        /// </summary>
        private void Accept(object sender, RoutedEventArgs e)
        {
            Result r = new Result(textBoxInfix.Text, values);
            textBoxPrefix.Text = r.ToPostfix(textBoxInfix.Text);
            resultFrame.Navigate(r);
        }

        /// <summary>
        /// Смена значения переменной на противоположное
        /// </summary>
        private void ChangeChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            values[Convert.ToChar(checkBox.Uid)] = (bool)checkBox.IsChecked;
        }
    }
}