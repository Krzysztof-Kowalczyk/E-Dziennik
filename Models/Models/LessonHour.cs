namespace Models.Models
{
    public enum SchoolDay
    {
       Poniedziałek,
        Wtorek,
        Środa,
        Czwartek,
        Piątek
    }

    public class LessonHour
    {
        public SchoolDay SchoolDay {get; set;}
        public int Lesson { get; set; }

    }
}