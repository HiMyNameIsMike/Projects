using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(getDate());
            var mth = DateTime.Now.Month;
            var yr = DateTime.Now.Year;
            Console.WriteLine(getMonth(yr,mth));
            var chosenMonth = createCalendar(yr, mth) ;
            foreach (string day in chosenMonth){
                Console.WriteLine(day);
            }
            menu();
            Console.ReadKey();

        }

        public static string getDate()
        {
            var currentDate = DateTime.Now;
            return currentDate.ToString();
        }

        public static int getMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public static List<string> createCalendar(int year, int month)
        {
            var daysInMonth = getMonth(year, month);
            List<string> calendar= new List<string>();
            for (int i=1; i <=daysInMonth; i++)
            {
                calendar.Add(DateTime.Parse(year + "-" + month + "-" + i).ToString());
            }
            return calendar;
        }

        public static void menu()
        {
            string choice = "";
            Console.WriteLine("Please select option");
            choice = Console.ReadLine();
        }
    }   

}
