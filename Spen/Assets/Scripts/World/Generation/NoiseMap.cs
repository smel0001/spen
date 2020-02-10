using UnityEngine;

public static class NoiseMap
{
    [System.Serializable]
    public struct Wave
    {
        public float seed;
        public float frequency;
        public float amplitude;

        public Wave(float _seed, float _frequency, float _amplitude)
        {
            this.seed = _seed;
            this.frequency = _frequency;
            this.amplitude = _amplitude;
        }
    }


    public static float[,] GeneratePerlinNoiseMap(int mapWidth, int mapHeight, float scale, float offsetX, float offsetY, Wave[] waves)
    {
        // create an empty noise map with the mapDepth and mapWidth coordinates
        float[,] noiseMap = new float[mapWidth, mapHeight];

        for (int yIndex = 0; yIndex < mapHeight; yIndex++)
        {
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                // calculate sample indices based on the coordinates, the scale and the offset
                float sampleX = (xIndex + offsetX) / scale;
                float sampleY = (yIndex + offsetY) / scale;

                float noise = 0f;
                float normalization = 0f;
                foreach (Wave wave in waves)
                {
                    // generate noise value using PerlinNoise for a given Wave
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }
                // normalize the noise value so that it is within 0 and 1
                noise /= normalization;

                noiseMap[xIndex, yIndex] = noise;
            }
        }

        return noiseMap;
    }

    //Used for static noise like heat maps
    public static float[,] GenerateUniformNoiseMap(int mapWidth, int mapHeight, float centerY, int maxDistanceY, int offsetY)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        for (int yIndex = 0; yIndex < mapHeight; yIndex++)
        {
            float sampleY = yIndex + offsetY;
            float noise = 1 - (Mathf.Abs(sampleY - centerY) / maxDistanceY);
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                noiseMap[xIndex, yIndex] = noise;
            }
        }
        return noiseMap;
    }

    public static float[,] GenerateCutoffPerlinNoiseMap(int mapWidth, int mapHeight, float scale, float offsetX, float offsetY, Wave[] waves, float threshold)
    {
        float[,] noiseMap = GeneratePerlinNoiseMap(mapWidth, mapHeight, scale, offsetX, offsetY, waves);

        for (int xIndex = 0; xIndex < mapWidth; xIndex++)
        {
            for (int yIndex = 0; yIndex < mapHeight; yIndex++)
            {
                if (noiseMap[xIndex, yIndex] < threshold)
                {
                    noiseMap[xIndex, yIndex] = 0f;
                }
            }
        }

        return noiseMap;
    }
}