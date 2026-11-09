using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Common.Models.Logging;
using System.Reflection;
using FPVDroneModServer.Services;
using WTTServerCommonLib.Helpers;
using WTTServerCommonLib.Services;
using Path = System.IO.Path;

namespace FPVDroneModServer
{
    [Injectable(InjectionType = InjectionType.Singleton, TypePriority = OnLoadOrder.Preload)]
    public class FPVDroneModServer(
        ISptLogger<FPVDroneModServer> logger,
        TankDeathService tankDeathService,
        WTTCustomQuestService questService,
        WTTCustomQuestZoneService zoneService,
        WTTCustomLootspawnService lootService,
        WTTCustomHideoutRecipeService recipeService,
        WTTCustomAssortSchemeService assortService,
        WTTCustomItemServiceExtended itemService,
        WTTCustomSlotImageService imageService,
        WTTCustomLocaleService localeService,
        WTTCustomItemParentService parentService) : IOnLoad
    {
        public string AssemblyLocation => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        public string ConfigPath => Path.Combine(AssemblyLocation, "config");
        public string ParentsPath => Path.Combine(AssemblyLocation, "parents");
        
        public async Task OnLoadAsync(CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            cancellationToken.ThrowIfCancellationRequested();
            tankDeathService.LoadTankStateConfig(ConfigPath, "tankdeathstate.json");
            imageService.CreateSlotImages(assembly, "slots");
            cancellationToken.ThrowIfCancellationRequested();
            await parentService.CreateCustomParents(assembly, "db/parents");
            cancellationToken.ThrowIfCancellationRequested();
            await localeService.CreateCustomLocales(assembly, "db/locales");
            cancellationToken.ThrowIfCancellationRequested();
            await itemService.CreateCustomItems(assembly, "db/items");
            cancellationToken.ThrowIfCancellationRequested();
            await questService.CreateCustomQuests(assembly, "db/quests");
            cancellationToken.ThrowIfCancellationRequested();
            await zoneService.CreateCustomQuestZones(assembly, "db/zones");
            cancellationToken.ThrowIfCancellationRequested();
            await lootService.CreateCustomLootSpawns(assembly, "db/loot");
            cancellationToken.ThrowIfCancellationRequested();
            await assortService.CreateCustomAssortSchemes(assembly, "db/assort");
            //await recipeService.CreateHideoutRecipes(Assembly.GetExecutingAssembly(), "db/recipes");
            
            logger.Success("Successfully loaded FPV Drone Mod! Don't blow yourself up.");
            
            await Task.CompletedTask;
        }
    }
}
