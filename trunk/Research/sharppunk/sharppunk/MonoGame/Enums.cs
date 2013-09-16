using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sharppunk
{
    public enum PowerLineStatus
    {
        Offline,
        Online,
        Unknown
    }

    [Flags]
    public enum BatteryChargeStatus
    {
        Charging = 8,
        Critical = 4,
        High = 1,
        Low = 2,
        NoSystemBattery = 0x80,
        Unknown = 0xff
    }

    public enum PlayerIndex
    {
        One,
        Two,
        Three,
        Four
    }

    public enum CurveLoopType
    {
        Constant,
        Cycle,
        CycleOffset,
        Oscillate,
        Linear
    }

    public enum CurveContinuity
    {
        Smooth,
        Step
    }

    public enum CurveTangent
    {
        Flat,
        Linear,
        Smooth
    }

    public enum TargetPlatform
    {
        Unknown,
        Windows,
        Xbox360,
        Zune
    }

    public enum PlaneIntersectionType
    {
        Front,
        Back,
        Intersecting
    }

    public enum ContainmentType
    {
        Disjoint,
        Contains,
        Intersects
    }
}
