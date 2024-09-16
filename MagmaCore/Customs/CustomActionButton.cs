using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagmaCore.Customs
{
    public abstract class CustomActionButton : CustomBase
    {
        public Button ButtonInstance;
        /// <value>
        /// Use this in characters to have a custom button. Assigned automatically using casted ID.
        /// </value>
        public virtual ActionButtonManager.Type ButtonType { get; private set; }
        public virtual string buttonText { get; private set; }
        public virtual string hoverText { get; private set; }

        public override void Convert()
        {
            if (ButtonType == default)
            {
                ButtonType = (ActionButtonManager.Type)GetHash();
            }
        }
        public virtual void OnClick() { }
    }
}
