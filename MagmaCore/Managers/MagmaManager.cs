using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaCore.Managera
{
    public class MagmaManager : MonoBehaviour
    {
        public static MagmaManager main;

        public void Awake()
        {
            main = this;
        }

        public void OnDestroy()
        {
            if (main == this)
            {
                main = null;
            }
        }

        public IEnumerator FixBagCoroutine()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();

            ModularBackpack.SetAllBackpackSprites();
        }
    }
}
