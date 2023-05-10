using UnityEngine;

namespace Infrastructure.Data
{
    public static class DataExtensions
    {
        public static VectorData ToVectorData(this Vector3 vector) => 
            new(vector.x, vector.y, vector.z);
        
        public static Vector3 AsUnityVector(this VectorData vector) => 
            new(vector.x, vector.y, vector.z);

        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);
    }
}