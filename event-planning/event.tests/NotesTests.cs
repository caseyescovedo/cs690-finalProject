namespace EventPlanning.Tests;

using EventPlanning;

public class NotesTests
{
    private readonly string _tempDir;
    private readonly string _originalDir;
    private readonly DataManager _dataManager;

    public NotesTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _originalDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_tempDir);
        _dataManager = new DataManager();
    }

    [Fact]
    public void AddNote_IncreasesCount()
    {
        _dataManager.AddNote(new DataManager.Note("Buy Balloons"));
        Assert.Single(_dataManager.Notes);
    }

    [Fact]
    public void AddNote_SavesContent()
    {
        _dataManager.AddNote(new DataManager.Note("Buy Balloons"));
        Assert.Equal("Buy Balloons", _dataManager.Notes[0].Content);
    }

    [Fact]
    public void DeleteNote_DecreasesCount()
    {
        var note = new DataManager.Note("Buy Balloons");
        _dataManager.AddNote(note);
        _dataManager.DeleteNote(note);
        Assert.Empty(_dataManager.Notes);
    }
}
