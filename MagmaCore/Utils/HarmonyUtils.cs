using MagmaCore.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaCore.Utils
{
    public static class HarmonyUtils
    {
        public class PatchedEnumerator : IEnumerable
        {
            public IEnumerator enumerator;
            public Action postfixAction;

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public IEnumerator GetEnumerator()
            {
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    yield return item;
                }
                postfixAction();
            }
        }

        /*static void Postfix(ref IEnumerator __result)
        {
            Action postfixAction = () => { };
            var patchedEnumerator = new PatchedEnumerator()
            {
                enumerator = __result,
                postfixAction = postfixAction,
            };
            __result = patchedEnumerator.GetEnumerator();
        }*/
    }
}
