using System;
using NUnit.Framework;
using Slothsoft.Informant.Implementation;
using StardewTests.Harness;

namespace InformantTest.Implementation; 

[TestFixture]
public class NewRecipeDisplayableTest {
    
    private static readonly IModHelper ModHelper = new TestModHelper(TestUtils.ModFolder);
    private NewRecipeDisplayable _classUnderTest = CreateClassUnderTest();

    private static NewRecipeDisplayable CreateClassUnderTest() {
        return new NewRecipeDisplayable(ModHelper, Guid.NewGuid().ToString());
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