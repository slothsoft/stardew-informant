using System;
using System.Linq;
using NUnit.Framework;
using Slothsoft.Informant;
using Slothsoft.Informant.Implementation.Common;
using Slothsoft.Informant.Implementation.TooltipGenerator;
using StardewTests.Harness;

namespace InformantTest.Implementation.TooltipGenerator;

[TestFixture]
internal class MachineTooltipGeneratorTest {
    private const HideMachineTooltips AlwaysVisible = (HideMachineTooltips) (-1);
    private static readonly HideMachineTooltips[] HideMachineTooltipsValues = Enum.GetValues(typeof(HideMachineTooltips)).Cast<HideMachineTooltips>().ToArray();

    private MachineTooltipGenerator _classUnderTest = CreateClassUnderTest();

    private static MachineTooltipGenerator CreateClassUnderTest() {
        return new MachineTooltipGenerator(new TestModHelper(TestUtils.ModFolder));
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

    [Test]
    // these machines are NEVER hidden
    [TestCase(BigCraftableIds.BeeHouse, AlwaysVisible)]
    [TestCase(BigCraftableIds.Cask, AlwaysVisible)]
    [TestCase(BigCraftableIds.CheesePress, AlwaysVisible)]
    [TestCase(BigCraftableIds.Keg, AlwaysVisible)]
    [TestCase(BigCraftableIds.Loom, AlwaysVisible)]
    [TestCase(BigCraftableIds.MayonnaiseMachine, AlwaysVisible)]
    [TestCase(BigCraftableIds.OilMaker, AlwaysVisible)]
    [TestCase(BigCraftableIds.PreservesJar, AlwaysVisible)]
    [TestCase(BigCraftableIds.BoneMill, AlwaysVisible)]
    [TestCase(BigCraftableIds.CharcoalKiln, AlwaysVisible)]
    [TestCase(BigCraftableIds.Crystalarium, AlwaysVisible)]
    [TestCase(BigCraftableIds.Furnace, AlwaysVisible)]
    [TestCase(BigCraftableIds.GeodeCrusher, AlwaysVisible)]
    [TestCase(BigCraftableIds.HeavyTapper, AlwaysVisible)]
    [TestCase(BigCraftableIds.LightningRod, AlwaysVisible)]
    [TestCase(BigCraftableIds.OstrichIncubator, AlwaysVisible)]
    [TestCase(BigCraftableIds.RecyclingMachine, AlwaysVisible)]
    [TestCase(BigCraftableIds.SeedMaker, AlwaysVisible)]
    [TestCase(BigCraftableIds.SlimeEggPress, AlwaysVisible)]
    [TestCase(BigCraftableIds.SlimeIncubator, AlwaysVisible)]
    [TestCase(BigCraftableIds.SolarPanel, AlwaysVisible)]
    [TestCase(BigCraftableIds.Tapper, AlwaysVisible)]
    [TestCase(BigCraftableIds.WoodChipper, AlwaysVisible)]
    [TestCase(BigCraftableIds.WormBin, AlwaysVisible)]
    [TestCase(BigCraftableIds.Incubator, AlwaysVisible)]
    [TestCase(BigCraftableIds.CoffeeMaker, AlwaysVisible)]
    [TestCase(BigCraftableIds.Deconstructor, AlwaysVisible)]
    [TestCase(BigCraftableIds.StatueOfPerfection, AlwaysVisible)]
    [TestCase(BigCraftableIds.StatueOfTruePerfection, AlwaysVisible)]
    // chests are hidden if at least ForChests is selected
    [TestCase(BigCraftableIds.Chest, HideMachineTooltips.ForChests)]
    [TestCase(BigCraftableIds.JunimoChest, HideMachineTooltips.ForChests)]
    [TestCase(BigCraftableIds.MiniFridge, HideMachineTooltips.ForChests)]
    [TestCase(BigCraftableIds.StoneChest, HideMachineTooltips.ForChests)]
    [TestCase(BigCraftableIds.MiniShippingBin, HideMachineTooltips.ForChests)]
    // all other BigCraftables are hidden if at least ForNonMachines is selected
    [TestCase(BigCraftableIds.PlushBunny, HideMachineTooltips.ForNonMachines)]
    // all UNKNOWN BigCraftables should be displayed, because we have no idea what they are
    [TestCase(999999, AlwaysVisible)]
    public void HasTooltip(int parentSheetIndex, HideMachineTooltips firstHide) {
        var obj = new SObject {
            parentSheetIndex = {parentSheetIndex},
            bigCraftable = {true}
        };
        // until the first hide the tooltips should be hidden
        for (var hideTooltips = 0; hideTooltips <= (int) firstHide; hideTooltips++) {
            var hideTooltipsEnum = (HideMachineTooltips) hideTooltips;
            Assert.IsFalse(MachineTooltipGenerator.HasTooltip(obj, hideTooltipsEnum), $"Should be hidden when {hideTooltipsEnum} is selected!");
        }

        // after the first hide the tooltips should be visible
        for (var hideTooltips = 1 + (int) firstHide; hideTooltips < HideMachineTooltipsValues.Length; hideTooltips++) {
            var hideTooltipsEnum = (HideMachineTooltips) hideTooltips;
            Assert.IsTrue(MachineTooltipGenerator.HasTooltip(obj, hideTooltipsEnum), $"Should be visible when {hideTooltipsEnum} is selected!");
        }
    }

    [Test]
    [TestCase(-1, "Cannot be unloaded")] // only for slime incubators?
    [TestCase(0, "Finished")]
    [TestCase(10, "00:00:10")]
    [TestCase(100, "00:01:40")]
    [TestCase(10_000, "06:22:40")]
    public void CalculateMinutesLeftString(int minutesLeft, string expectedString) {
        var obj = new SObject {
            minutesUntilReady = { minutesLeft }
        };
        Assert.AreEqual(expectedString, _classUnderTest.CalculateMinutesLeftString(obj));
    }
}