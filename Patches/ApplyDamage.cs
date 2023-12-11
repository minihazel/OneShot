using System;
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
    internal class ApplyDamage : ModulePatch
    {
        private static ActiveHealthController healthController;
        private static ValueStruct currentHeadHealth;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ActiveHealthController), "ApplyDamage");
        }

        private static bool Prefix(ActiveHealthController __instance, ref float damage, EBodyPart bodyPart, EDamageType damageInfo)
        {
            try
            {
                if (__instance.Player != null && __instance.Player.IsYourPlayer)
                {
                    healthController = __instance.Player.ActiveHealthController;
                    currentHeadHealth = healthController.GetBodyPartHealth(EBodyPart.Head, false);

                    if (bodyPart == EBodyPart.Head)
                    {
                        if (damageInfo == EDamageType.Bullet && damage > currentHeadHealth.Current)
                        {
                            OneShotPlugin.killPlayer = false;
                            damage = currentHeadHealth.Current / 2;
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            OneShotPlugin.killPlayer = true;
            return true;
        }
    }
}
