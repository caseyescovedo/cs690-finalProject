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
        while (true)
        {
            var meetingOptions = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Meetings Manager")
                    .AddChoices(new[] { "See Schedule", "Create Meeting", "Edit Meeting", "Cancel Meeting", "Back" })
            );

            if (meetingOptions == "Back")
                return;

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
            else if (meetingOptions == "Edit Meeting")
            {
                if (dataManager.Meetings.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No meetings scheduled to edit.[/]");
                    continue;
                }

                var allMeetings = dataManager.Meetings
                    .Select(m => $"{m.Title} - {m.Time:yyyy-MM-dd HH:mm}")
                    .ToList();
                allMeetings.Add("Cancel / Exit");

                var selectedMeeting = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which meeting do you want to edit?")
                        .AddChoices(allMeetings)
                );

                if (selectedMeeting == "Cancel / Exit")
                    continue;

                var meeting = dataManager.Meetings
                    .First(m => $"{m.Title} - {m.Time:yyyy-MM-dd HH:mm}" == selectedMeeting);

                var newTitle = AnsiConsole.Prompt(
                    new TextPrompt<string>("Title:")
                        .DefaultValue(meeting.Title)
                        .Validate(title =>
                        {
                            return string.IsNullOrWhiteSpace(title)
                                ? ValidationResult.Error("[red]Title cannot be empty[/]")
                                : ValidationResult.Success();
                        }));

                var newTime = AnsiConsole.Prompt(
                    new TextPrompt<DateTime>("Date and time (yyyy-MM-dd HH:mm):")
                        .DefaultValue(meeting.Time)
                        .Validate(date =>
                        {
                            return date < DateTime.Now
                                ? ValidationResult.Error("[red]Meeting cannot be in the past[/]")
                                : ValidationResult.Success();
                        }));

                dataManager.UpdateMeeting(meeting, new DataManager.Meeting(newTitle, newTime));
                AnsiConsole.MarkupLine($"[green]Meeting updated to '{newTitle}' at {newTime:yyyy-MM-dd HH:mm}[/]");
            }
            else if (meetingOptions == "Cancel Meeting")
            {
                if (dataManager.Meetings.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No meetings scheduled to cancel.[/]");
                    continue;
                }

                var allMeetings = dataManager.Meetings
                    .Select(m => $"{m.Title} - {m.Time:yyyy-MM-dd HH:mm}")
                    .ToList();
                allMeetings.Add("Cancel / Exit");

                var selectedMeeting = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which meeting do you want to cancel?")
                        .AddChoices(allMeetings)
                );

                if (selectedMeeting == "Cancel / Exit")
                    continue;

                var meeting = dataManager.Meetings
                    .First(m => $"{m.Title} - {m.Time:yyyy-MM-dd HH:mm}" == selectedMeeting);
                dataManager.RemoveMeeting(meeting);
                AnsiConsole.MarkupLine($"[red]Meeting '{meeting.Title}' has been cancelled.[/]");
            }
        }
    }
}
