using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ILOG.Concert;
using ILOG.CPLEX;

class Subject
{
    public string Name { get; set; }
    public bool Generality { get; set; }

    public Subject(string name, bool general)
    {
        Name = name;
        Generality = general;
    }
}

class Day
{
    public string Name { get; set; }
    public int Certainty { get; set; }
    public List<(int, int)> Hours { get; set; }

    public Day(string name, int certain, List<(int, int)> hours)
    {
        Name = name;
        Certainty = certain; //1 = good, 2 = will, 3 = possible
        Hours = hours;
    }
}

class Teacher
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

class Class
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

class School
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

static class Program
{
    static void Main(string[] args)
    {
        var math = new Subject("Math", false);
        var physics = new Subject("Physics", false);
        var chemistry = new Subject("Chemistry", false);
        var literature = new Subject("Literature", true);

        var monday = new Day("Monday", 1, new List<(int, int)> { (8, 9), (9, 10), (10, 11) });
        var tuesday = new Day("Tuesday", 1, new List<(int, int)> { (8, 9), (9, 10), (10, 11) });
        var wednesday = new Day("Wednesday", 1, new List<(int, int)> { (8, 9), (9, 10), (10, 11) });

        var teacher1 = new Teacher("teacher1", "B", new List<(Subject, int)>{(math, 3), (physics, 4)}, new List<Day>
            {
                new Day("Monday", 1, new List<(int, int)> { (8, 9), (9, 10)}), 
                new Day("Tuesday", 2, new List<(int, int)> { (8, 9), (10, 11)}),
                new Day("Wednesday", 1, new List<(int, int)> { (8, 9), (9, 10)})
            },
            6);
        var teacher2 = new Teacher("teacher2", "A", new List<(Subject, int)>{(chemistry, 3), (literature, 4)}, new List<Day>
            {
                new Day("Monday", 1, new List<(int, int)> { (9, 10), (10, 11)}),
                new Day("Tuesday", 1, new List<(int, int)> { (8, 9), (9, 10), (10, 11)}), 
                new Day("Wednesday", 1, new List<(int, int)> { (8, 9), (9, 10)})
            },
            5);
        var teacher3 = new Teacher("teacher3", "A", new List<(Subject, int)>{(math, 3), (physics, 4), (literature, 2)}, new List<Day>
            {
                new Day("Monday", 1, new List<(int, int)> { (8, 9), (9, 10), (10, 11)}), 
                new Day("Wednesday", 1, new List<(int, int)> { (8, 9), (10, 11)}),
                new Day("Tuesday", 1, new List<(int, int)> { (8, 9), (9, 10), (10, 11)})
            },
            5);
        var teacher4 = new Teacher("teacher4", "B", new List<(Subject, int)>{(math, 3), (chemistry, 4), (literature,2)}, new List<Day>
            { 
                new Day("Tuesday", 1, new List<(int, int)> { (8, 9)}), 
                new Day("Wednesday", 2, new List<(int, int)> { (8, 9), (9, 10), (10, 11)})
            },
            2);

        var class1 = new Class(10, new List<Day> { monday, tuesday, wednesday }, 1,
            new List<(Subject, int)> { (math, 3), (physics, 2), (chemistry, 2), (literature, 2)}, 9);
        var class2 = new Class(10, new List<Day> { monday, tuesday, wednesday }, 2,
            new List<(Subject, int)> { (math, 3), (physics, 2), (chemistry, 2), (literature, 2)}, 9);

        var subjects = new List<Subject> { math, physics, chemistry, literature };
        var days = new List<Day> { monday, tuesday, wednesday };
        var teachers = new List<Teacher> { teacher1, teacher2, teacher3, teacher4 };
        var classes = new List<Class> { class1, class2 };


        
        cplex_solver(subjects, days, teachers, classes);
    }
    static void cplex_solver(List<Subject> subjects, List<Day> days, List<Teacher> teachers, List<Class> classes)
{
    Cplex cplex = new Cplex();
    School school = new School(subjects, days, teachers, classes);

    // Create decision variables
    int classCount = classes.Count;
    int dayCount = days.Count;
    int hourCount = days[0].Hours.Count;

    IIntVar[,,,,] Xc_t_s_p_d = new IIntVar[classCount, teachers.Count, subjects.Count, hourCount, dayCount];
    IIntVar[,,] Yc_t_s = new IIntVar[classCount, teachers.Count, subjects.Count];
    for (int c = 0; c < classCount; c++)
    {
        for (int t = 0; t < teachers.Count; t++)
        {
            for (int s = 0; s < subjects.Count; s++)
            {
                Yc_t_s[c, t, s] = cplex.BoolVar($"Y_{c}_{t}_{s}");
                for (int p = 0; p < hourCount; p++)
                {
                    for (int d = 0; d < dayCount; d++)
                    {
                        Xc_t_s_p_d[c, t, s, p, d] = cplex.BoolVar($"X_{c}_{t}_{s}_{p}_{d}");
                    }
                }
            }
        }
    }
    // List to store constraints for conflict refinement
    List<IConstraint> constraintList = new List<IConstraint>();
    
    
    // Define the weights for the soft constraints
    double[] W = { 1, 1, 1, 1, 1, 1 }; // Adjust weights as necessary

    // Initialize the objective expression
    ILinearNumExpr objectiveExpr = cplex.LinearNumExpr();

    // List to store all penalty variables
    List<IIntVar> penaltyVars = new List<IIntVar>();

    // 1. The unlike time of teacher t
    foreach (var t in teachers)
    {
        foreach (var s in subjects)
        {
            foreach (var d in days)
            {
                foreach (var p in d.Hours)
                {
                    // Check if the teacher is not available at this time with certainty 2
                    if (t.Days.Any(day => day.Name == d.Name && day.Hours.Contains(p) && day.Certainty == 2))
                    {
                        ILinearNumExpr expr = cplex.LinearNumExpr();
                        foreach (var c in classes)
                        {
                            expr.AddTerm(1.0,
                                Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s),
                                    d.Hours.IndexOf(p), days.IndexOf(d)]);
                        }

                        // Define a penalty variable for this constraint
                        IIntVar penaltyVar = cplex.BoolVar();
                        penaltyVars.Add(penaltyVar);

                        // Add the constraint that forces the penalty if the condition is violated
                        cplex.AddLe(expr, penaltyVar);
                    }
                }
            }
        }
    }

    
    // 2. Each subject should be taught once in a day
    foreach (var c in classes)
    {
        foreach (var t in teachers)
        {
            foreach (var s in subjects)
            {
                foreach (var d in days)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    foreach (var p in d.Hours)
                    {
                        expr.AddTerm(1.0,
                            Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), d.Hours.IndexOf(p),
                                days.IndexOf(d)]);
                    }

                    // Define a penalty variable for this constraint
                    IIntVar penaltyVar = cplex.BoolVar();
                    penaltyVars.Add(penaltyVar);

                    // Add the constraint that forces the penalty if the subject is taught more than once in a day
                    cplex.AddLe(expr, 1 + penaltyVar); // If expr is greater than 1, penaltyVar will be 1
                    cplex.AddGe(penaltyVar, expr - 1); // Ensure penaltyVar is 0 if expr is less than or equal to 1
                }
            }
        }
    }
    // Add penalties to the objective function with their weights
    foreach (var penaltyVar in penaltyVars)
    {
        objectiveExpr.AddTerm(W[1], penaltyVar); // Use the appropriate weight for each penalty term
    }
    
    
    // // 3. Penalty for having two periods of s in one day
    // ILinearNumExpr Z3_t_s = cplex.LinearNumExpr();
    // foreach (var t in teachers)
    // {
    //     foreach (var s in subjects)
    //     {
    //         foreach (var d in days)
    //         {
    //             foreach (var c in classes)
    //             {
    //                 ILinearNumExpr Lc_t_s_d = cplex.LinearNumExpr();
    //                 foreach (var p in d.Hours)
    //                 {
    //                     Lc_t_s_d.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), d.Hours.IndexOf(p), days.IndexOf(d)]);
    //                 }
    //                 Z3_t_s.Add(Lc_t_s_d); // Accumulate terms for the day
    //             }
    //         }
    //     }
    // }
    // IIntVar penaltyVar3 = cplex.BoolVar(); // Create penalty variable
    // cplex.AddEq(penaltyVar3, Z3_t_s);      // Link penalty variable to Z3_t_s
    // Zt_s.AddTerm(W[2], penaltyVar3);
    //
    // // 4. Normalize a schedule
    // ILinearNumExpr Z4_t_s = cplex.LinearNumExpr();
    // foreach (var t in teachers)
    // {
    //     foreach (var s in subjects)
    //     {
    //         foreach (var d in days)
    //         {
    //             foreach (var c in classes)
    //             {
    //                 ILinearNumExpr Lc_t_s_d = cplex.LinearNumExpr();
    //                 foreach (var p in d.Hours)
    //                 {
    //                     Lc_t_s_d.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), d.Hours.IndexOf(p), days.IndexOf(d)]);
    //                 }
    //                 double normalized = (1.0 / dayCount) * c.TotalPeriods;
    //             
    //                 IIntVar penaltyVar = cplex.BoolVar();
    //                 ILinearNumExpr diffExpr = cplex.LinearNumExpr();
    //                 diffExpr.Add(Lc_t_s_d);
    //                 diffExpr.AddTerm(-1.0 * normalized, Yc_t_s[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s)]);
    //             
    //                 // Adjusting the expression to use a linear expression
    //                 ILinearNumExpr diffExprWithPenalty = cplex.LinearNumExpr();
    //                 diffExprWithPenalty.AddTerm(-1.0, penaltyVar);
    //                 diffExprWithPenalty.Add(diffExpr);
    //
    //                 // Now, use the adjusted expression in the constraint
    //                 cplex.AddGe(diffExprWithPenalty, -1.0); // if diffExpr is greater than -1, penalty is zero
    //                 cplex.AddLe(diffExprWithPenalty, 1.0); // if diffExpr is less than 1, penalty is zero
    //             
    //                 Z4_t_s.AddTerm(1.0, penaltyVar);
    //             }
    //         }
    //     }
    // }
    // IIntVar penaltyVar4 = cplex.BoolVar();
    // cplex.AddEq(penaltyVar4, Z4_t_s);
    // Zt_s.AddTerm(W[3], penaltyVar4);
    //
    //
    // // 5. Normalize the teacher schedule
    // ILinearNumExpr Z5_t_s = cplex.LinearNumExpr();
    // foreach (var t in teachers)
    // {
    //     foreach (var s in subjects)
    //     {
    //         foreach (var d in days)
    //         {
    //             ILinearNumExpr expr = cplex.LinearNumExpr();
    //             foreach (var p in d.Hours)
    //             {
    //                 foreach (var c in classes)
    //                 {
    //                     expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), d.Hours.IndexOf(p), days.IndexOf(d)]);
    //                 }
    //             }
    //             double normalized = (1.0 / dayCount) * t.TotalTimeInSchool;
    //
    //             IIntVar penaltyVar = cplex.BoolVar();
    //         
    //             // Adjusting the expression to use a linear expression
    //             ILinearNumExpr diffExpr = cplex.LinearNumExpr();
    //             diffExpr.Add(expr);
    //             diffExpr.AddTerm(-1.0 * normalized, Yc_t_s[0, teachers.IndexOf(t), subjects.IndexOf(s)]);
    //
    //             // Adjusting the constraints to use valid linear expressions
    //             cplex.AddGe(diffExpr, -1.0); // if diffExpr is greater than -1, penaltyVar should be zero or positive
    //             cplex.AddLe(diffExpr, penaltyVar); // if diffExpr is less than or equal to penaltyVar, penaltyVar should be 1 (true)
    //
    //
    //             Z5_t_s.AddTerm(1.0, penaltyVar);
    //         }
    //     }
    // }
    // IIntVar penaltyVar5 = cplex.BoolVar();
    // cplex.AddEq(penaltyVar5, Z5_t_s);
    // Zt_s.AddTerm(W[4], penaltyVar5);
    // // 6. Reducing amount of last periods for a teacher
    // ILinearNumExpr Z6_t_s = cplex.LinearNumExpr();
    // foreach (var t in teachers)
    // {
    //     foreach (var s in subjects)
    //     {
    //         foreach (var d in days)
    //         {
    //             foreach (var c in classes)
    //             {
    //                 int lastPeriodIndex = d.Hours.Count - 1;
    //                 Z6_t_s.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), lastPeriodIndex, days.IndexOf(d)]);
    //             }
    //         }
    //     }
    // }
    // IIntVar penaltyVar6 = cplex.BoolVar(); // Create penalty variable
    // cplex.AddEq(penaltyVar6, Z6_t_s);      // Link penalty variable to Z6_t_s
    // Zt_s.AddTerm(W[5], penaltyVar6);
    
    // Add the overall objective function to the model
    cplex.AddMinimize(objectiveExpr);

    // Constraints
    try
    {
        // 1. No teacher can teach more than one class in a period of a day
        Console.WriteLine("Adding Constraint 1");
        foreach (var t in teachers)
        {
            foreach (var d in days)
            {
                foreach (var p in d.Hours)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    foreach (var c in classes)
                    {
                        foreach (var s in subjects)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), d.Hours.IndexOf(p), days.IndexOf(d)]);
                            Console.WriteLine($"Added constraint for teacher {t.Name}, subject {s.Name}, day {d.Name}, period {p}");
                        }
                    }
                    IConstraint constraint = cplex.AddLe(expr, 1);
                    constraintList.Add(constraint);
                }
            }
        }

        // 2. One class can't have two teachers in a period of a day
        Console.WriteLine("Adding Constraint 2");
        foreach (var c in classes)
        {
            foreach (var d in days)
            {
                foreach (var p in days[0].Hours)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    foreach (var s in subjects)
                    {
                        foreach (var t in teachers)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), days[0].Hours.IndexOf(p), days.IndexOf(d)]);
                        }
                    }
                    IConstraint constraint = cplex.AddLe(expr, 1);
                    constraintList.Add(constraint);
                }
            }
        }

        // 3. Number of periods that teacher t must teach in school
        Console.WriteLine("Adding Constraint 3");
        foreach (var t in teachers)
        {
            ILinearNumExpr expr = cplex.LinearNumExpr();
            foreach (var c in classes)
            {
                foreach (var d in days)
                {
                    foreach (var p in days[0].Hours)
                    {
                        foreach (var s in subjects)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), days[0].Hours.IndexOf(p), days.IndexOf(d)]);
                        }
                    }
                }
            }
            IConstraint constraint = cplex.AddEq(expr, t.TotalTimeInSchool);
            constraintList.Add(constraint);
            
        }

        // 4. Each class must have hc periods in a week
        Console.WriteLine("Adding Constraint 4");
        foreach (var c in classes)
        {
            foreach (var s in subjects)
            {
                ILinearNumExpr expr = cplex.LinearNumExpr();
                foreach (var t in teachers)
                {
                    foreach (var d in days)
                    {
                        foreach (var p in days[0].Hours)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), days[0].Hours.IndexOf(p), days.IndexOf(d)]);
                        }
                    }
                }
                IConstraint constraint = cplex.AddEq(expr, c.GetSubjectHours(s));
                constraintList.Add(constraint);
                Console.WriteLine("Hereee:          " + constraintList.Count);
            }
        }

        // 5. Each class must have a specific number of periods in a week
        Console.WriteLine("Adding Constraint 5");
        foreach (var c in classes)
        {
            ILinearNumExpr expr = cplex.LinearNumExpr();
            foreach (var s in subjects)
            {
                foreach (var t in teachers)
                {
                    foreach (var d in days)
                    {
                        foreach (var p in days[0].Hours)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), days[0].Hours.IndexOf(p), days.IndexOf(d)]);
                        }
                    }
                }
            }
            IConstraint constraint = cplex.AddEq(expr, c.TotalPeriods);
            constraintList.Add(constraint);
        }

        // 6. Teacher availability
        Console.WriteLine("Adding Constraint 6");
        foreach (var t in teachers)
        {
            foreach (var d in days)
            {
                foreach (var p in days[0].Hours)
                {
                    if (!t.Days.Any(day => day.Name == d.Name && day.Hours.Contains(p)))
                    {
                        foreach (var c in classes)
                        {
                            foreach (var s in subjects)
                            {
                                IConstraint constraint = cplex.AddEq(Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), days[0].Hours.IndexOf(p), days.IndexOf(d)], 0);
                                constraintList.Add(constraint);
                            }
                        }
                    }
                }
            }
        }

        // 7. Ensure the same teacher teaches the same subject to a class
        Console.WriteLine("Adding Constraint 7");
        foreach (var c in classes)
        {
            foreach (var s in subjects)
            {
                foreach (var t in teachers)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    foreach (var d in days)
                    {
                        foreach (var p in days[0].Hours)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), days[0].Hours.IndexOf(p), days.IndexOf(d)]);
                        }
                    }
                    ILinearNumExpr rhsExpr = cplex.LinearNumExpr();
                    rhsExpr.AddTerm(c.GetSubjectHours(s), Yc_t_s[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s)]);
                    IConstraint constraint = cplex.AddEq(expr, rhsExpr);
                    constraintList.Add(constraint);
                }
            }
        }

        // 8. Classes must be in continuous periods
        Console.WriteLine("Adding Constraint 8");
        foreach (var c in classes)
        {
            foreach (var d in days)
            {
                for (int p = 0; p < days[0].Hours.Count - 2; p++)
                {
                    ILinearNumExpr expr = cplex.LinearNumExpr();
                    foreach (var s in subjects)
                    {
                        foreach (var t in teachers)
                        {
                            expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), p + 1, days.IndexOf(d)]);
                            expr.AddTerm(-1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), p, days.IndexOf(d)]);
                        }
                    }
                    IConstraint constraint = cplex.AddGe(expr, 0);
                    constraintList.Add(constraint);
                }
            }
        }

        // 9. First period must not be empty
        Console.WriteLine("Adding Constraint 9");
        foreach (var c in classes)
        {
            foreach (var d in days)
            {
                ILinearNumExpr expr = cplex.LinearNumExpr();
                foreach (var s in subjects)
                {
                    foreach (var t in teachers)
                    {
                        expr.AddTerm(1.0, Xc_t_s_p_d[classes.IndexOf(c), teachers.IndexOf(t), subjects.IndexOf(s), 0, days.IndexOf(d)]);
                    }
                }
                IConstraint constraint = cplex.AddEq(expr, 1);
                constraintList.Add(constraint);
            }
        }

        // Solve the problem
        if (cplex.Solve())
        {
            Console.WriteLine("Solution status = " + cplex.GetStatus());
            Console.WriteLine("Objective value = " + cplex.ObjValue);

            // Extract the solution and update the school schedule
            for (int classNum = 0; classNum < school.GetClassNumbers().Count; classNum++)
            {
                foreach (var day in days)
                {
                    foreach (var hour in day.Hours)
                    {
                        int subjectIndex = -1;
                        int teacherIndex = -1;
                        foreach (var subject in subjects)
                        {
                            foreach (var teacher in teachers)
                            {
                                if (cplex.GetValue(Xc_t_s_p_d[classNum, teachers.IndexOf(teacher), subjects.IndexOf(subject), day.Hours.IndexOf(hour), days.IndexOf(day)]) > 0.5)
                                {
                                    subjectIndex = subjects.IndexOf(subject);
                                    teacherIndex = teachers.IndexOf(teacher);
                                }
                            }
                        }
                        if (subjectIndex != -1 && teacherIndex != -1)
                        {
                            school.AssignClass(day, hour, classes[classNum], subjects[subjectIndex], teachers[teacherIndex]);
                        }
                    }
                }
            }

            school.DisplaySchedule();
        }
        else
        {
            
            Console.WriteLine("No solution found." + constraintList.Count);
            AnalyzeInfeasibility(cplex, constraintList);
        }
    }
    catch (ILOG.Concert.Exception ex)
    {
        Console.WriteLine("Concert exception caught: " + ex);
    }
    finally
    {
        cplex.End();
    }
}

    static void AnalyzeInfeasibility(Cplex cplex, List<IConstraint> constraints)
    {
        List<double> preferences = new List<double>();

        // Add constraints and their preferences to the lists (example below)
        foreach (IConstraint constraint in constraints)
        {
            preferences.Add(1.0); // Default preference value
        }

        // Convert lists to arrays for RefineConflict method
        IConstraint[] constraintArray = constraints.ToArray();
        double[] preferenceArray = preferences.ToArray();

        // Conflict refiner settings
        Console.WriteLine("Refining conflict...");

        if (cplex.RefineConflict(constraintArray, preferenceArray))
        {
            for (int c=0; c < constraintArray.Length; c++)
            {
                if (cplex.GetConflict(constraintArray[c]) != Cplex.ConflictStatus.Excluded)
                {
                    Console.WriteLine("Conflict involving constraint: " + c);
                }
            }
        }
        else
        {
            Console.WriteLine("Conflict refiner could not determine the conflicting constraints.");
        }
    }


    static void Validator(Dictionary<int, Dictionary<string, Dictionary<(int, int), (Subject, Teacher)>>> solution)
    {
        // Implement your validation logic here
    }
}
