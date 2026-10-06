namespace home_work_54_lab_work.Models;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Comment>?Comments { get; set; }
}