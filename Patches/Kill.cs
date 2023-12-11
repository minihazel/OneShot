using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Aki.Reflection.Patching;
using OneShot;
using EFT;
using EFT.HealthSystem;
using EFT.UI;
using HarmonyLib;

namespace OneShot.Patches
{
    internal class Kill : ModulePatch
    {
        private static ActiveHealthController hc;
        private static ValueStruct currentHeadHealth;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ActiveHealthController), "Kill");
        }

        public static bool Prefix(ActiveHealthController __instance, EDamageType damageType)
        {
            if (__instance.Player != null && __instance.Player.IsYourPlayer)
            {
                hc = __instance.Player.ActiveHealthController;
                currentHeadHealth = hc.GetBodyPartHealth(EBodyPart.Head, false);

                if (!OneShotPlugin.killPlayer)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
