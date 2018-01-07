using System;

namespace HelloHome.Central.Domain.Entities
{
    [Flags]
    public enum NodeFeature : byte
    {
        Si7021 =         1 << 0,
        Bmp =            1 << 1,
        VInMeasure =     1 << 2
    }
}