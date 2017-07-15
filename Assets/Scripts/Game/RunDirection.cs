﻿public enum RunDirection
{
    North,
    East,
    South,
    West
}

class RunDirectionHelper
{
    private static BiDictionary<RunDirection, float> _directionMapping;

    public RunDirectionHelper()
    {
        _directionMapping = new BiDictionary<RunDirection, float>();
        _directionMapping.Add(RunDirection.North, 0);
        _directionMapping.Add(RunDirection.East, 90);
        _directionMapping.Add(RunDirection.South, 180);
        _directionMapping.Add(RunDirection.West, 270);
    }

    public static float ToRotation(RunDirection direction)
    {
        float rotation;
        _directionMapping.TryGetByFirst(direction, out rotation);
        return rotation;
    }
    
    public static RunDirection ToDirection(float rotation)
    {
        RunDirection direction;
        _directionMapping.TryGetBySecond(rotation, out direction);
        return direction;
    }
}