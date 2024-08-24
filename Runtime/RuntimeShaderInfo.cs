namespace io.github.noridev
{
    public static class RuntimeShaderInfo
    {
        public delegate void FromBitMaskDelegate(ref TargetShaders targetShaders, int bitMask);

        public static FromBitMaskDelegate FromBitMask;
    }
}
