﻿using System;
using UnityEngine;

namespace io.github.azukimochi
{
    partial class ShaderInfo
    {
        public sealed class Poiyomi : ShaderInfo
        {
            public static Poiyomi Instance { get; } = new Poiyomi();

            public const string _LightingMinLightBrightness = "_LightingMinLightBrightness";
            public const string _LightingCap = "_LightingCap";
            public const string _MainColorAdjustToggle = "_MainColorAdjustToggle";
            public const string _Saturation = "_Saturation";
            public const string _Color = "_Color";
            public const string _MainTex = "_MainTex";

            public const string _EnableDissolve = "_EnableDissolve";
            public const string _DissolveTextureColor = "_DissolveTextureColor";
            public const string _DissolveToTexture = "_DissolveToTexture";

            private static class PropertyIDs
            {
                public static readonly int LightingMinLightBrightness = Shader.PropertyToID(_LightingMinLightBrightness);
                public static readonly int LightingCap = Shader.PropertyToID(_LightingCap);
                public static readonly int MainColorAdjustToggle = Shader.PropertyToID(_MainColorAdjustToggle);
                public static readonly int Saturation = Shader.PropertyToID(_Saturation);
                public static readonly int Color = Shader.PropertyToID(_Color);
                public static readonly int MainTex = Shader.PropertyToID(_MainTex);
                public static readonly int EnableDissolve = Shader.PropertyToID(_EnableDissolve);
                public static readonly int DissolveTextureColor = Shader.PropertyToID(_DissolveTextureColor);
                public static readonly int DissolveToTexture = Shader.PropertyToID(_DissolveToTexture);
            }

            private static class DefaultParameters
            {
                public static readonly float Saturation = 0;
                public static readonly Color Color = Color.white;
            }

            private const string Animated_Suffix = "Animated";
            private const string Flag_IsAnimated = "1";

            public override bool TryNormalizeMaterial(Material material, LightLimitChangerObjectCache cache)
            {
                var textureBaker = TextureBaker.GetInstance<PoiyomiTextureBaker>();
                bool result = false;

                {
                    bool bakeFlag = false;
                    var tex = material.GetTexture(PropertyIDs.MainTex);
                    if (tex != null)
                        textureBaker.Texture = tex;

                    if (material.GetColor(PropertyIDs.Color) != DefaultParameters.Color)
                    {
                        textureBaker.Color = material.GetColor(PropertyIDs.Color);
                        material.SetColor(PropertyIDs.Color, DefaultParameters.Color);
                        bakeFlag = true;
                    }

                    if (material.GetFloat(PropertyIDs.Saturation) != DefaultParameters.Saturation)
                    {
                        textureBaker.HSVG = new Vector4(0, material.GetFloat(PropertyIDs.Saturation), 1, 1);
                        material.SetFloat(PropertyIDs.Saturation, DefaultParameters.Saturation);
                        bakeFlag = true;
                    }

                    if (bakeFlag)
                    {
                        material.SetTexture(PropertyIDs.MainTex, cache.Register(textureBaker.Bake()));
                    }

                    result |= bakeFlag;
                }

                if (material.GetFloat(PropertyIDs.EnableDissolve) != 0)
                {
                    textureBaker.Reset();

                    bool bakeFlag = false;
                    var tex = material.GetTexture(PropertyIDs.DissolveToTexture);
                    if (tex != null)
                        textureBaker.Texture = tex;

                    if (material.GetColor(PropertyIDs.DissolveTextureColor) != DefaultParameters.Color)
                    {
                        textureBaker.Color = material.GetColor(PropertyIDs.DissolveTextureColor);
                        material.SetColor(PropertyIDs.DissolveTextureColor, DefaultParameters.Color);
                        bakeFlag = true;
                    }

                    if (bakeFlag)
                    {
                        material.SetTexture(PropertyIDs.DissolveToTexture, cache.Register(textureBaker.Bake()));
                    }

                    result |= bakeFlag;
                }


                return result;
            }

            public override bool IsTargetShader(Shader shader)
            {
                //return shader.name.Contains("poiyomi", StringComparison.OrdinalIgnoreCase);
                return shader.name.Contains("Poiyomi 8", StringComparison.OrdinalIgnoreCase);
            }

            public override void SetControlAnimation(in ControlAnimationContainer container, in ControlAnimationParameters parameters)
            {
                switch (container.ControlType)
                {
                    case LightLimitControlType.Light:

                        container.Default.SetParameterAnimation(parameters, _LightingMinLightBrightness, parameters.MinLightValue);
                        container.Default.SetParameterAnimation(parameters, _LightingCap, parameters.MaxLightValue);

                        container.Control.SetParameterAnimation(parameters, _LightingMinLightBrightness, parameters.MinLightValue, parameters.MaxLightValue);
                        container.Control.SetParameterAnimation(parameters, _LightingCap, parameters.MinLightValue, parameters.MaxLightValue);

                        break;

                    case LightLimitControlType.Saturation:

                        container.Default.SetParameterAnimation(parameters, _Saturation, DefaultParameters.Saturation);
                        container.Control.SetParameterAnimation(parameters, _Saturation, -1, 1);

                        break;

                    case LightLimitControlType.ColorTemperature:

                        container.Default.SetParameterAnimation(parameters, _Color, DefaultParameters.Color);
                        container.Control.SetColorTempertureAnimation(parameters, _Color, DefaultParameters.Color);

                        container.Default.SetParameterAnimation(parameters, _DissolveTextureColor, DefaultParameters.Color);
                        container.Control.SetColorTempertureAnimation(parameters, _DissolveTextureColor, DefaultParameters.Color);

                        break;
                }
            }

            public override void AdditionalControl(Material material, in LightLimitChangerParameters parameters)
            {
                if (parameters.AllowSaturationControl || parameters.AllowColorTempControl)
                {
                    material.SetFloat(_MainColorAdjustToggle, 1);
                    material.EnableKeyword($"{_MainColorAdjustToggle.ToUpperInvariant()}_ON");
                    material.EnableKeyword("COLOR_GRADING_HDR");

                    material.SetOverrideTag($"{_LightingCap}{Animated_Suffix}", Flag_IsAnimated);
                    material.SetOverrideTag($"{_LightingMinLightBrightness}{Animated_Suffix}", Flag_IsAnimated);

                    if (parameters.AllowColorTempControl)
                    {
                        material.SetOverrideTag($"{_Color}{Animated_Suffix}", Flag_IsAnimated);

                        if (material.GetFloat(PropertyIDs.EnableDissolve) != 0)
                            material.SetOverrideTag($"{_DissolveTextureColor}{Animated_Suffix}", Flag_IsAnimated);
                    }
                    if (parameters.AllowSaturationControl)
                    {
                        material.SetOverrideTag($"{_Saturation}{Animated_Suffix}", Flag_IsAnimated);
                    }
                }
            }

            public static void EnableColorAdjust(Material material)
            {
                material.SetFloat(_MainColorAdjustToggle, 1);
                material.EnableKeyword($"{_MainColorAdjustToggle.ToUpperInvariant()}_ON");
                material.EnableKeyword("COLOR_GRADING_HDR");
            }

            public static void SetAnimatedFlags(Material material, bool allowColorTempControl, bool allowSaturationControl)
            {
                material.SetOverrideTag($"{_LightingCap}{Animated_Suffix}", Flag_IsAnimated);
                material.SetOverrideTag($"{_LightingMinLightBrightness}{Animated_Suffix}", Flag_IsAnimated);

                if (allowColorTempControl )
                {
                    material.SetOverrideTag($"{_Color}{Animated_Suffix}", Flag_IsAnimated);
                }
                if (allowSaturationControl)
                {
                    material.SetOverrideTag($"{_Saturation}{Animated_Suffix}", Flag_IsAnimated);
                }
            }
        }
    }
}
