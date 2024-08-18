namespace ConsoleApp1
{
    public class School
    {
        public Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>> Schedule { get; set; }
    private List<Subject> Subjects { get; set; }
    private List<Day> Days { get; set; }
    private List<Teacher> Teachers { get; set; }
    private List<Class> Classes { get; set; }

    public School(List<Subject> subjects, List<Day> days, List<Teacher> teachers, List<Class> classes)
    {
        Subjects = subjects;
        Days = days;
        Teachers = teachers;
        Classes = classes;
        Schedule = InitializeSchedule();
    }

    private Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>> InitializeSchedule()
    {
        var schedule = new Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>>();
        foreach (var clas in Classes)
        {
            schedule[clas.Number] = new Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>();
            foreach (var day in Days)
            {
                schedule[clas.Number][day.Name] = new Dictionary<(int, int), (Subject, Teacher)>();
                foreach (var hour in day.Hours)
                {
                    schedule[clas.Number][day.Name][hour] = (null, null);
                }
            }
        }
        return schedule;
    }

    public bool AssignClass(Day day, (int, int) hour, Class classObj, Subject subjectObj, Teacher teacherObj)
    {
        if (Schedule[classObj.Number][day.Name][hour].Item1 == null 
            && CheckSameTeacherForASubjectInOneClass(classObj, teacherObj, subjectObj))
        {
            Schedule[classObj.Number][day.Name][hour] = (subjectObj, teacherObj);
            return true;
        }
        return false;
    }

    private bool CheckSameTeacherForASubjectInOneClass(Class classObj, Teacher teacherObj, Subject subjectObj)
    {
        // Basic implementation, you can customize it according to your requirements
        return true;
    }

    public void DisplaySchedule()
    {
        foreach (var classNum in Schedule.Keys)
        {
            Console.WriteLine($"Class {classNum}:");
            foreach (var day in Schedule[classNum].Keys)
            {
                Console.WriteLine($"\t{day}:");
                foreach (var hour in Schedule[classNum][day].Keys)
                {
                    var assignment = Schedule[classNum][day][hour];
                    Console.WriteLine($"\t\t{hour.Item1}-{hour.Item2} - {assignment.Item1?.Name ?? "Free"} - {assignment.Item2?.Name ?? "Any"}");
                }
            }
        }
    }

    public List<int> GetClassNumbers()
    {
        return Classes.Select(clas => clas.Number).ToList();
    }
    }
}

