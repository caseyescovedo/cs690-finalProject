namespace EventPlanning.Tests;

using EventPlanning;

public class MeetingsTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _originalDir;
    private readonly DataManager _dataManager;

    public MeetingsTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _originalDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_tempDir);
        _dataManager = new DataManager();
    }

    void IDisposable.Dispose()
    {
        Directory.SetCurrentDirectory(_originalDir);
        Directory.Delete(_tempDir, recursive: true);
    }

    [Fact]
    public void AddMeeting_IncreasesCount()
    {
        var meeting = new DataManager.Meeting("Test 1", DateTime.Now.AddDays(1));
        _dataManager.AddMeeting(meeting);
        Assert.Single(_dataManager.Meetings);
    }

    [Fact]
    public void AddMeeting_SaveTitleAndTime()
    {
        var time = DateTime.Now.AddDays(1);
        var meeting = new DataManager.Meeting("Test 2", time);
        _dataManager.AddMeeting(meeting);
        Assert.Equal("Test 2", _dataManager.Meetings[0].Title);
        Assert.Equal(time, _dataManager.Meetings[0].Time);
    }

    [Fact]
    public void RemoveMeeting_DecreasesCount()
    {
        var meeting = new DataManager.Meeting("Test 2", DateTime.Now.AddDays(1));
        _dataManager.AddMeeting(meeting);
        _dataManager.RemoveMeeting(meeting);
        Assert.Empty(_dataManager.Meetings);
    }

    [Fact]
    public void UpdateMeeting_NewTitle()
    {
        var original = new DataManager.Meeting("Old Title", DateTime.Now.AddDays(1));
        _dataManager.AddMeeting(original);
        var updated = new DataManager.Meeting("New Title", original.Time);
        _dataManager.UpdateMeeting(original, updated);
        Assert.Equal("New Title", _dataManager.Meetings[0].Title);
    }

    [Fact]
    public void GetMeetings_GetsMeetings()
    {
        var meeting = new DataManager.Meeting("Test 2", DateTime.Now.AddDays(1));
        _dataManager.AddMeeting(meeting);
        var copy = _dataManager.GetMeetings();
        copy.Clear();
        Assert.Single(_dataManager.Meetings);
    }
}
