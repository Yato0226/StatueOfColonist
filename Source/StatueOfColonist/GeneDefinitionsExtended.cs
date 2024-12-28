namespace StatueOfColonist
{
    public class GeneDef
    {
        public GeneGraphicData graphicData { get; set; } // Updated to the correct type
        public bool CanDrawNow() 
        {
            // Placeholder for actual implementation
            return true; 
        }
    }

    public class GeneGraphicData
    {
        public float drawScale { get; set; }
        public Vector3 drawOffset { get; set; }
        public float narrowCrownHorizontalOffset { get; set; }
        public bool drawOnEyes { get; set; }
    }

    public enum GeneDrawLayer
    {
        Base,
        Head,
        Body,
        Hair
    }

    public static class GeneNamespace
    {
        // This can contain related methods or properties if needed
    }
}
