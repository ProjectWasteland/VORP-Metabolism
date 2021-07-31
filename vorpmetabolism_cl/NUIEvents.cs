using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;

namespace vorpmetabolism_cl
{
    internal class NUIEvents : BaseScript
    {
        public static async Task UpdateHUD()
        {
            var msgNUI = new JObject();

            var thirst = vorpmetabolism_init.pStatus["Thirst"].ToObject<double>() / 1000;
            var hunger = vorpmetabolism_init.pStatus["Hunger"].ToObject<double>() / 1000;

            msgNUI.Add("action", "update");
            msgNUI.Add("water", thirst);
            msgNUI.Add("food", hunger);

            API.SendNuiMessage(msgNUI.ToString());
            TriggerEvent("joew-at:changeMetabolisms", vorpmetabolism_init.pStatus["Thirst"].ToObject<int>(),
                         vorpmetabolism_init.pStatus["Hunger"].ToObject<int>());
        }

        public static async Task ShowHUD(bool show)
        {
            var msgNUI = new JObject();
            if (show)
            {
                msgNUI.Add("action", "show");
            }
            else
            {
                msgNUI.Add("action", "hide");
            }

            API.SendNuiMessage(msgNUI.ToString());
        }
    }
}
