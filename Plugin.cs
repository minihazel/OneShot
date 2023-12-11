using Aki.Reflection.Patching;
using BepInEx;
using BepInEx.Configuration;
using EFT;
using System;
using System.Diagnostics;
using System.Reflection;

namespace OneShot
{
    [BepInPlugin("com.hazelify.oneshot", "OneShot Protection", "1.0.0")]
    public class OneShotPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<Boolean> enableMod
        {
            get; set;
        }

        public static bool killPlayer = true;

        private void Awake()
        {
            enableMod = Config.Bind("1. OneShot", "Enable protection", false,
                new ConfigDescription("When enabled, an insta-kill headshot will not kill you.",
                null,
                    new ConfigurationManagerAttributes { IsAdvanced = false, Order = 1 }));

            new NewGamePatch().Enable();
            new OneShot.Patches.Kill().Enable();
            new OneShot.Patches.ApplyDamage().Enable();
        }
        internal class NewGamePatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));

            [PatchPrefix]
            private static void PatchPrefix()
            {
            }
        }
    }
}
