namespace EventPlanning;

public class DataManager
{
    FileSaver fileSaver;
    public List<Meeting> Meetings { get; }
    public List<Note> Notes { get; }
    public List<BudgetItem> BudgetItems { get; }
    public decimal? BudgetCap { get; private set; }
    public List<Volunteer> Volunteers { get; }
    public List<Task> Tasks { get; }

    public enum TaskStatus { NotStarted, InProgress, Done }

    public class Volunteer
    {
        public string Name { get; }

        public Volunteer(string name)
        {
            Name = name;
        }
    }

    public class Task
    {
        public string Title { get; }
        public string VolunteerName { get; set; }
        public TaskStatus Status { get; set; }

        public Task(string title, string volunteerName = "Unassigned", TaskStatus status = TaskStatus.NotStarted)
        {
            Title = title;
            VolunteerName = volunteerName;
            Status = status;
        }
    }

    public class BudgetItem
    {
        public string Vendor { get; }
        public decimal Price { get; }

        public BudgetItem(string vendor, decimal price)
        {
            Vendor = vendor;
            Price = price;
        }
    }

    public class Note
    {
        public string Content { get; }

        public Note(string content)
        {
            Content = content;
        }
    }

    public class Meeting
    {
        public string Title { get; }
        public DateTime Time { get; }

        public Meeting(string title, DateTime time)
        {
            Title = title;
            Time = time;
        }
    }

    public DataManager()
    {
        fileSaver = new FileSaver("passenger-data.txt");
        Meetings = new List<Meeting>();
        Notes = new List<Note>();
        BudgetItems = new List<BudgetItem>();
        Volunteers = new List<Volunteer>();
        Tasks = new List<Task>();
        if (File.Exists("volunteers.txt"))
        {
            foreach (var line in File.ReadAllLines("volunteers.txt"))
                if (!string.IsNullOrWhiteSpace(line))
                    Volunteers.Add(new Volunteer(line.Trim()));
        }
        if (File.Exists("tasks.txt"))
        {
            foreach (var line in File.ReadAllLines("tasks.txt"))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split("|", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3 && Enum.TryParse<TaskStatus>(parts[2], out var status))
                    Tasks.Add(new Task(parts[0], parts[1], status));
            }
        }
        if (File.Exists("budget-cap.txt"))
        {
            var capLine = File.ReadAllText("budget-cap.txt").Trim();
            if (decimal.TryParse(capLine, out var cap))
                BudgetCap = cap;
        }
        if (File.Exists("budget-items.txt"))
        {
            var itemLines = File.ReadAllLines("budget-items.txt");
            foreach (var line in itemLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split("|", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && decimal.TryParse(parts[1], out var price))
                    BudgetItems.Add(new BudgetItem(parts[0], price));
            }
        }
        if (File.Exists("notes.txt"))
        {
            var noteLines = File.ReadAllLines("notes.txt");
            foreach (var line in noteLines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    Notes.Add(new Note(line));
            }
        }
        if (File.Exists("meetings.txt"))
        {
            var meetingLines = File.ReadAllLines("meetings.txt");
            foreach (var line in meetingLines)
            {
                var parts = line.Split("|", StringSplitOptions.RemoveEmptyEntries);
                var title = parts[0];
                var dateTime = DateTime.Parse(parts[1]);
                Meetings.Add(new Meeting(title, dateTime));
            }
        }
    }

   

    public void AddMeeting(Meeting meeting)
    {
        Meetings.Add(meeting);
        var meetingData = $"{meeting.Title}|{meeting.Time:yyyy-MM-dd HH:mm}";
        File.AppendAllText("meetings.txt", meetingData + Environment.NewLine);
    }

    public void RemoveMeeting(Meeting meeting)
    {
        Meetings.Remove(meeting);
        File.WriteAllLines("meetings.txt", Meetings.Select(m => $"{m.Title}|{m.Time:yyyy-MM-dd HH:mm}"));
    }

    public void UpdateMeeting(Meeting oldMeeting, Meeting newMeeting)
    {
        var index = Meetings.IndexOf(oldMeeting);
        Meetings[index] = newMeeting;
        File.WriteAllLines("meetings.txt", Meetings.Select(m => $"{m.Title}|{m.Time:yyyy-MM-dd HH:mm}"));
    }

    public List<Meeting> GetMeetings(){
    return new List<Meeting>(Meetings);
    }

    public void SetBudgetCap(decimal cap)
    {
        BudgetCap = cap;
        File.WriteAllText("budget-cap.txt", cap.ToString());
    }

    public void AddBudgetItem(BudgetItem item)
    {
        BudgetItems.Add(item);
        File.AppendAllText("budget-items.txt", $"{item.Vendor}|{item.Price}" + Environment.NewLine);
    }

    public void DeleteBudgetItem(BudgetItem item)
    {
        BudgetItems.Remove(item);
        File.WriteAllLines("budget-items.txt", BudgetItems.Select(i => $"{i.Vendor}|{i.Price}"));
    }

    public void AddVolunteer(Volunteer volunteer)
    {
        Volunteers.Add(volunteer);
        File.AppendAllText("volunteers.txt", volunteer.Name + Environment.NewLine);
    }

    public void RemoveVolunteer(Volunteer volunteer)
    {
        Volunteers.Remove(volunteer);
        File.WriteAllLines("volunteers.txt", Volunteers.Select(v => v.Name));
    }

    public void AddTask(Task task)
    {
        Tasks.Add(task);
        File.AppendAllText("tasks.txt", $"{task.Title}|{task.VolunteerName}|{task.Status}" + Environment.NewLine);
    }

    public void UpdateTaskStatus(Task task, TaskStatus status)
    {
        task.Status = status;
        File.WriteAllLines("tasks.txt", Tasks.Select(t => $"{t.Title}|{t.VolunteerName}|{t.Status}"));
    }

    public void AssignTask(Task task, string volunteerName)
    {
        task.VolunteerName = volunteerName;
        File.WriteAllLines("tasks.txt", Tasks.Select(t => $"{t.Title}|{t.VolunteerName}|{t.Status}"));
    }

    public void DeleteTask(Task task)
    {
        Tasks.Remove(task);
        File.WriteAllLines("tasks.txt", Tasks.Select(t => $"{t.Title}|{t.VolunteerName}|{t.Status}"));
    }

    public void AddNote(Note note)
    {
        Notes.Add(note);
        File.AppendAllText("notes.txt", note.Content + Environment.NewLine);
    }

    public void DeleteNote(Note note)
    {
        Notes.Remove(note);
        File.WriteAllLines("notes.txt", Notes.Select(n => n.Content));
    }
}