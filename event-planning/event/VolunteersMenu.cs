namespace EventPlanning;

using Spectre.Console;

public class VolunteersMenu
{
    private DataManager dataManager;

    public VolunteersMenu(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Show()
    {
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Volunteers Manager")
                    .AddChoices(new[] { "See Volunteers", "Add Volunteer", "Delete Volunteer", "Back" })
            );

            if (option == "Back")
                return;

            if (option == "See Volunteers")
            {
                if (dataManager.Volunteers.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No volunteers registered.[/]");
                    continue;
                }

                foreach (var volunteer in dataManager.Volunteers)
                {
                    Console.WriteLine(volunteer.Name);
                }
            }
            else if (option == "Add Volunteer")
            {
                var name = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter volunteer name:")
                        .Validate(n =>
                        {
                            return string.IsNullOrWhiteSpace(n)
                                ? ValidationResult.Error("[red]Name cannot be empty[/]")
                                : ValidationResult.Success();
                        }));

                dataManager.AddVolunteer(new DataManager.Volunteer(name));
                AnsiConsole.MarkupLine($"[green]Volunteer '{name}' added.[/]");
            }
            else if (option == "Delete Volunteer")
            {
                if (dataManager.Volunteers.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No volunteers to delete.[/]");
                    continue;
                }

                var choices = dataManager.Volunteers.Select(v => v.Name).ToList();
                choices.Add("Cancel / Exit");

                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which volunteer do you want to remove?")
                        .AddChoices(choices)
                );

                if (selected == "Cancel / Exit")
                    continue;

                var volunteer = dataManager.Volunteers.First(v => v.Name == selected);
                dataManager.RemoveVolunteer(volunteer);
                AnsiConsole.MarkupLine($"[red]Volunteer '{selected}' removed.[/]");
            }
        }
    }
}
