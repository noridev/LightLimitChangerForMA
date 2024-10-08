﻿using System;

namespace moe.noridev
{
    [Flags]
    public enum LightLimitControlType : long
    {
        Light = LightMin | LightMax, // 1 << 0,
        Saturation = 1 << 1,
        Unlit = 1 << 2,
        ColorTemperature = 1 << 3,
        LightMin = 1 << 4,
        LightMax = 1 << 5,
        Monochrome = 1 << 6,
        MonochromeAdditive = 1 << 7,
        Emission = 1 << 8,
        ShadowEnvStrength = 1 << 9,

        AdditionalControls = Saturation | Unlit | ColorTemperature | Monochrome | MonochromeAdditive | Emission | ShadowEnvStrength,
    }
}
