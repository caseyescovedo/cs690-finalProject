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
                .AddChoices(new[] { "Meetings", "Layout", "Volunteers", "Budget", "Notes", "Tasks" })
        );

        switch (mode)
        {
            case "Meetings":
                new MeetingsMenu(dataManager).Show();
                break;

            case "Layout":
                Console.WriteLine("You are now in Layout");
                break;

            case "Volunteers":
                Console.WriteLine("You are now in Volunteers");
                break;

            case "Budget":
                new BudgetMenu(dataManager).Show();
                break;

            case "Notes":
                new NotesMenu(dataManager).Show();
                break;

            case "Tasks":
                new TasksMenu(dataManager).Show();
                break;
        }
    }

    public static string AskForInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }
}