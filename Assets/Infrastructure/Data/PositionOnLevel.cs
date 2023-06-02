using System;

namespace Infrastructure.Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public string Level;
        public VectorData Position;

        public PositionOnLevel(string level, VectorData position)
        {
            Level = level;
            Position = position;
        }
    }
}