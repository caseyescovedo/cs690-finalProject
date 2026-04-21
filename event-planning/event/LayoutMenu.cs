namespace EventPlanning;

using Spectre.Console;

public class LayoutMenu
{
    private DataManager dataManager;

    public LayoutMenu(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Show()
    {
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Layout Manager")
                    .AddChoices(new[] { "View Layout", "Assign Space", "Edit Space", "Unassign Space", "Back" })
            );

            if (option == "Back")
                return;

            if (option == "View Layout")
            {
                foreach (var space in dataManager.Layout)
                {
                    var color = space.Vendor == "Unassigned" ? "grey" : "green";
                    AnsiConsole.MarkupLine($"Space [cyan]{space.Number,2}[/]  [{color}]{space.Vendor}[/]");
                }
            }
            else if (option == "Assign Space")
            {
                var unassigned = dataManager.Layout.Where(s => s.Vendor == "Unassigned").ToList();
                if (unassigned.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]All spaces are already assigned.[/]");
                    continue;
                }

                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which space do you want to assign?")
                        .AddChoices(unassigned.Select(s => $"Space {s.Number}"))
                );

                var spaceNum = int.Parse(selected.Replace("Space ", ""));
                var space = dataManager.Layout.First(s => s.Number == spaceNum);

                var vendor = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter vendor name:")
                        .Validate(v =>
                        {
                            return string.IsNullOrWhiteSpace(v)
                                ? ValidationResult.Error("[red]Vendor name cannot be empty[/]")
                                : ValidationResult.Success();
                        }));

                space.Vendor = vendor;
                dataManager.SaveLayout();
                AnsiConsole.MarkupLine($"[green]Space {spaceNum} assigned to '{vendor}'.[/]");
            }
            else if (option == "Edit Space")
            {
                var assigned = dataManager.Layout.Where(s => s.Vendor != "Unassigned").ToList();
                if (assigned.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No spaces are currently assigned.[/]");
                    continue;
                }

                var choices = assigned.Select(s => $"Space {s.Number} — {s.Vendor}").ToList();
                choices.Add("Cancel / Exit");

                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which space do you want to edit?")
                        .AddChoices(choices)
                );

                if (selected == "Cancel / Exit")
                    continue;

                var spaceNum = int.Parse(selected.Split(' ')[1]);
                var space = dataManager.Layout.First(s => s.Number == spaceNum);

                var vendor = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter new vendor name:")
                        .DefaultValue(space.Vendor)
                        .Validate(v =>
                        {
                            return string.IsNullOrWhiteSpace(v)
                                ? ValidationResult.Error("[red]Vendor name cannot be empty[/]")
                                : ValidationResult.Success();
                        }));

                space.Vendor = vendor;
                dataManager.SaveLayout();
                AnsiConsole.MarkupLine($"[green]Space {spaceNum} updated to '{vendor}'.[/]");
            }
            else if (option == "Unassign Space")
            {
                var assigned = dataManager.Layout.Where(s => s.Vendor != "Unassigned").ToList();
                if (assigned.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No spaces are currently assigned.[/]");
                    continue;
                }

                var choices = assigned.Select(s => $"Space {s.Number} — {s.Vendor}").ToList();
                choices.Add("Cancel / Exit");

                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which space do you want to unassign?")
                        .AddChoices(choices)
                );

                if (selected == "Cancel / Exit")
                    continue;

                var spaceNum = int.Parse(selected.Split(' ')[1]);
                var space = dataManager.Layout.First(s => s.Number == spaceNum);
                space.Vendor = "Unassigned";
                dataManager.SaveLayout();
                AnsiConsole.MarkupLine($"[red]Space {spaceNum} is now unassigned.[/]");
            }
        }
    }
}
