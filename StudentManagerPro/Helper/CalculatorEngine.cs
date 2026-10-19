using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentManagerPro.Helper
{
    class CalculatorEngine
    {
        public string CurrentInput { get; private set; } = "0";
        public string Expression { get; private set; } = "";
        private bool _isRewriting = false;

        public delegate void DataChangedHandler(CalculatorEngine calculatorEngine);
        public event DataChangedHandler DataChanged;
        
        private void NotifyDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this);
            }
        }

        public void PressButton(string key)
        {
            if (char.IsDigit(key[0]) || key == ".")
            {
                HandleDigit(key);
            }
            else if (key == "+" || key == "-" || key == "*" || key == "/")
            {
                HandleOperator(key);
            }
            else if (key == "=")
            {
                HandleEquals();
            }

            else if (key == "X" || key.ToLower() == "del") 
            {
                HandleBackspace();
            }

            else if (key == "+/-") 
            {
                HandleNegate();
            }
            else if (key.ToUpper() == "C")
            {
                Reset();
            }

            NotifyDataChanged();
        }

        private void HandleDigit(string key) {
            if (_isRewriting)
                {
                    CurrentInput = key;
                    _isRewriting = false;

                    if (Expression.EndsWith("=") || string.IsNullOrEmpty(Expression))
                    {
                        Expression = "";
                    }
                }
                else
                {
                    if (key == "." && CurrentInput.Contains(".")) return;

                    if (CurrentInput == "0" && key != ".") CurrentInput = key;
                    else CurrentInput += key;
                }
        }

        private void HandleOperator(string key) {
            if (Expression.EndsWith("="))
                {
                    Expression = CurrentInput + key;
                    _isRewriting = true;
                }
                else if (_isRewriting)
                {
                    if (Expression.Length > 0)
                    {
                        Expression = Expression.Substring(0, Expression.Length - 1) + key;
                    }
                }
                else
                {
                    Expression += CurrentInput + key;
                    _isRewriting = true;
                }
        }

        private void HandleEquals(){
             if (_isRewriting) return;
                if (string.IsNullOrEmpty(Expression) && CurrentInput == "0") return;

                string fullExpression = Expression + CurrentInput;

                try
                {
                    decimal result = MathParser.Calculate(fullExpression);
                    CurrentInput = result.ToString();
                    string displayExpression = fullExpression.Replace("+-", "-").Replace("--", "+");
                    Expression = displayExpression + "=";
                }
                catch
                {
                    CurrentInput = "错误";
                    Expression = "";
                }

                _isRewriting = true;
        }

        private void HandleBackspace(){
            if (_isRewriting) return; 

                if (CurrentInput.Length > 1)
                {
                    CurrentInput = CurrentInput.Substring(0, CurrentInput.Length - 1);
                    
                    if (CurrentInput == "-") CurrentInput = "0";
                }
                else
                {
                    CurrentInput = "0";
                }
        }
        
        private void HandleNegate(){
            if (CurrentInput == "0" || string.IsNullOrEmpty(CurrentInput)) return;

                if (CurrentInput.StartsWith("-"))
                {
                    CurrentInput = CurrentInput.Substring(1);
                }
                else
                {
                    CurrentInput = "-" + CurrentInput;
                }
        }

        private void Reset()
        {
            CurrentInput = "0"; 
            Expression = "";   
            _isRewriting = false; 

            NotifyDataChanged();
        }
    }
}
