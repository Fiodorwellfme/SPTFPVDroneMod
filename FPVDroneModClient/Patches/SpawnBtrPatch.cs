#if !UNITY_EDITOR
using System.Reflection;
using FPVDroneModClient.Config;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace FPVDroneModClient.Patches
{
    public class SpawnBtrPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(EFT.Vehicle.BtrController), nameof(EFT.Vehicle.BtrController.LoadBTRVehicle));
        }

        [PatchPrefix]
        private static bool PatchPrefix()
        {
            // if tank is dead and perma death is enabled dont spawn the tank
            if (Plugin.TankDeathState.IsDead &&
                GeneralConfig.EnableTankPermaDeath.Value)
            {
                return false;
            }

            return true;
        }
    }
}
#endif