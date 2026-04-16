namespace BusShuttle;

using Spectre.Console;

public class MeetingsMenu
{
    private DataManager dataManager;

    public MeetingsMenu(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Show()
    {
        var meetingOptions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Meetings Manager")
                .AddChoices(new[] { "See Schedule", "Create Meeting", "Cancel Meeting" })
        );

        if (meetingOptions == "See Schedule")
        {
            foreach (var meeting in dataManager.Meetings)
            {
                Console.WriteLine($"{meeting.Title} - {meeting.Time:yyyy-MM-dd HH:mm}");
            }
        }
        else if (meetingOptions == "Create Meeting")
        {
            var newMeetingName = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter meeting title:")
                    .Validate(title =>
                    {
                        return string.IsNullOrWhiteSpace(title)
                            ? ValidationResult.Error("[red]Title cannot be empty[/]")
                            : ValidationResult.Success();
                    }));

            var newMeetingTime = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter meeting day and time (yyyy-MM-dd HH:mm):")
                    .Validate(date =>
                    {
                        return date < DateTime.Now
                            ? ValidationResult.Error("[red]Meeting cannot be in the past[/]")
                            : ValidationResult.Success();
                    }));

            var meeting = new DataManager.Meeting(newMeetingName, newMeetingTime);
            dataManager.AddMeeting(meeting);

            AnsiConsole.MarkupLine($"[green]Meeting '{newMeetingName}' scheduled for {newMeetingTime:yyyy-MM-dd HH:mm}[/]");
        }
        else if (meetingOptions == "Cancel Meeting")
        {
            var allMeetings = dataManager.Meetings
                .Select(m => $"{m.Title} - {m.Time:yyyy-MM-dd HH:mm}")
                .ToList();
            allMeetings.Add("Cancel / Exit");

            if (allMeetings.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No meetings scheduled to cancel.[/]");
                return;
            }

            var selectedMeeting = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which meeting do you want to cancel?")
                    .AddChoices(allMeetings)
            );
        }
    }
}
