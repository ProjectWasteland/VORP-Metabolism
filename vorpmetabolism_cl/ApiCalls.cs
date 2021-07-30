using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpmetabolism_cl
{
    class ApiCalls : BaseScript
    {
        #region Properties

        public static bool APIShowOn { get; set; } = true;

        #endregion Properties

        #region Constructors

        public ApiCalls()
        {
            EventHandlers["vorpmetabolism:changeValue"] += new Action<string, int>(changeValue);
            EventHandlers["vorpmetabolism:setValue"] += new Action<string, int>(setValue);
            EventHandlers["vorpmetabolism:getValue"] += new Action<string, dynamic>(getValue);
            EventHandlers["vorpmetabolism:setHud"] += new Action<bool>(setHud);
            EventHandlers["vorpmetabolism:setValues"] += new Action<int, int>(setValues);
            EventHandlers["vorpmetabolism:getValues"] += new Action<dynamic>(getValues);
        }

        #endregion Constructors

        #region Methods

        private void setHud(bool enable)
        {
            APIShowOn = enable;
        }

        private void getValue(string key, dynamic cb)
        {
            string newKey = key.First().ToString().ToUpper() + key.Substring(1); //Fixed first char to upper case

            if (vorpmetabolism_init.pStatus.ContainsKey(newKey))
            {
                cb.Invoke(vorpmetabolism_init.pStatus[newKey].ToObject<int>());
            }
            else
            {
                cb.Invoke(null);
            }

        }

        private void changeValue(string key, int value)
        {
            string newKey = key.First().ToString().ToUpper() + key.Substring(1); //Fixed first char to upper case
            if (vorpmetabolism_init.pStatus.ContainsKey(newKey))
            {
                int newValue = vorpmetabolism_init.pStatus[newKey].ToObject<int>() + value;
                if (newKey.Equals("Metabolism"))
                {
                    if (newValue > 10000)
                    {
                        newValue = 10000;
                    }
                    else if (newValue < -10000)
                    {
                        newValue = -10000;
                    }
                }
                else
                {
                    if (newValue > 1000)
                    {
                        newValue = 1000;
                    }
                    else if (newValue < 0)
                    {
                        newValue = 0;
                    }
                }

                vorpmetabolism_init.pStatus[newKey] = newValue;

            }
        }

        private void setValue(string key, int value)
        {
            string newKey = key.First().ToString().ToUpper() + key.Substring(1); //Fixed first char to upper case
            if (!vorpmetabolism_init.pStatus.ContainsKey(newKey))
            {
                return;
            }

            var newValue = value;
            if (newKey.Equals("Metabolism"))
            {
                if (newValue > 10000)
                {
                    newValue = 10000;
                }
                else if (newValue < -10000)
                {
                    newValue = -10000;
                }
            }
            else
            {
                if (newValue > 1000)
                {
                    newValue = 1000;
                }
                else if (newValue < -1000)
                {
                    newValue = -1000;
                }
            }

            vorpmetabolism_init.pStatus[newKey] = newValue;
        }

        private void setValues(int food, int water)
        {
            if (food > 1000)
            {
                food = 1000;
            }
            else if (food < 0)
            {
                food = 0;
            }
            
            if (water > 1000)
            {
                water = 1000;
            }
            else if (water < 0)
            {
                water = 0;
            }

            vorpmetabolism_init.pStatus["Hunger"] = food;
            vorpmetabolism_init.pStatus["Thirst"] = water;
        }

        private void getValues(dynamic cb)
        {
            cb.Invoke(vorpmetabolism_init.pStatus["Hunger"].ToObject<int>(),
                      vorpmetabolism_init.pStatus["Thirst"].ToObject<int>());
        }

        #endregion Methods
    }
}
