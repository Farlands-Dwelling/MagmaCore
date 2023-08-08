using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Utils
{
    public static class ItemUtils
    {
        public static Item2 FindItem(string itemName)
        {
            // not checked if works
            return Resources.Load<Item2>(itemName);
        }
    }
}
