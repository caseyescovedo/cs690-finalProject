namespace EventPlanning.Tests;

using EventPlanning;

public class VolunteersTests
{
    private readonly string _tempDir;
    private readonly string _originalDir;
    private readonly DataManager _dataManager;

    public VolunteersTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _originalDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_tempDir);
        _dataManager = new DataManager();
    }

    // void IDisposable.Dispose()
    // {
    //     Directory.SetCurrentDirectory(_originalDir);
    //     Directory.Delete(_tempDir, recursive: true);
    // }

    [Fact]
    public void AddVolunteer_IncreasesCount()
    {
        _dataManager.AddVolunteer(new DataManager.Volunteer("Alice"));
        Assert.Single(_dataManager.Volunteers);
    }

    [Fact]
    public void AddVolunteer_SavesName()
    {
        _dataManager.AddVolunteer(new DataManager.Volunteer("Bob"));
        Assert.Equal("Bob", _dataManager.Volunteers[0].Name);
    }

    [Fact]
    public void RemoveVolunteer_DecreasesCount()
    {
        var volunteer = new DataManager.Volunteer("Charlie");
        _dataManager.AddVolunteer(volunteer);
        _dataManager.RemoveVolunteer(volunteer);
        Assert.Empty(_dataManager.Volunteers);
    }
}
