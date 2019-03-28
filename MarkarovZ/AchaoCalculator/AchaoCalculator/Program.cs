using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchaoCalculator
{
    public class Program
    {
        List<string> formulas = new List<string>();
        Random random = new Random();
        Stack<string> stack_operator = new Stack<string>();//运算符栈
        Stack<string> stack_number = new Stack<string>();//数字栈
        public static List<double> answers = new List<double>();//结果集合
        string str = "";
        string substr = "";
        bool flag = true;

        static void Main(string[] args)
        {
            Program program = new Program();
            int n = 0;
            n = Convert.ToInt32(Console.ReadLine());
            program.FormulaGeneration(n);
            foreach(var str in program.formulas)
            {
                Console.WriteLine(str);
            }
            program.Savefile();
        }
        //四则运算栈
        #region
        private bool judgenumber(string text)//判断是否为数字
        {
            try
            {
                int var1 = Convert.ToInt32(text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool judgeoperator(string text)//判断是否为运算符
        {
            if (text == "(" || text == ")" || text == "+" || text == "-" || text == "*" || text == "/")
            {
                return true;
            }
            else
                return false;
        }
        public object addition(object a, object b)//加法
        {
            Decimal d1 = Decimal.Parse(a.ToString());
            Decimal d2 = Decimal.Parse(b.ToString());
            return d2 + d1;
        }
        public object subduction(object a, object b)//减法
        {
            Decimal d1 = Decimal.Parse(a.ToString());
            Decimal d2 = Decimal.Parse(b.ToString());
            return d2 - d1;
        }
        public object multiplication(object a, object b)//乘法
        {
            Decimal d1 = Decimal.Parse(a.ToString());
            Decimal d2 = Decimal.Parse(b.ToString());
            return d2 * d1;
        }
        public object division(object a, object b)//除法
        {
            Decimal d1 = Decimal.Parse(a.ToString());
            Decimal d2 = Decimal.Parse(b.ToString());
            if(d2%d1!=0||d2<d1)
            {
                flag = false;
                return -10000;
            }
            else
            {
                flag = true;
                return d2 / d1;
            }

        }
        int judgelevel(string text)//判断优先级
        {
            if (text.Equals("("))
            {
                return 1;
            }
            else if (text.Equals(")") || text.Equals("（") || text.Equals("）"))
            {
                return 1;
            }
            else if (text.Equals("+") || text.Equals("-"))
            {
                return 2;
            }
            else if (text.Equals("*") || text.Equals("/"))
            {
                return 3;
            }
            else
                return 10;


        }
        int operator_dected(string types, string a, string b)//根据运算符的类型返回对应的值
        {
            if (types == "+")
            {
                return Convert.ToInt32(addition(a, b));
            }
            else if (types == "-")
            {
                return Convert.ToInt32(subduction(a, b));
            }
            else if (types == "*")
            {
                return Convert.ToInt32(multiplication(a, b));
            }
            else if (types == "/")
            {
                return Convert.ToInt32(division(a, b));
            }
            else
                return 999;
        }
        double operate(string Str)
        {
            stack_number.Clear();//清空栈
            stack_operator.Clear();
            str = Str + "!";//！为结束运算符
            int temp_count = 0;
            try
            {
                for (int i = 0; i < str.Length; i++)
                {
                    substr = str.Substring(i, 1);
                    if (judgenumber(substr))//如果是数字
                    {
                        if (temp_count == 0)
                        {
                            stack_number.Push(substr);
                        }
                        else
                            temp_count--;
                        if (judgenumber(str.Substring(i + 1, 1)))
                        {
                            string link1 = stack_number.Pop();
                            link1 += str.Substring(i + 1, 1);
                            stack_number.Push(link1);
                            temp_count++;
                        }
                    }
                    else if (judgeoperator(substr))
                    {
                        if (stack_operator.Count >= 1)
                        {
                            int new1 = judgelevel(substr);
                            int old1 = judgelevel(stack_operator.Peek());
                            if (old1 < new1 || substr == "(")//判断优先级
                            {
                                stack_operator.Push(substr); //将运算符插入栈中
                            }
                            else
                            {
                                if (substr == ")")
                                {
                                    for (; stack_operator.Count > 0; stack_operator.Pop())
                                    {
                                        if (stack_operator.Contains("(") && stack_operator.Peek() == "(")
                                        {
                                            stack_operator.Pop();
                                            break;
                                        }
                                        else
                                        {
                                            int temp1 = Convert.ToInt32(stack_number.Peek()); stack_number.Pop();
                                            int temp2 = Convert.ToInt32(stack_number.Peek()); stack_number.Pop();
                                            stack_number.Push(operator_dected(stack_operator.Peek(), temp1.ToString(), temp2.ToString()).ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    for (; stack_operator.Count > 0 && stack_number.Count >= 2 && stack_operator.Peek() != "("; stack_operator.Pop())
                                    {
                                        string temp_a = substr;
                                        int new2 = judgelevel(temp_a);
                                        int old2 = judgelevel(stack_operator.Peek());
                                        if (old2 < new2 || substr == "(")//判断优先级
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            int temp3 = Convert.ToInt32(stack_number.Peek()); stack_number.Pop();
                                            int temp4 = Convert.ToInt32(stack_number.Peek()); stack_number.Pop();
                                            stack_number.Push(operator_dected(stack_operator.Peek(), temp3.ToString(), temp4.ToString()).ToString());
                                        }
                                    }
                                    stack_operator.Push(substr);
                                }
                            }
                        }
                        else
                        {
                            stack_operator.Push(substr);


                        }
                    }
                    else if (substr == "!")
                    {
                        for (; stack_operator.Count > 0 && stack_number.Count >= 2 && stack_operator.Peek() != "("; stack_operator.Pop())
                        {
                            int temp3 = Convert.ToInt32(stack_number.Peek()); stack_number.Pop();
                            int temp4 = Convert.ToInt32(stack_number.Peek()); stack_number.Pop();
                            stack_number.Push(operator_dected(stack_operator.Peek(), temp3.ToString(), temp4.ToString()).ToString());
                        }
                        return Convert.ToDouble(stack_number.Peek());
                    }
                    else
                    {
                        
                        break;
                    }
                }
            }
            catch
            {


            }
            return 0;
        }
        #endregion
        /// <summary>
        /// 生成算式
        /// </summary>
        /// <param name="num">算式数量</param>
        public void FormulaGeneration(int num)
        {
            string formula = "";
            int n = 0;
            string str = "";
            double ans = 0;
            for (int i = 0; i < num; i++)
            {
                n = random.Next(2, 4);
                do
                {
                    str = FormulaCreation(n);
                    formula = str + "!";
                    ans = operate(formula);
                    if (ans % 1 == 0 && ans > 0 && flag == true)
                    {
                        answers.Add(ans);
                        break;
                    }
                } while (true);
                formula = str + "=" + operate(formula); ;
                formulas.Add(formula);
                
            }
        }
        /// <summary>
        /// 生成算式字符串
        /// </summary>
        /// <param name="n">运算符数量</param>
        /// <returns>算式字符串</returns>
        string FormulaCreation(int n)
        {
            int a, b, c, d, e;
            string x, y, z;
            switch (n)
            {
                case 2:
                    a = random.Next(0, 101);
                    b = random.Next(0, 101);
                    c = random.Next(0, 101);
                    d = random.Next(0, 4);
                    x = Formula(d);
                    d = random.Next(0, 4);
                    y = Formula(d);
                    return a + "" + x + "" + b + "" + y + "" + c;
                case 3:
                    a = random.Next(0, 101);
                    b = random.Next(0, 101);
                    c = random.Next(0, 101);
                    d = random.Next(0, 101);
                    e = random.Next(0, 4);
                    x = Formula(e);
                    e = random.Next(0, 4);
                    y = Formula(e);
                    e = random.Next(0, 4);
                    z = Formula(e);
                    return a + "" + x + "" + b + "" + y + "" + c + "" + z + "" + d;
            }
            return "";
        }
        /// <summary>
        /// 返回运算符
        /// </summary>
        /// <param name="n">运算符类型</param>
        /// <returns>运算符</returns>
        string Formula(int n)
        {
            switch(n)
            {
                case 0:
                    return "+";
                case 1:
                    return "-";
                case 2:
                    return "*";
                case 3:
                    return "/";
            }
            return "";
        }

        void Savefile()
        {
            string s = "E:/开发内容/1.txt";
            System.IO.File.WriteAllLines(s, formulas);
        }
    }
}
