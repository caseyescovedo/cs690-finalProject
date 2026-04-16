namespace BusShuttle;

using Spectre.Console;

public class TasksMenu
{
    private DataManager dataManager;

    public TasksMenu(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Show()
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Tasks Manager")
                .AddChoices(new[] { "See Tasks", "Add Task", "Change Task Status", "Delete Task" })
        );

        if (option == "See Tasks")
        {
            if (dataManager.Tasks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No tasks found.[/]");
                return;
            }

            foreach (var task in dataManager.Tasks)
            {
                var statusColor = task.Status switch
                {
                    DataManager.TaskStatus.NotStarted => "grey",
                    DataManager.TaskStatus.InProgress => "yellow",
                    DataManager.TaskStatus.Done => "green",
                    _ => "white"
                };
                AnsiConsole.MarkupLine($"[{statusColor}]{task.Status,-12}[/]  {task.Title,-30}  Assigned: {task.VolunteerName}");
            }
        }
        else if (option == "Add Task")
        {
            if (dataManager.Volunteers.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No volunteers available. Add volunteers first.[/]");
                return;
            }

            var title = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter task title:")
                    .Validate(t =>
                    {
                        return string.IsNullOrWhiteSpace(t)
                            ? ValidationResult.Error("[red]Title cannot be empty[/]")
                            : ValidationResult.Success();
                    }));

            var volunteerName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Assign to volunteer:")
                    .AddChoices(dataManager.Volunteers.Select(v => v.Name))
            );

            dataManager.AddTask(new DataManager.Task(title, volunteerName));
            AnsiConsole.MarkupLine($"[green]Task '{title}' added and assigned to {volunteerName}.[/]");
        }
        else if (option == "Change Task Status")
        {
            if (dataManager.Tasks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No tasks to update.[/]");
                return;
            }

            var taskChoices = dataManager.Tasks
                .Select(t => $"{t.Title} ({t.VolunteerName}) [{t.Status}]")
                .ToList();

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which task do you want to update?")
                    .AddChoices(taskChoices)
            );

            var index = taskChoices.IndexOf(selected);
            var task = dataManager.Tasks[index];

            var newStatus = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select new status:")
                    .AddChoices(new[] { "Not Started", "In Progress", "Done" })
            );

            var status = newStatus switch
            {
                "Not Started" => DataManager.TaskStatus.NotStarted,
                "In Progress" => DataManager.TaskStatus.InProgress,
                "Done" => DataManager.TaskStatus.Done,
                _ => DataManager.TaskStatus.NotStarted
            };

            dataManager.UpdateTaskStatus(task, status);
            AnsiConsole.MarkupLine($"[green]Task updated to '{newStatus}'.[/]");
        }
        else if (option == "Delete Task")
        {
            if (dataManager.Tasks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No tasks to delete.[/]");
                return;
            }

            var taskChoices = dataManager.Tasks
                .Select(t => $"{t.Title} ({t.VolunteerName}) [{t.Status}]")
                .ToList();
            taskChoices.Add("Cancel / Exit");

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which task do you want to delete?")
                    .AddChoices(taskChoices)
            );

            if (selected == "Cancel / Exit")
                return;

            var index = taskChoices.IndexOf(selected);
            dataManager.DeleteTask(dataManager.Tasks[index]);
            AnsiConsole.MarkupLine("[green]Task deleted.[/]");
        }
    }
}
