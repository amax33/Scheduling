namespace ConsoleApp1
{
    public class Teacher
    {
        public string Name { get; set; }
        public string Degree { get; set; }
        public List<(Subject, int)> Experience { get; set; }
        public List<Day> Days { get; set; }
        public int TotalTimeInSchool { get; set; } // ht_s

        public Teacher(string name, string degree, List<(Subject, int)> expr, List<Day> days, int totalTimeInSchool)
        {
            Name = name;
            Degree = degree;
            Experience = expr;
            Days = days;
            TotalTimeInSchool = totalTimeInSchool;
        }
    }
}

