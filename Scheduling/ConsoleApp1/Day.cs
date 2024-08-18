namespace ConsoleApp1
{
    public class Day
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
}

