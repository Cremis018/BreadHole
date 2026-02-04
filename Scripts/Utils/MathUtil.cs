using System;

public class MathUtil
{
    public static float ShortestRotateTarget(float from, float to)
    {
        if (Math.Abs(to - from) > float.Pi)
            return to + Math.Sign(from - to) * float.Tau;
        return to;
    }
}