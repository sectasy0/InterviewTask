using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace InterviewTask {
    class Program {
        static void Main(string[] args) {
            if (args.Length > 0 && args.Length < 2) {
                try {
                    List<EmployeesDay> values = File.ReadAllLines(Convert.ToString(args[0]))
                                           .Select(v => EmployeesDay.FromCSV(v))
                                           .ToList();
                } catch (FileNotFoundException ex) {
                    Console.WriteLine(ex);
                }

            } else {
                Console.WriteLine("Incorrect number of arguments, please enter only one argument");
                return;
            }

            foreach (EmployeesDay empDay in EmployeesDay.EmployessDayList) {
                Console.WriteLine("{0}, {1}, {2}, {3}", empDay.EmployeeCode, empDay.Date, empDay.TimeOfEntry, empDay.TimeOfExit);
            }
        }
    }

    class EmployeesDay {
        public static List<EmployeesDay> EmployessDayList = new List<EmployeesDay>();

        public String EmployeeCode { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan TimeOfEntry { get; private set; }
        public TimeSpan TimeOfExit { get; private set; }

        public static EmployeesDay FromCSV(string csvLine) {
            string[] values = csvLine.Split(';');

            EmployeesDay employeesday = new EmployeesDay();
            employeesday.EmployeeCode = Convert.ToString(values[0]);
            employeesday.Date = Convert.ToDateTime(values[1]);

            if (values.Length > 5 && values.Length < 4) {
                Console.WriteLine("Incorrect .csv format, the file must have a specific format for the program to work properly");
                return null;
            }

            if (values.Length == 5) {
                employeesday.TimeOfEntry = TimeSpan.Parse(values[2]);
                employeesday.TimeOfExit = TimeSpan.Parse(values[3]);

                EmployessDayList.Add(employeesday);
                return employeesday;

            }

            if (!EmployessDayList.Any(n => n.EmployeeCode == employeesday.EmployeeCode && n.Date == employeesday.Date)) {
                EmployessDayList.Add(employeesday);
            }

            foreach (EmployeesDay empDay in EmployessDayList) {
                if (empDay.EmployeeCode == employeesday.EmployeeCode && empDay.Date == employeesday.Date) {
                    try {
                        if (values[3] == "WE") {
                            empDay.TimeOfEntry = TimeSpan.Parse(values[2]);
                        } else if (values[3] == "WY") {
                            empDay.TimeOfExit = TimeSpan.Parse(values[2]);
                        }
                    } catch (FormatException) {
                        continue;
                    }
                }
            }
            return employeesday;
        }
    }
}
