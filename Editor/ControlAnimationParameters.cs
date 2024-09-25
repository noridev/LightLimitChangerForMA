﻿using System;

namespace moe.noridev
{
    public readonly struct ControlAnimationParameters
    {
        public readonly string TargetPath;
        public readonly Type TargetType;

        public readonly float MinLightValue;
        public readonly float MaxLightValue;
        public readonly float DefaultMinLightValue;
        public readonly float DefaultMaxLightValue;
        public readonly float DefaultMonochromeLightingValue;
        public readonly float DefaultMonochromeAdditiveLightingValue;
        public readonly float DefaultShadowEnvStrengthValue;

        public ControlAnimationParameters(string targetPath, Type targetType, float minLightValue, float maxLightValue, float defaultMinLightValue, float defaultMaxLightValue, float defaultMonochromeLightingValue, float defaultMonochromeAdditiveLightingValue, float defaultShadowEnvStrengthValue)
        {
            TargetPath = targetPath;
            TargetType = targetType;
            MinLightValue = minLightValue;
            MaxLightValue = maxLightValue;
            DefaultMinLightValue = defaultMinLightValue;
            DefaultMaxLightValue = defaultMaxLightValue;
            DefaultMonochromeLightingValue = defaultMonochromeLightingValue;
            DefaultMonochromeAdditiveLightingValue = defaultMonochromeAdditiveLightingValue;
            DefaultShadowEnvStrengthValue = defaultShadowEnvStrengthValue;
        }
    }
}
