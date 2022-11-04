using NUnit.Framework;

namespace StardewTests.Harness; 

public class TestModEventsTest {
    
    private TestModEvents _classUnderTest = new();

    [SetUp]
    public void SetUp() {
        _classUnderTest = new();
    }

    [Test]
    public void Content() {
        Assert.NotNull(_classUnderTest.Content);
    }
    
    [Test]
    public void Display() {
        Assert.NotNull(_classUnderTest.Display);
    }
    
    [Test]
    public void GameLoop() {
        Assert.NotNull(_classUnderTest.GameLoop);
    }
    
    [Test]
    public void Input() {
        Assert.NotNull(_classUnderTest.Input);
    }
    
    [Test]
    public void Multiplayer() {
        Assert.NotNull(_classUnderTest.Multiplayer);
    }
    
    [Test]
    public void Player() {
        Assert.NotNull(_classUnderTest.Player);
    }
    
    [Test]
    public void World() {
        Assert.NotNull(_classUnderTest.World);
    }
    
    [Test]
    public void Specialized() {
        Assert.NotNull(_classUnderTest.Specialized);
    }
}