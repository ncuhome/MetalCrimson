using UnityEngine;

namespace ER
{
    //Unity的一些扩展方法
    public static class MethodExtensionForUnity
    {
        /// <summary>
        /// Gets or add a component. Usage example:
        /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this Component child, bool set_enable = false) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }
            var bcomp = result as Behaviour;
            if (set_enable)
            {
                if (bcomp != null) bcomp.enabled = true;
            }
            return result;
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T result = go.transform.GetComponent<T>();
            if (result == null)
            {
                result = go.AddComponent<T>();
            }
            var bcomp = result as Behaviour;
            if (bcomp != null) bcomp.enabled = true;
            return result;
        }

        public static void Walk(this GameObject o, System.Action<GameObject> f)
        {
            f(o);

            int numChildren = o.transform.childCount;

            for (int i = 0; i < numChildren; ++i)
            {
                Walk(o.transform.GetChild(i).gameObject, f);
            }
        }
    }
}