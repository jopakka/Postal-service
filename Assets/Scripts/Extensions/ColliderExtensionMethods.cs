using UnityEngine;

namespace Extensions
{
    public static class ColliderExtensionMethods
    {
        public static bool TryGetComponentIfIsTag<T>(this Collider other, string tag, out T component) where T : class
        {
            if (other.CompareTag(tag))
            {
                return other.TryGetComponent(out component);
            }

            component = null;
            return false;
        }
    }
}
