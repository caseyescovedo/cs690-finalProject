namespace EventPlanning.Tests;

using EventPlanning;

public class LayoutTests
{
    private readonly string _tempDir;
    private readonly string _originalDir;
    private readonly DataManager _dataManager;

    public LayoutTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _originalDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_tempDir);
        _dataManager = new DataManager();
    }

    [Fact]
    public void Layout_InitializesWithTenUnassignedSpaces()
    {
        Assert.Equal(10, _dataManager.Layout.Count);
        Assert.All(_dataManager.Layout, s => Assert.Equal("Unassigned", s.Vendor));
    }

    [Fact]
    public void AssignSpace_UpdatesVendorName()
    {
        var space = _dataManager.Layout.First(s => s.Number == 1);
        space.Vendor = "Apple";
        _dataManager.SaveLayout();
        Assert.Equal("Apple", _dataManager.Layout.First(s => s.Number == 1).Vendor);
    }

    [Fact]
    public void UnassignSpace_ResetsVendorToUnassigned()
    {
        var space = _dataManager.Layout.First(s => s.Number == 3);
        space.Vendor = "Meta";
        _dataManager.SaveLayout();

        space.Vendor = "Unassigned";
        _dataManager.SaveLayout();

        Assert.Equal("Unassigned", _dataManager.Layout.First(s => s.Number == 3).Vendor);
    }

    
}
