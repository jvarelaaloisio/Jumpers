using UnityEngine;

namespace Core.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 XZ(this Vector3 original) => new Vector2(original.x, original.z);
    }
}