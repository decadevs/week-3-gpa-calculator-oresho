using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GpaCalculator
{
    public class CalculatorService
    {
        public void Start() 
        {
            Console.WriteLine("Welcome to Ore's Console Gpa calculator.");
            Console.WriteLine("Please enter the courses you would like to calculate your Gpa for: ");
            bool isDone = false;
            // Create a list to store the courses
            List<CourseInfo> courses = new List<CourseInfo>();

            // Add CourseInfo of each course to the list
            while(!isDone)
            {
                courseName:
                Console.Write("Enter the course name: ");
                string courseName = Console.ReadLine().ToUpper().Trim();
                string pattern = @"^[a-zA-Z]+$"; // Regex to ensure only alphabet characters
                if (String.IsNullOrEmpty(courseName) || String.IsNullOrWhiteSpace(courseName)
                    || courseName.Length != 3 || !Regex.IsMatch(courseName, pattern))
                {
                    Console.WriteLine("Invalid input, course name is a 3 letter abbreviation. " +
                        "Special characters, numbers and whitespaces are not accepted in your course name. Please enter a valid course name.");
                    goto courseName;
                }

                courseCode:
                Console.Write("Enter the course code: "); 
                string courseCodeInput = Console.ReadLine().Trim();
                if(courseCodeInput.Length != 3 || !int.TryParse(courseCodeInput, out int courseCode))
                {
                    Console.WriteLine("Invalid input, course code is a 3 digit number. Please enter a valid course code.");
                    goto courseCode;
                }

                // Prevent the addition of duplicate courses
                if (CourseExists(courseName, courseCode, courses)) 
                {
                    Console.WriteLine("Duplicate Course detected, please add new course.");
                    continue; 
                }

                courseUnit:
                Console.Write("Enter the course unit: ");
                string courseUnitInput = Console.ReadLine().Trim();
                if(!int.TryParse(courseUnitInput, out int courseUnit))
                { 
                    Console.WriteLine("Invalid input. Please enter a valid course unit.");
                    goto courseUnit;
                }

                courseScore:
                Console.Write("Enter the course score: ");
                string courseScoreInput = Console.ReadLine().ToUpper().Trim();
                if(!double.TryParse(courseScoreInput, out double courseScore) || courseScore < 0 || courseScore > 100)
                {
                    Console.WriteLine("Invalid input. Please enter a valid course score between 0 - 100.");
                    goto courseScore;
                }

                courses.Add(new CourseInfo
                {
                    CourseName = courseName,
                    CourseCode = courseCode,
                    CourseUnit = courseUnit,
                    CourseScore = courseScore
                });

                // Check if the user wants to add another course
                continuecheck:
                Console.Write("Do you want to add another course? (yes/no): ");
                string response = Console.ReadLine().Trim();
                if(String.IsNullOrEmpty(response) || String.IsNullOrWhiteSpace(response)
                    || (!response.ToLower().Equals("yes") && !response.ToLower().Equals("no")))
                {
                    Console.WriteLine("Please input yes or no");
                    // Ask again if the user's response is not yes or no
                    goto continuecheck;
                }
                if(response.ToLower() == "no")
                {
                    isDone = true;
                }
            }
            DisplayCourses(courses);
            CalculateGpa(courses);
        }

        private bool CourseExists(string courseName, int courseCode, List<CourseInfo> courses)
        {
            return courses.Exists(x => x.CourseName == courseName && x.CourseCode == courseCode);
        }

        public void CalculateGpa(List<CourseInfo> courses)
        {
            // Initialize variables for calculating gpa
            double totalQualityPoints = 0;
            double totalCourseUnit = 0;
           
            foreach(CourseInfo course in courses)
            {
                totalQualityPoints +=  (GetGradeUnit(course.CourseScore) * course.CourseUnit);
                totalCourseUnit += course.CourseUnit;
            }

            // Calculate the Gpa
            double gpa = totalQualityPoints / totalCourseUnit;
            string gpaString = gpa.ToString($"F2");
            Console.WriteLine($"Your Gpa is = {gpaString}, to 2 decimal places");
        }

        public int GetGradeUnit(double courseScore)
        {
            Dictionary<char, int> gradeUnits = new Dictionary<char, int>
            {
                {'A', 5},
                {'B', 4},
                {'C', 3},
                {'D', 2},
                {'E', 1},
                {'F', 0}
            };

            int gradeUnit = 0;
            if (IsInRange(courseScore, 70, 100))
            {
                gradeUnit = gradeUnits['A'];
            }
            else if(IsInRange(courseScore, 60, 69))
            {
                gradeUnit = gradeUnits['B'];
            }
            else if(IsInRange(courseScore, 50, 59))
            {
                gradeUnit = gradeUnits['C'];
            }
            else if(IsInRange(courseScore, 45, 49))
            {
                gradeUnit = gradeUnits['D'];
            }
            else if(IsInRange(courseScore, 40, 44))
            {
                gradeUnit = gradeUnits['E'];
            }
            else
            {
                gradeUnit = gradeUnits['F'];
            }
            return gradeUnit;
        }

        public bool IsInRange(double score, int lowerBound, int upperBound)
        {
            return score >= lowerBound && score <= upperBound;
        }

        private void DisplayCourses(List<CourseInfo> courses)
        {
            Dictionary<int, char> unitGrades = new Dictionary<int, char>
            {
                {5, 'A'},
                {4, 'B'},
                {3, 'C'},
                {2, 'D'},
                {1, 'E'},
                {0, 'F'}
            };

            // Set column widths
            int courseWidth = 10;
            int unitWidth = 10;
            int gradeWidth = 10;
            int gradeUnitWidth = 10;


            // Draw the table separator
            Console.WriteLine($"|{new string('-', 15)}|{new string('-', 13)}|{new string('-', 14)}|{new string('-', 12)}|");

            string h1 = "COURSE & CODE".PadRight(courseWidth); // Column 1 Name
            string h2 = "COURSE UNIT".PadRight(unitWidth); // Column 2 Name
            string h3 = "COURSE GRADE".PadRight(gradeWidth); // Column 3 Name
            string h4 = "GRADE-UNIT".PadRight(gradeUnitWidth); // Column 4 Name

            // Draw the table header
            Console.WriteLine($"| {h1} | {h2} | {h3} | {h4} |");

            // Draw the table separator
            Console.WriteLine($"|{new string('-', 15)}|{new string('-', 13)}|{new string('-', 14)}|{new string('-', 12)}|");

            // Draw the table data
            foreach (var course in courses)
            {
                string courseNameCode = course.CourseName + " " + course.CourseCode;
                string courseUnit = course.CourseUnit.ToString();
                string courseGrade = unitGrades[GetGradeUnit(course.CourseScore)].ToString();
                string gradeUnit = GetGradeUnit(course.CourseScore).ToString();
                Console.WriteLine($"| {courseNameCode.PadRight(h1.Length)} | {courseUnit.PadRight(h2.Length)} | {courseGrade.PadRight(h3.Length)} | {gradeUnit.PadRight(h4.Length)} |");
            }

            // Draw the table separator
            Console.WriteLine($"|{new string('-', 15)}|{new string('-', 13)}|{new string('-', 14)}|{new string('-', 12)}|");

        }
    }
}
