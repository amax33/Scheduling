namespace ConsoleApp1
{
    public class Class
    {
        public int Grade { get; set; }
        public int Number { get; set; }
        public List<Day> Days { get; set; }
        public List<(Subject, int)> SubjectPeriods { get; set; } // hc_s
        public int TotalPeriods { get; set; } // hc

        public Class(int grade, List<Day> days, int number, List<(Subject, int)> subjectPeriods, int totalPeriods)
        {
            Grade = grade;
            Number = number;
            Days = days;
            SubjectPeriods = subjectPeriods;
            TotalPeriods = totalPeriods;
        }
        public int GetSubjectHours(Subject subject)
        {
            var subjectTuple = SubjectPeriods.FirstOrDefault(s => s.Item1.Equals(subject));
            if (subjectTuple != default)
            {
                return subjectTuple.Item2;
            }
            throw new ArgumentException("Subject not found in the class");
        }
    }
}

