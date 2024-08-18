namespace ConsoleApp1
{
    public class Subject
    {
        public string Name { get; set; }
        public bool Generality { get; set; }

        public Subject(string name, bool general)
        {
            Name = name;
            Generality = general;
        }
    }
}

