namespace EventPlanning.Tests;

using EventPlanning;

public class ConsoleUITests
{
    [Fact]
    public void AskForInput_ReturnsTypedInput()
    {
        Console.SetIn(new StringReader("hello world"));
        var result = ConsoleUI.AskForInput("Enter something: ");
        Assert.Equal("hello world", result);
    }

    [Fact]
    public void AskForInput_ReturnsEmptyString_WhenInputIsEmpty()
    {
        Console.SetIn(new StringReader(""));
        var result = ConsoleUI.AskForInput("Enter something: ");
        Assert.Equal("", result);
    }

    [Fact]
    public void ConsoleUI_CanBeInstantiated()
    {
        var ui = new ConsoleUI();
        Assert.NotNull(ui);
    }
}
