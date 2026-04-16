namespace BusShuttle;

using Spectre.Console;

public class BudgetMenu
{
    private DataManager dataManager;

    public BudgetMenu(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Show()
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Budget Manager")
                .AddChoices(new[] { "See Budget Summary", "Set Overall Budget", "Add Line Item", "Delete Line Item" })
        );

        if (option == "See Budget Summary")
        {
            var total = dataManager.BudgetItems.Sum(i => i.Price);
            if (dataManager.BudgetCap.HasValue)
            {
                var remaining = dataManager.BudgetCap.Value - total;
                AnsiConsole.MarkupLine($"Overall Budget: [cyan]${dataManager.BudgetCap.Value:F2}[/]");
                AnsiConsole.MarkupLine($"Total Spent:    [yellow]${total:F2}[/]");
                if (remaining >= 0)
                    AnsiConsole.MarkupLine($"Remaining:      [green]${remaining:F2}[/]");
                else
                    AnsiConsole.MarkupLine($"Over Budget By: [red]${Math.Abs(remaining):F2}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"Total Spent: [yellow]${total:F2}[/]  (no overall budget set)");
            }

            if (dataManager.BudgetItems.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No line items.[/]");
                return;
            }

            Console.WriteLine();
            foreach (var item in dataManager.BudgetItems)
            {
                Console.WriteLine($"  {item.Vendor,-30} ${item.Price:F2}");
            }
        }
        else if (option == "Set Overall Budget")
        {
            var cap = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Enter overall budget amount:")
                    .Validate(amount =>
                    {
                        return amount <= 0
                            ? ValidationResult.Error("[red]Budget must be greater than zero[/]")
                            : ValidationResult.Success();
                    }));

            dataManager.SetBudgetCap(cap);
            AnsiConsole.MarkupLine($"[green]Overall budget set to ${cap:F2}[/]");
        }
        else if (option == "Add Line Item")
        {
            var vendor = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter vendor name:")
                    .Validate(name =>
                    {
                        return string.IsNullOrWhiteSpace(name)
                            ? ValidationResult.Error("[red]Vendor name cannot be empty[/]")
                            : ValidationResult.Success();
                    }));

            var price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Enter price:")
                    .Validate(amount =>
                    {
                        return amount <= 0
                            ? ValidationResult.Error("[red]Price must be greater than zero[/]")
                            : ValidationResult.Success();
                    }));

            dataManager.AddBudgetItem(new DataManager.BudgetItem(vendor, price));
            AnsiConsole.MarkupLine($"[green]Added '{vendor}' for ${price:F2}[/]");
        }
        else if (option == "Delete Line Item")
        {
            if (dataManager.BudgetItems.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No line items to delete.[/]");
                return;
            }

            var choices = dataManager.BudgetItems.Select(i => $"{i.Vendor} - ${i.Price:F2}").ToList();
            choices.Add("Cancel / Exit");

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which line item do you want to delete?")
                    .AddChoices(choices)
            );

            if (selected == "Cancel / Exit")
                return;

            var index = choices.IndexOf(selected);
            dataManager.DeleteBudgetItem(dataManager.BudgetItems[index]);
            AnsiConsole.MarkupLine("[green]Line item deleted.[/]");
        }
    }
}
