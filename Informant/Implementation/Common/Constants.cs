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

/// <summary>
/// These constants can be used to compare to <code>Object.ParentSheetIndex</code>.
/// See https://stardewcommunitywiki.com/Modding:Big_craftables_data
/// </summary>
public static class BigCraftableIds {
    public const int BeeHouse = 10;
    public const int Cask = 163;
    public const int CheesePress = 16;
    public const int Keg = 12;
    public const int Loom = 17;
    public const int MayonnaiseMachine = 24;
    public const int OilMaker = 19;
    public const int PreservesJar = 15;
    public const int BoneMill = 90;
    public const int CharcoalKiln = 114;
    public const int Crystalarium = 21;
    public const int Furnace = 13;
    public const int GeodeCrusher =182;
    public const int HeavyTapper = 264;
    public const int LightningRod = 9;
    public const int OstrichIncubator = 254;
    public const int RecyclingMachine = 20;
    public const int SeedMaker = 25;
    public const int SlimeEggPress = 158;
    public const int SlimeIncubator = 156;
    public const int SlimeIncubator2 = 157; // game has both of these IDs?
    public const int SolarPanel = 231;
    public const int Tapper = 105;
    public const int WoodChipper = 211;
    public const int WormBin = 154;
    public const int Incubator = 101;
    public const int Incubator2 = 102; // the Wiki above shows Incubator as 101, but the game as 102?
    public const int Incubator3 = 103; // maybe it's the egg color?
    public const int CoffeeMaker = 246; 
    public const int Deconstructor = 265;

    public static readonly int[] AllMachines = {
        BeeHouse, Cask, CheesePress, Keg, Loom, MayonnaiseMachine, OilMaker, PreservesJar,
        BoneMill, CharcoalKiln, Crystalarium, Furnace, GeodeCrusher, HeavyTapper, LightningRod, OstrichIncubator, RecyclingMachine, SeedMaker, SlimeEggPress,
        SlimeIncubator, SlimeIncubator2, SolarPanel, Tapper, WoodChipper, WormBin, Incubator, Incubator2, Incubator3, CoffeeMaker, Deconstructor
    };

    public const int Chest = 130;
    public const int JunimoChest = 256;
    public const int MiniFridge = 216;
    public const int StoneChest = 232;
    public const int MiniShippingBin = 248;

    public static readonly int[] AllChests = {
        Chest, JunimoChest, MiniFridge, StoneChest, MiniShippingBin
    };
    
    public const int PinkBunny = 107;
}