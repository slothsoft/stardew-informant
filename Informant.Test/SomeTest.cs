using NUnit.Framework;

namespace InformantTest; 

[TestFixture]
public class SomeTest {

    [Test]
    public void Test() {
        IModHelper modHelper = null;
        Assert.IsNull(modHelper);
    }
}