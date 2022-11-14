using InformantTest.Implementation;
using NUnit.Framework;
using StardewTests.Harness.Test;

namespace InformantTest; 

[TestFixture]
public class I18NTest {

    [Test]
    [TestCase(LocalizedContentManager.LanguageCode.de)] 
    [TestCase(LocalizedContentManager.LanguageCode.ko)] 
    [TestCase(LocalizedContentManager.LanguageCode.tr)] 
    [TestCase(LocalizedContentManager.LanguageCode.zh)] 
    public void ValidateLocales(LocalizedContentManager.LanguageCode locale) {
        I18NTestUtil.Assert18NCorrect(TestUtils.ModFolder, locale);
    }
}