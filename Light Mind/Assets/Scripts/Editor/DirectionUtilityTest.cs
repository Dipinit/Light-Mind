using System;
using Assets.Scripts.Utilities;
using NUnit.Framework;
using UnityEngine;

namespace Editor
{
    public class DirectionUtilityTest
    {
        [Test]
        public void DirectionUtilityReturnsValidDirectionAsVector3Passes()
        {
            Assert.AreEqual(new Vector3(1, 0, 0), DirectionUtility.GetDirectionAsVector3(Direction.East));
            Assert.AreEqual(new Vector3(1, 1, 0), DirectionUtility.GetDirectionAsVector3(Direction.NorthEast));
            Assert.AreEqual(new Vector3(0, 1, 0), DirectionUtility.GetDirectionAsVector3(Direction.North));
            Assert.AreEqual(new Vector3(-1, 1, 0), DirectionUtility.GetDirectionAsVector3(Direction.NorthWest));
            Assert.AreEqual(new Vector3(-1, 0, 0), DirectionUtility.GetDirectionAsVector3(Direction.West));
            Assert.AreEqual(new Vector3(-1, -1, 0), DirectionUtility.GetDirectionAsVector3(Direction.SouthWest));
            Assert.AreEqual(new Vector3(0, -1, 0), DirectionUtility.GetDirectionAsVector3(Direction.South));
            Assert.AreEqual(new Vector3(1, -1, 0), DirectionUtility.GetDirectionAsVector3(Direction.SouthEast));
        }

        [Test]
        public void DirectionUtilityReturnsValidDirectionFromVector3Passes()
        {
            Assert.AreEqual(Direction.East, DirectionUtility.ToDirection(new Vector3(1, 0, 0)));
            Assert.AreEqual(Direction.NorthEast, DirectionUtility.ToDirection(new Vector3(1, 1, 0)));
            Assert.AreEqual(Direction.North, DirectionUtility.ToDirection(new Vector3(0, 1, 0)));
            Assert.AreEqual(Direction.NorthWest, DirectionUtility.ToDirection(new Vector3(-1, 1, 0)));
            Assert.AreEqual(Direction.West, DirectionUtility.ToDirection(new Vector3(-1, 0, 0)));
            Assert.AreEqual(Direction.SouthWest, DirectionUtility.ToDirection(new Vector3(-1, -1, 0)));
            Assert.AreEqual(Direction.South, DirectionUtility.ToDirection(new Vector3(0, -1, 0)));
            Assert.AreEqual(Direction.SouthEast, DirectionUtility.ToDirection(new Vector3(1, -1, 0)));
        }

        [Test]
        public void DirectionUtilityThrowsExceptionIfInvalidVector3Supplied()
        {
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(0, 0, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(2, 0, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(0, 2, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(-2, 0, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(0, -2, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(2, 2, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(-2, -2, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(2, -2, 0)));
            Assert.Throws<ArgumentException>(() => DirectionUtility.ToDirection(new Vector3(-2, 2, 0)));
        }
    }
}