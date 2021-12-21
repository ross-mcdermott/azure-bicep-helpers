using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace azure_bicep_helpers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating Mapping File...");
            
            var myDeserializedClass =  JsonConvert.DeserializeObject<RoleDefinition[]>(File.ReadAllText("roles.json")); 

            Dictionary<string,string> map = new Dictionary<string,string>();

            foreach(var i in myDeserializedClass.OrderBy(x=>x.roleName)) {
                if("BuiltInRole".Equals(i.roleType, StringComparison.InvariantCultureIgnoreCase)){
                    // map output
                    map[i.roleName] = i.name;
                    Console.WriteLine($" - Mapping '{i.roleName}' to '{i.name}'.");
                }
            }

            File.WriteAllText("wellknownroles.json", JsonConvert.SerializeObject(map, Formatting.Indented));

            Console.WriteLine("Done.");
        }
    }

    public class RoleDefinition
    {
        public string id { get; set; }
        public string roleName { get; set; }
        public string name { get; set; }
        public string roleType {get;set;}
    }
}
