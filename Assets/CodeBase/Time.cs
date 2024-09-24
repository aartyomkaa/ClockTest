namespace CodeBase
{
    public class Time
    {
        private const int Count = 60;
        private const int HoursCount = 24;

        public int Hour { get; private set; }
        public int Minute { get; private set; }
        public int Second { get; private set; }

        public void Synchronize(int hours, int minutes, int seconds)
        {
            Hour = hours;
            Minute = minutes;
            Second = seconds;
        }

        public void Update(int seconds)
        {
            Second += seconds;
            
            if (Second == Count)
            {
                Minute += 1;
                Second = 0;
            }

            if (Minute == Count)
            {
                Hour += 1;
                Minute = 0;
            }

            if (Hour == HoursCount) 
                Hour = 0;
        }
    }
}