namespace Slothsoft.Informant.Implementation.Common; 

internal static class GameInformation {
    
    internal static string GetObjectDisplayName(int parentSheetIndex) {
        Game1.objectInformation.TryGetValue(parentSheetIndex, out var str);
        return string.IsNullOrEmpty(str) ? "???" : str.Split('/')[4];
    }
}