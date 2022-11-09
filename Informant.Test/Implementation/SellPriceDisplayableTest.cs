using System;
using NUnit.Framework;
using Slothsoft.Informant.Implementation;
using StardewTests.Harness;

namespace InformantTest.Implementation; 

[TestFixture]
public class SellPriceDisplayableTest {
    
    private static readonly IModHelper ModHelper = new TestModHelper(TestUtils.ModFolder);
    private SellPriceDisplayable _classUnderTest = CreateClassUnderTest();

    private static SellPriceDisplayable CreateClassUnderTest() {
        return new SellPriceDisplayable(ModHelper, Guid.NewGuid().ToString());
    }

    [SetUp]
    public void SetUp() {
        _classUnderTest = CreateClassUnderTest();
    }

    [Test]
    public void Id() {
        Assert.NotNull(_classUnderTest.Id);
    }
    
    [Test]
    public void DisplayName() {
        Assert.NotNull(_classUnderTest.DisplayName);
    }
    
    [Test]
    public void Description() {
        Assert.NotNull(_classUnderTest.Description);
    }
}