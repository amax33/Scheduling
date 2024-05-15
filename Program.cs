class Subject{
    public string Name { get; set; }
    public bool Generality { get; set; }
    public (int, int) Time { get; set; }

    public Subject(string name, bool general, (int, int) time){
        Name = name;
        Generality = general;
        Time = time;
    }
}

class Day{
    public string Name { get; set; }
    public bool Certainty { get; set; }
    public List<(int, int)> Hours { get; set; }

    public Day(string name, bool certain, List<(int, int)> hours){
        Name = name;
        Certainty = certain;
        Hours = hours;
    }
}

class Teacher{
    public string Name { get; set; }
    public string Degree { get; set; }
    public int Experience { get; set; }
    public List<Day> Days { get; set; }

    public Teacher(string name, string degree, int expr, List<Day> days){
        Name = name;
        Degree = degree;
        Experience = expr;
        Days = days;
    }
}

class Class{
    public int Grade { get; set; }
    public int Number { get; set; }
    public List<Day> Days { get; set; }

    public Class(int grade, List<Day> days, int number){
        Grade = grade;
        Number = number;
        Days = days;
    }

    public List<string> GetClassDaysName(){
        return Days.Select(day => day.Name).ToList();
    }
}

class School{
    public Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>> Schedule { get; set; }
    private List<Subject> Subjects { get; set; }
    private List<Day> Days { get; set; }
    private List<Teacher> Teachers { get; set; }
    private List<Class> Classes { get; set; }

    public School(List<Subject> subjects, List<Day> days, List<Teacher> teachers, List<Class> classes){
        Subjects = subjects;
        Days = days;
        Teachers = teachers;
        Classes = classes;
        Schedule = InitializeSchedule();
    }

    private Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>> InitializeSchedule(){
        var schedule = new Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>>();
        foreach (var clas in Classes){
            schedule[clas.Number] = new Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>();
            foreach (var day in Days){
                schedule[clas.Number][day.Name] = new Dictionary<(int, int), (Subject, Teacher)>();
                foreach (var hour in day.Hours){
                    schedule[clas.Number][day.Name][hour] = (null, null);
                }
            }
        }
        return schedule;
    }

    public bool AssignClass(Day day, (int, int) hour, Class classObj, Subject subjectObj, Teacher teacherObj){
        if (Schedule[classObj.Number][day.Name][hour].Item1 == null
            && CheckSameTeacherForASubjectInOneClass(classObj, teacherObj, subjectObj)){
            Schedule[classObj.Number][day.Name][hour] = (subjectObj, teacherObj);
            return true;
        }
        return false;
    }

    private bool CheckSameTeacherForASubjectInOneClass(Class classObj, Teacher teacherObj, Subject subjectObj){
        // Basic implementation, you can customize it according to your requirements
        return true;
    }

    public void DisplaySchedule(){
        foreach (var classNum in Schedule.Keys){
            Console.WriteLine($"Class {classNum}:");
            foreach (var day in Schedule[classNum].Keys){
                Console.WriteLine($"\t{day}:");
                foreach (var hour in Schedule[classNum][day].Keys){
                    var assignment = Schedule[classNum][day][hour];
                    Console.WriteLine($"\t\t{hour.Item1}-{hour.Item2} - {assignment.Item1?.Name ?? "Free"} - {assignment.Item2?.Name ?? "Any"}");
                }
            }
        }
    }

    public List<int> GetClassNumbers(){
        return Classes.Select(clas => clas.Number).ToList();
    }
}

static class Program{
    static void Main(string[] args){
        // Sample data
        var math = new Subject("Math", false, (8, 9));
        var physics = new Subject("Physics", false, (9, 10));
        var chemistry = new Subject("Chemistry", false, (10, 11));

        var monday = new Day("Monday", true, new List<(int, int)> { (8, 9), (9, 10), (10, 11) });
        var tuesday = new Day("Tuesday", true, new List<(int, int)> { (8, 9), (9, 10), (10, 11) });
        var wednesday = new Day("Wednesday", true, new List<(int, int)> { (8, 9), (9, 10), (10, 11) });

        var teacher1 = new Teacher("teach1", "B", 10, new List<Day> { monday, tuesday });
        var teacher2 = new Teacher("teach2", "A", 5, new List<Day> { tuesday, wednesday });

        var class1 = new Class(10, new List<Day> { monday, wednesday }, 1);
        var class2 = new Class(11, new List<Day> { tuesday, wednesday }, 2);

        var subjects = new List<Subject> { math, physics, chemistry };
        var days = new List<Day> { monday, tuesday, wednesday };
        var teachers = new List<Teacher> { teacher1, teacher2 };
        var classes = new List<Class> { class1, class2 };

        var schoolSchedule = new School(subjects, days, teachers, classes);

        // Assign some classes to the schedule
        schoolSchedule.AssignClass(monday, (8, 9), class1, math, teacher1);
        schoolSchedule.AssignClass(tuesday, (10, 11), class2, physics, teacher2);

        // Display the schedule
        schoolSchedule.DisplaySchedule();

        // Validate the solution
        Validator(schoolSchedule.Schedule);
    }

    static void Validator(Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>> solution)
    {
        // Implement your validation logic here
    }
}
