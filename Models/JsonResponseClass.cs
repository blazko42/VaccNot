using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaccNot.Models
{
    public class JsonResponseClass
    {
        public List<VaccineSlot> Content { get; set; }

        public static List<VaccineSlot> ParseJSONVaccineSlotsResponse(string slots)
        {
            try
            {
                return JsonConvert.DeserializeObject<JsonResponseClass>(slots).Content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}