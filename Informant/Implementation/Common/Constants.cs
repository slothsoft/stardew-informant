// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
namespace Slothsoft.Informant.Implementation.Common;

/// <summary>
/// Contains constants for all the seasons.
/// </summary>
internal static class Seasons {
    public const string Spring = "spring";
    public const string Summer = "summer";
    public const string Fall = "fall";
    public const string Winter = "winter";
    public static readonly string[] All = { Spring, Summer, Fall, Winter };
    
    public const int LengthInDays = 28;
}
