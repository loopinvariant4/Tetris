namespace MagicAssembly.Core
{
    public class Consts
    {
        /// <summary>
        /// Number of pixels in a single tile side
        /// </summary>
        public const int TILELENGTH = 32;

        /// <summary>
        /// Number of tiles in a single chunk side. The total tiles would be a square of this number
        /// </summary>
        public const int CHUNKTILECOUNT = 32;

        /// <summary>
        /// Maximum number of chunks in a world. All worlds are squares 
        /// </summary>
        public const int MAXCHUNKCOUNT = 8192;

        /// <summary>
        /// Number of pixels in a single CHUNK SIDE
        /// </summary>
        public const int CHUNKLENGTH = CHUNKTILECOUNT * TILELENGTH;

        /// <summary>
        /// The current version of the world based on the world spec
        /// </summary>
        public const double WORLDVERSION = 1.0d;
    }
}
