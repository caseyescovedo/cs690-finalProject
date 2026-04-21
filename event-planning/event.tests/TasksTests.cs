namespace EventPlanning.Tests;

using EventPlanning;

public class TasksTests
{
    private readonly string _tempDir;
    private readonly string _originalDir;
    private readonly DataManager _dataManager;

    public TasksTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _originalDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_tempDir);
        _dataManager = new DataManager();
    }

    [Fact]
    public void AddTask_IncreasesCount()
    {
        _dataManager.AddTask(new DataManager.Task("Setup tables"));
        Assert.Single(_dataManager.Tasks);
    }

    [Fact]
    public void AddTask_DefaultsToUnassignedAndNotStarted()
    {
        _dataManager.AddTask(new DataManager.Task("Setup tables"));
        var task = _dataManager.Tasks[0];
        Assert.Equal("Unassigned", task.VolunteerName);
        Assert.Equal(DataManager.TaskStatus.NotStarted, task.Status);
    }

    [Fact]
    public void DeleteTask_DecreasesCount()
    {
        var task = new DataManager.Task("Setup tables");
        _dataManager.AddTask(task);
        _dataManager.DeleteTask(task);
        Assert.Empty(_dataManager.Tasks);
    }

    [Fact]
    public void AssignTask_UpdatesVolunteerName()
    {
        var task = new DataManager.Task("Setup tables");
        _dataManager.AddTask(task);
        _dataManager.AssignTask(task, "Casey");
        Assert.Equal("Casey", _dataManager.Tasks[0].VolunteerName);
    }

    [Fact]
    public void UpdateTaskStatus_ChangesStatus()
    {
        var task = new DataManager.Task("Setup tables");
        _dataManager.AddTask(task);
        _dataManager.UpdateTaskStatus(task, DataManager.TaskStatus.Done);
        Assert.Equal(DataManager.TaskStatus.Done, _dataManager.Tasks[0].Status);
    }
}
