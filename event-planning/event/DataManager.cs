namespace BusShuttle;

public class DataManager
{
    FileSaver fileSaver;
    public List<Meeting> Meetings { get; }

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

    public List<Meeting> GetMeetings(){
    return new List<Meeting>(Meetings);
    }



}