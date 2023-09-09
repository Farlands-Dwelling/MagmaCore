using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Utils
{
    public static class GameObjectUtils
    {
        /* COPIED FROM https://github.com/KitchenMods/KitchenLib/blob/0e4660f10d9b864b279f2441b5c3ed1ed57ae50c/KitchenLib/src/Utils/GameObjectUtils.cs#L90 */

        /// <summary>
        /// Clones a component from one GameObject to another
        /// </summary>
        /// <param name="original">The original component to copy.</param>
        /// <param name="destination">The GameObject to assign the clone to.</param>
        /// <returns>Cloned component</returns>
        public static Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        public static void CopyComponentValues<T>(T from, T to)
        {
            var json = JsonUtility.ToJson(from);
            JsonUtility.FromJsonOverwrite(json, to);
        }

        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static GameObject DuplicateOnto(this GameObject originalObject, GameObject objectToCopyOnto)
        {
            foreach (Component comp in originalObject.GetComponents<Component>()) 
            {
                objectToCopyOnto.AddComponent(comp.GetType()).GetCopyOf(comp);
            }

            foreach (Transform child in originalObject.transform)
            {
                Transform.Instantiate(child).SetParent(objectToCopyOnto.transform);
            }

            objectToCopyOnto.tag = originalObject.tag;
            objectToCopyOnto.layer = originalObject.layer;

            return objectToCopyOnto;
        }
    }
}
