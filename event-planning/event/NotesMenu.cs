namespace EventPlanning;

using Spectre.Console;

public class NotesMenu
{
    private DataManager dataManager;

    public NotesMenu(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Show()
    {
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Notes Manager")
                    .AddChoices(new[] { "See Notes", "Add Note", "Delete Note", "Back" })
            );

            if (option == "Back")
                return;

            if (option == "See Notes")
            {
                if (dataManager.Notes.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No notes found.[/]");
                    continue;
                }
                foreach (var note in dataManager.Notes)
                {
                    Console.WriteLine(note.Content);
                }
            }
            else if (option == "Add Note")
            {
                var content = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter note:")
                        .Validate(text =>
                        {
                            return string.IsNullOrWhiteSpace(text)
                                ? ValidationResult.Error("[red]Note cannot be empty[/]")
                                : ValidationResult.Success();
                        }));

                dataManager.AddNote(new DataManager.Note(content));
                AnsiConsole.MarkupLine("[green]Note added.[/]");
            }
            else if (option == "Delete Note")
            {
                if (dataManager.Notes.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No notes to delete.[/]");
                    continue;
                }

                var choices = dataManager.Notes.Select(n => n.Content).ToList();
                choices.Add("Cancel / Exit");

                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which note do you want to delete?")
                        .AddChoices(choices)
                );

                if (selected == "Cancel / Exit")
                    continue;

                var noteToDelete = dataManager.Notes.First(n => n.Content == selected);
                dataManager.DeleteNote(noteToDelete);
                AnsiConsole.MarkupLine("[green]Note deleted.[/]");
            }
        }
    }
}
