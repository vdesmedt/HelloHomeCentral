using System;

namespace HelloHome.Central.Domain.Entities
{
    [Flags]
    public enum NodeFeature : short
    {
        Si7021 =         1 << 0,
        Bmp =            1 << 1,
        VInMeasure =     1 << 2,
        Hal1 =           1 << 3,
        Hal2 =           1 << 4,
        Dry1 =           1 << 5,
    }
}