using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
