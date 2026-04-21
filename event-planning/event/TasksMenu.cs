namespace EventPlanning;

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
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Tasks Manager")
                    .AddChoices(new[] { "See Tasks", "Add Task", "Assign Task", "Change Task Status", "Delete Task", "Back" })
            );

            if (option == "Back")
                return;

            if (option == "See Tasks")
            {
                if (dataManager.Tasks.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No tasks found.[/]");
                    continue;
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
                var title = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter task title:")
                        .Validate(t =>
                        {
                            return string.IsNullOrWhiteSpace(t)
                                ? ValidationResult.Error("[red]Title cannot be empty[/]")
                                : ValidationResult.Success();
                        }));

                string volunteerName = "Unassigned";

                if (dataManager.Volunteers.Count > 0)
                {
                    var assignNow = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Assign to a volunteer now?")
                            .AddChoices(new[] { "Yes", "No (leave unassigned)" })
                    );

                    if (assignNow == "Yes")
                    {
                        volunteerName = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Assign to volunteer:")
                                .AddChoices(dataManager.Volunteers.Select(v => v.Name))
                        );
                    }
                }

                dataManager.AddTask(new DataManager.Task(title, volunteerName));
                AnsiConsole.MarkupLine($"[green]Task '{title}' created. Assigned to: {volunteerName}[/]");
            }
            else if (option == "Assign Task")
            {
                if (dataManager.Tasks.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No tasks available.[/]");
                    continue;
                }

                if (dataManager.Volunteers.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No volunteers registered. Add volunteers first.[/]");
                    continue;
                }

                var taskChoices = dataManager.Tasks
                    .Select(t => $"{t.Title} [[{t.VolunteerName}]]")
                    .ToList();
                taskChoices.Add("Cancel / Exit");

                var selectedTask = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which task do you want to assign?")
                        .AddChoices(taskChoices)
                );

                if (selectedTask == "Cancel / Exit")
                    continue;

                var index = taskChoices.IndexOf(selectedTask);
                var task = dataManager.Tasks[index];

                var volunteerName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Assign to volunteer:")
                        .AddChoices(dataManager.Volunteers.Select(v => v.Name))
                );

                dataManager.AssignTask(task, volunteerName);
                AnsiConsole.MarkupLine($"[green]Task '{task.Title}' assigned to {volunteerName}.[/]");
            }
            else if (option == "Change Task Status")
            {
                if (dataManager.Tasks.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No tasks to update.[/]");
                    continue;
                }

                var taskChoices = dataManager.Tasks
                    .Select(t => $"{t.Title} ({t.VolunteerName}) [[{t.Status}]]")
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
                    continue;
                }

                var taskChoices = dataManager.Tasks
                    .Select(t => $"{t.Title} ({t.VolunteerName}) [[{t.Status}]]")
                    .ToList();
                taskChoices.Add("Cancel / Exit");

                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which task do you want to delete?")
                        .AddChoices(taskChoices)
                );

                if (selected == "Cancel / Exit")
                    continue;

                var index = taskChoices.IndexOf(selected);
                dataManager.DeleteTask(dataManager.Tasks[index]);
                AnsiConsole.MarkupLine("[green]Task deleted.[/]");
            }
        }
    }
}
