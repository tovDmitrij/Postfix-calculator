using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace Postfix_Calculator
{
    /// <summary>
    /// Вычисление пропозициональной формулы, находящейся в постфиксной записи
    /// </summary>
    public partial class Result : Page
    {
        private string infixNotation;

        /// <summary>
        /// Вычисление пропозициональной формулы, находящейся в постфиксной записи
        /// </summary>
        /// <param name="infixNotation">Инфиксная запись</param>
        public Result(string infixNotation, Dictionary<char, bool> values)
        {
            InitializeComponent();
            switch (CheckInfix(infixNotation))
            {
                case true:
                    break;
                case false:
                    MessageBox.Show("Инфиксная запись введена неправильно!");
                    return;
            }
            this.infixNotation = ToPostfix(infixNotation);
            textResult.Text = FunctionResult(values) ? "1" : "0";
        }

        /// <summary>
        /// Результат функции при входных параметрах
        /// </summary>
        /// <param name="row">Входные параметры</param>
        private bool FunctionResult(Dictionary<char, bool> row)
        {
            Stack<bool> stack = new Stack<bool>();
            foreach (var symbol in infixNotation)
            {
                switch (char.IsLetter(symbol))
                {
                    case true:
                        stack.Push(row[symbol]);
                        break;
                    case false:
                        switch (symbol)
                        {
                            case '¬':
                                stack.Push(!stack.Pop());
                                break;
                            default:
                                bool operand2 = stack.Pop();
                                bool operand1 = stack.Pop();
                                switch (symbol)
                                {
                                    case '→':
                                        stack.Push(!operand1 || operand2);
                                        break;
                                    case '⋀':
                                        stack.Push(operand1 & operand2);
                                        break;
                                    case '⋁':
                                        stack.Push(operand1 || operand2);
                                        break;
                                    case '↔':
                                        stack.Push(operand1 == operand2);
                                        break;
                                    case '⊕':
                                        stack.Push((operand1 || operand2) && (!operand1 || !operand2));
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            return stack.Peek();
        }

        /// <summary>
        /// Преобразование инфиксной записи в постфиксную
        /// </summary>
        public string ToPostfix(string infixNotation)
        {
            switch (CheckInfix(infixNotation))
            {
                case true:
                    break;
                case false:
                    MessageBox.Show("Инфиксная запись введена неправильно!");
                    return "";
            }
            string postfix = "";
            Stack<char> stack = new Stack<char>();
            foreach (var symbol in infixNotation)
            {
                switch (char.IsLetter(symbol))
                {
                    case true:
                        postfix += symbol.ToString();
                        if (stack.Count != 0)
                        {
                            while (stack.Peek() == '¬')
                            {
                                postfix += stack.Pop();
                                if (stack.Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    case false:
                        switch (symbol)
                        {
                            case '(':
                                stack.Push(symbol);
                                break;
                            case ')':
                                while (stack.Peek() != '(')
                                {
                                    postfix += stack.Pop();
                                }
                                stack.Pop();
                                if (stack.Count != 0)
                                {
                                    while (stack.Peek() == '¬')
                                    {
                                        postfix += stack.Pop();
                                        if (stack.Count == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                                break;
                            default:
                                if (stack.Count == 0)
                                {
                                    stack.Push(symbol);
                                }
                                else
                                {
                                    if (Priority(stack.Peek()) > Priority(symbol))
                                    {
                                        postfix += stack.Pop();
                                    }
                                    stack.Push(symbol);
                                }
                                break;
                        }
                        break;
                }
            }
            while (stack.Count != 0)
            {
                postfix += stack.Pop();
            }
            return postfix;
        }

        /// <summary>
        /// Приоритет операции
        /// </summary>
        /// <param name="operation">Операция</param>
        private static int Priority(char operation)
        {
            switch (operation)
            {
                case '¬':
                    return 3;
                case '⋀':
                    return 2;
                case '⋁':
                    return 1;
                case '⊕':
                    return 1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Проверка на правильность написания инфиксной нотации
        /// </summary>
        /// <param name="infixNotation">Инфиксная запись</param>
        public static bool CheckInfix(string infixNotation)
        {
            char[] operators = { '(', ')', '⋁', '⋀', '→', '↔', '¬', '⊕' };
            try
            {
                if (operators.Contains(infixNotation[^1]) && (infixNotation[^1] != ')') || operators.Contains(infixNotation[0]) && (infixNotation[0] != '¬') && (infixNotation[0] != '('))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            Stack<char> stack = new Stack<char>();
            foreach (var symbol in infixNotation)
            {
                switch (symbol)
                {
                    case '(':
                        stack.Push('(');
                        break;
                    case ')':
                        if (stack.Count == 0)
                        {
                            return false;
                        }
                        else
                        {
                            stack.Pop();
                        }
                        break;
                    default:
                        break;
                }
            }
            if (stack.Count != 0)
            {
                return false;
            }
            for (int i = 0; i < infixNotation.Length - 1; i++)
            {
                char currentSymbol = infixNotation[i];
                char nextSymbol = infixNotation[i + 1];
                if (char.IsLetter(currentSymbol) && (!(operators.Contains(nextSymbol)) || (nextSymbol == '(') || (nextSymbol == '¬')))
                {
                    return false;
                }
                if (operators.Contains(currentSymbol) && (operators.Contains(nextSymbol)) && (nextSymbol != '(') && (nextSymbol != '¬') && (currentSymbol != ')'))
                {
                    return false;
                }
                if ((currentSymbol == ')') && ((char.IsLetter(nextSymbol)) || (nextSymbol == '¬')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}