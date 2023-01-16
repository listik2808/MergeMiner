using UnityEngine;

namespace Source.Scripts.SaveSystem
{
    public static class DataExtensions
    {
        public static bool ToBool(this int obj) =>
            obj != 0;

        public static int ToInt(this bool obj) =>
            obj ? 1 : 0;
        
        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);

        public static Vector3Data AsVectorData(this Vector3 vector) =>
            new Vector3Data(vector.x, vector.y, vector.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static QuaternionData AsQuaternionData(this Quaternion quaternion) =>
            new QuaternionData(quaternion.x, quaternion.y, quaternion.z, quaternion.w);

        public static Quaternion AsUnityQuaternion(this QuaternionData quaternionData) =>
            new Quaternion(quaternionData.X, quaternionData.Y, quaternionData.Z, quaternionData.W);
    }
}