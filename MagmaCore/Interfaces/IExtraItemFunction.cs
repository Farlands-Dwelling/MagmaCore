using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Interfaces
{
    public interface IExtraItemFunction
    {
        /// <param name="sender">The original method patched to invoke this function. Usually a class ending with "Manager".</param>
        /// <returns>The child GameObject, if found. Otherwise null.</returns>
        void Event(MonoBehaviour sender, Enum enumType);
    }
}
