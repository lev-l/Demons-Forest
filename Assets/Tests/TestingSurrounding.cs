using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestingSurrounding
{
    private float basicAngle = 180;

    private float[] GetSurroundingAngles(int numberOfAngles)
    {
        Surround functions = new Surround();

        float[] results = functions.FindAngles(basicAngle, numberOfAngles);

        return results;
    }

    [Test]
    public void TestingSurroundingBasicResult()
    {
        Assert.AreEqual(basicAngle, GetSurroundingAngles(1)[0]);
    }

    [Test]
    public void TestingSurroundingTwoAngles()
    {
        Assert.AreEqual(new float[] { basicAngle, basicAngle + 180 },
                        GetSurroundingAngles(2));
    }

    [Test]
    public void TestingSurroundingFiveAngles()
    {
        // just wanted to be sure, after all I'm testin for it
        float[] expected = new float[5];
        expected[0] = basicAngle;
        expected[1] = basicAngle + 72;
        expected[2] = basicAngle + 72 * 2;
        expected[3] = basicAngle + 72 * 3;
        expected[4] = basicAngle + 72 * 4;

        Assert.AreEqual(expected, GetSurroundingAngles(5));
    }
}
