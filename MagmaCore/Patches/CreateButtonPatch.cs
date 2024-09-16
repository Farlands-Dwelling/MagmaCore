using HarmonyLib;
using MagmaCore.Customs;
using MagmaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagmaCore.Patches
{
    [HarmonyPatch(typeof(ActionButtonManager), "CreateButton")]
    class CreateButtonPatch
    {
        static void Postfix (ref ActionButtonManager __instance, ref GameObject __result, ActionButtonManager.Type type)
        {
            foreach (CustomActionButton actionButton in CustomUtils.GetCustomsOfType<CustomActionButton>())//CustomCharacter.CustomCharacters.Values)
            {
                if (type == actionButton.ButtonType)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.actionButtonPrefab, Vector3.zero, Quaternion.identity, __instance.combatButtonsParent);
                    TranslationUtils.GetOrCreateTranslation("english", actionButton.ID.ToString(), actionButton.buttonText);
                    TranslationUtils.GetOrCreateTranslation("english", actionButton.ID.ToString() + "hover", actionButton.hoverText);
                    gameObject.GetComponent<ReplacementText>().key = actionButton.ID.ToString();
                    gameObject.GetComponentInChildren<SimpleHoverText>().SetText(actionButton.ID.ToString() + "hover");
                    gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(actionButton.OnClick));
                    __instance.numberOfCombatButtons++;

                    __result = gameObject;
                    break;
                }
            }
        }
    }
}
