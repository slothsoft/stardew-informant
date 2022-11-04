using System;
using NUnit.Framework;

namespace StardewTests.Harness; 

[TestFixture]
public class TestModHelperTest {

    private record TestConfig {
        public string? Value { get; init; }
    }
    
    private record OtherConfig {
        public string? Value { get; init; }
    }
    
    private TestModHelper _classUnderTest = new();

    [SetUp]
    public void SetUp() {
        _classUnderTest = new();
    }

    [Test]
    public void ClearConfig() {
        _classUnderTest.WriteConfig(new TestConfig {
            Value = Guid.NewGuid().ToString(),
        });
        _classUnderTest.ClearConfig<TestConfig>();

        ReadConfig_Null();
    }
    
    [Test]
    public void ReadConfig_Null() {
        var config = _classUnderTest.ReadConfig<TestConfig>();
        Assert.NotNull(config);
        Assert.IsNull(config.Value);
    }
    
    [Test]
    public void ReadConfig_Multiple() {
        var testValue = Guid.NewGuid().ToString();
        _classUnderTest.WriteConfig(new TestConfig {
            Value = testValue
        });
        var otherValue = Guid.NewGuid().ToString();
        _classUnderTest.WriteConfig(new OtherConfig {
            Value = otherValue
        });
        
        var testConfig = _classUnderTest.ReadConfig<TestConfig>();
        Assert.NotNull(testConfig);
        Assert.AreEqual(testValue, testConfig.Value);
        
        var otherConfig = _classUnderTest.ReadConfig<OtherConfig>();
        Assert.NotNull(otherConfig);
        Assert.AreEqual(otherValue, otherConfig.Value);
    }
    
    [Test]
    public void WriteConfig() {
        var value = Guid.NewGuid().ToString();
        _classUnderTest.WriteConfig(new TestConfig {
            Value = value
        });
        
        var config = _classUnderTest.ReadConfig<TestConfig>();
        Assert.NotNull(config);
        Assert.AreEqual(value, config.Value);
    }
}