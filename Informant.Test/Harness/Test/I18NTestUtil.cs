using System.Linq;
using NUnit.Framework;

namespace StardewTests.Harness.Test; 

public static class I18NTestUtil {

    public static void Assert18NCorrect(string modFolder, LocalizedContentManager.LanguageCode locale) {
        var translationHelper = new TestTranslationHelper(modFolder);
        var allDefaultTranslations = translationHelper.GetTranslations().ToArray();
        
        // now set to tested locale and assert
        translationHelper.LocaleEnum = locale;
        foreach (var defaultTranslation in allDefaultTranslations) {
            // this throws an exception if the key does not exist at all
            var translation = translationHelper.Get(defaultTranslation.Key).ToString();
            // warn if the key exists, but is English (since I can't translate those keys myself)
            Warn.If(translation.Equals(defaultTranslation.ToString()), () => $"{defaultTranslation.Key} was not translated in locale {locale}");
        }
    }
}