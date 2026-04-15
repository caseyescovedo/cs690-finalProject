namespace BusShuttle;

using Spectre.Console;

public class ConsoleUI
{
    private DataManager dataManager;

    public ConsoleUI()
    {
        dataManager = new DataManager();
    }

    public void Show()
    {
        var mode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Event Manager Menu")
                .AddChoices(new[] { "Meetings", "Layout", "Volunteers", "Budget", "Notes" })
        );

        switch (mode)
        {
            case "Meetings":
                ShowMeetingsMenu();
                break;

            case "Layout":
                Console.WriteLine("You are now in Layout");
                break;

            case "Volunteers":
                Console.WriteLine("You are now in Volunteers");
                break;

            case "Budget":
                Console.WriteLine("You are now in Budget");
                break;

            case "Notes":
                Console.WriteLine("You are now in Notes");
                break;
        }
    }

    private void ShowMeetingsMenu()
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
            
            // Logic to handle deletion would go here
        }
    }

    public static string AskForInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }
}