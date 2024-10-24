using ConMaster.Bedrock.Data.Types;

namespace ConMaster.Bedrock.Data.Biomes
{
    public struct BiomeWaterSettings
    {
        public float WaterColorR;
        public float WaterColorG;
        public float WaterColorB;
        public float WaterColorA;
        public float WaterTransparency;
    }
    public class BiomeType: Type<BiomeType>
    {
        public float Ash;
        public float BlueScores;
        public float Depth;
        public float DownFall;
        public float Height;
        public bool CanRainIn;
        public float RedScores;
        public string[] Tags = [];
        public float Temperature;
        public BiomeWaterSettings WaterSettings;
        public float WhiteAsh;
        public BiomeType(string id): base(id)
        {
            /*
             *   plains: {
                    ash: 0f,
                    blue_spores: 0f,
                    depth: 0.125f,
                    downfall: 0.4f,
                    height: 0.05f,
                    name_hash: "plains",
                    rain: 1b,
                    red_spores: 0f,
                    tags: [
                      "animal",
                      "bee_habitat",
                      "monster",
                      "overworld",
                      "plains",
                    ],
                    temperature: 0.8f,
                    waterColorA: 0.65f,
                    waterColorB: 0.9607843f,
                    waterColorG: 0.6862745f,
                    waterColorR: 0.26666668f,
                    waterTransparency: 0.65f,
                    white_ash: 0f,
                  },
            */
        }
    }
    public sealed class BiomeTypes: Types<BiomeType>;
}
