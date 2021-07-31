using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json.Linq;

namespace vorpmetabolism_sv
{
    public class vorpmetabolism_init : BaseScript
    {
        public static dynamic CORE;

        public vorpmetabolism_init()
        {
            EventHandlers["vorpmetabolism:SaveLastStatus"] += new Action<Player, string>(SaveLastStatus);

            EventHandlers["vorpmetabolism:GetStatus"] += new Action<Player>(GetLastStatus);
            RegisterUsableItems();

            TriggerEvent("getCore", new Action<dynamic>(dic => { CORE = dic; }));
        }

        public async Task RegisterUsableItems()
        {
            await Delay(3000);
            Debug.WriteLine($"Metabolism: Loading {LoadConfig.Config["ItemsToUse"].Count().ToString()} items usables ");
            for (var i = 0; i < LoadConfig.Config["ItemsToUse"].Count(); i++)
            {
                var index = i;
                TriggerEvent("vorpCore:registerUsableItem", LoadConfig.Config["ItemsToUse"][i]["Name"].ToString(),
                             new Action<dynamic>(data =>
                             {
                                 Player p = Players[data.source];
                                 string itemLabel = data.item.label;
                                 p.TriggerEvent("vorpmetabolism:useItem", index, itemLabel);
                                 TriggerEvent("vorpCore:subItem", data.source,
                                              LoadConfig.Config["ItemsToUse"][index]["Name"].ToString(), 1);
                             }));
            }
        }

        private void GetLastStatus([FromSource] Player player)
        {
            var _source = int.Parse(player.Handle);
            var UserCharacter = CORE.getUser(int.Parse(player.Handle)).getUsedCharacter;
            string s_status = UserCharacter.status;

            if (s_status.Length > 5)
            {
                player.TriggerEvent("vorpmetabolism:StartFunctions", s_status);
            }
            else
            {
                var status = new JObject();
                status.Add("Hunger", LoadConfig.Config["FirstHungerStatus"].ToObject<int>());
                status.Add("Thirst", LoadConfig.Config["FirstThirstStatus"].ToObject<int>());
                status.Add("Metabolism", LoadConfig.Config["FirstMetabolismStatus"].ToObject<int>());

                UserCharacter.setStatus(status.ToString());

                player.TriggerEvent("vorpmetabolism:StartFunctions", status.ToString());
            }
        }

        private void SaveLastStatus([FromSource] Player player, string status)
        {
            var UserCharacter = CORE.getUser(int.Parse(player.Handle)).getUsedCharacter;
            UserCharacter.setStatus(status);
        }
    }
}
