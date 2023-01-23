using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ADOConsoleApp
{
    class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCity { get; set; }
        public int EmpSalary { get; set; }
        public int DeptId { get; set; }
        
    }

    static class DataComponent
    {
        const string fileName = "KarData.csv";
        private static List<Employee> getAll()
        {
            var list = new List<Employee>();
            var lines = File.ReadAllLines(fileName);
            
            foreach (var line in lines)
            {
                
                var words = line.Split(',');
                var newEmp = new Employee
                {
                    EmpId = int.Parse(words[0]),
                    EmpName = words[1].ToString(),
                    EmpCity = words[2].ToString(),
                    EmpSalary = int.Parse(words[3]),
                    DeptId = int.Parse(words[4])

                };
                list.Add(newEmp);
            }
            return list;
        }
        public static List<Employee> AllRecords => getAll();
    }


    class LINQDemo
    {
        static List<Employee> data = DataComponent.AllRecords;
        static void Main()
        {
            //displayAllNames();
            //displayNamesAndAddress();
            //displayNamesFromCity("Southaven");
            //displayNamesWithSalariesGreaterThan(5000);
            //displayCityWithSalariesGreaterThan(5000);
            //displayNamesOrderbyName();
            //displayUniqueCities();
            //displayNamesFromID(30);
            dsiplayNamesByAlphabets();

        }

        private static void dsiplayNamesByAlphabets()
        {
            
        }

        private static void displayNamesFromID(int id)
        {
            var query = from rec in data
                        where rec.EmpId == id
                        select rec.EmpName;
            foreach (var name in query)
                Console.WriteLine(name);
        }

        private static void displayUniqueCities()
        {
            var query = (from rec in data
                         select rec.EmpCity).Distinct();
            foreach (var cityName in query)
                Console.WriteLine(cityName);
        }

        private static void displayNamesOrderbyName()
        {
            var query = from rec in data
                        orderby rec.EmpName ascending
                        select rec.EmpName;
            foreach (var name in query)
                Console.WriteLine(name);
        }

        private static void displayCityWithSalariesGreaterThan(int salary)
        {
            var query = from rec in data
                        where rec.EmpSalary >= salary && rec.EmpCity.StartsWith("R")
                        select new { rec.EmpCity, rec.EmpSalary };
            foreach (var city in query)
                Console.WriteLine(city);
        }

        private static void displayNamesWithSalariesGreaterThan(int salary)
        {
            var query = from rec in data
                        where rec.EmpSalary >= salary && rec.EmpName.StartsWith("V")
                        select new { rec.EmpName, rec.EmpSalary };
            foreach (var name in query)
                Console.WriteLine(name);
        }

        private static void displayNamesFromCity(string city)
        {
            var query = from rec in data
                        where rec.EmpCity == city
                        select rec.EmpName;
            foreach (var name in query)
                Console.WriteLine(name);
        }
        private static void displayNamesAndAddress()
        {
            var query = from rec in data
                        select new { Name = rec.EmpName, Address = rec.EmpCity};
            foreach(var res in query)
                Console.WriteLine($"{res.Name} from {res.Address}");
        }

        private static void displayAllNames()
        {
            var data = DataComponent.AllRecords;
            var query = from emp in data
                        select emp.EmpCity;
            foreach(var name in query)
                Console.WriteLine(name.ToUpper());
        }
    }
}
