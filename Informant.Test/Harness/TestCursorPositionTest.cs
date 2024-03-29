﻿using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace StardewTests.Harness; 

public class TestCursorPositionTest {
    
    private TestCursorPosition _classUnderTest = new();

    [SetUp]
    public void SetUp() {
        _classUnderTest = new();
    }

    [Test]
    public void GetScaledAbsolutePixels() {
        _classUnderTest.AbsolutePixels = new Vector2(1, 2);
        _classUnderTest.AbsoluteScale = 3;
        Assert.AreEqual(new Vector2(3, 6), _classUnderTest.GetScaledAbsolutePixels());
    }
    
    [Test]
    public void GetScaledScreenPixels() {
        _classUnderTest.ScreenPixels = new Vector2(3, 4);
        _classUnderTest.ScreenScale = 4;
        Assert.AreEqual(new Vector2(12, 16), _classUnderTest.GetScaledScreenPixels());
    }
}