using System;

namespace Infrastructure.Data
{
    [Serializable]
    public class VectorData
    {
        public float x;
        public float y;
        public float z;

        public VectorData(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}