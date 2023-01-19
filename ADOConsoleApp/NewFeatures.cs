using System;
using System.Collections.Generic;

namespace ADOConsoleApp
{
    class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress => "Bangalore";

    }

    static class MyExtensions
    {
        public static int GetNoOfWords(this string obj)
        {
            var words = obj.Split(' ');
            return words.Length;
        }
    }

    class NewFeatures
    {
        static void Main(string[] args)
        {
            //varKeyword();
            //anonymousTypes();
            //extensionMethods();
            lambdaExpressions();
            //automaticProperties();
        }

        private static void automaticProperties()
        {
            var obj = new Customer { CustomerId = 123, CustomerName = "Raj" };
            Console.WriteLine(obj.CustomerName);
        }

        private static void lambdaExpressions()
        {
            List<string> data = new List<string>("Khan's first starring role was in Lekh Tandon's television series Dil Dariya, which began shooting in 1988, but production delays led to the Raj Kumar Kapoor directed 1989 series Fauji becoming his television debut instead. In the series, which depicted a realistic look at the training of army cadets, he played the leading role of Abhimanyu Rai. This led to further appearances in Aziz Mirza's television series Circus (1989–90) and Mani Kaul's miniseries Idiot (1992). Khan also played minor parts in the serials Umeed (1989) and Wagle Ki Duniya (1988–90), and in the English-language television film In Which Annie Gives It Those Ones (1989). His appearances in these serials led critics to compare his look and acting style with those of the film actor Dilip Kumar,but Khan was not interested in film acting at the time, thinking that he was not good enough.".Split(' '));

            var founddata = data.FindAll((str) => str == "the");
            foreach (var item in founddata)
            {
                Console.WriteLine(item);
            }
        }

        private static void extensionMethods()
        {
            string content = "Khan's first starring role was in Lekh Tandon's television series Dil Dariya, which began shooting in 1988, but production delays led to the Raj Kumar Kapoor directed 1989 series Fauji becoming his television debut instead. In the series, which depicted a realistic look at the training of army cadets, he played the leading role of Abhimanyu Rai. This led to further appearances in Aziz Mirza's television series Circus (1989–90) and Mani Kaul's miniseries Idiot (1992). Khan also played minor parts in the serials Umeed (1989) and Wagle Ki Duniya (1988–90), and in the English-language television film In Which Annie Gives It Those Ones (1989). His appearances in these serials led critics to compare his look and acting style with those of the film actor Dilip Kumar,but Khan was not interested in film acting at the time, thinking that he was not good enough.";
            Console.WriteLine("The total no of words: " + content.GetNoOfWords());
        }

        private static void varKeyword()
        {
            var apples = 123.45f;
            Console.WriteLine(apples);
        }

        private static void anonymousTypes()
        {
            var data = new
            {
                EmpId = 123,
                EmpName = "Pavan R",
                EmpAddress = "Bangalore"
            };

            var data2 = new
            {
                EmpId = 124,
                EmpName = "Sachin",
                EmpAddress = "Mysore"
            };
            var data3 = data;
            Console.WriteLine(data == data3);
        }
    }

}
