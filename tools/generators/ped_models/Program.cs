using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PedModelsPageCreator;

namespace PedPageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var depsFolder = "../../deps/gta-v-data-dumps/";
            var pedsJson = depsFolder + "peds.json";

            var pedModelsFile = "../../../articles/ped/models.md";
            var imagePath = "~/altv-docs-gta-assets/images/ped/models/";

            /*
             * Read JSON files from gta-v-data-dumps by DurtyFree
             */

            if (!File.Exists(pedsJson))
            {
                Console.WriteLine("Couldn't find " + pedsJson + ". Are you running the project with 'dotnet run .\\generators-ped_models.csproj'?");
                Environment.Exit(1);
            }

            using var readerPeds = new StreamReader(pedsJson);
            var jsonPeds = readerPeds.ReadToEnd();
            var peds = JsonConvert.DeserializeObject<List<Ped>>(jsonPeds);

            var sortedPedsByName = peds.OrderBy(x => x.Name);

            /*
             * Generate Images
             */
            var gallery = File.CreateText(pedModelsFile);

            gallery.WriteLine("<!--- This page gets generated with tools/deps/generators/ped_models -->");
            gallery.WriteLine("# Ped Models");

            gallery.WriteLine("## Gallery");

            foreach (var ped in sortedPedsByName)
            {
                if (ped.DlcName.ToLower() == "titleupdate")
                {
                    gallery.WriteLine("![Missing image '" + ped.Name + ".png'](" + imagePath + ped.Name.ToLower() + ".png)<br/>");
                    gallery.WriteLine("**Name:** " + ped.Name + "<br/>");
                    gallery.WriteLine("**Hash:** " + ped.HexHash + "<br/>");
                    gallery.WriteLine("**Type:** " + ped.Pedtype.ToUpper() + "<br/>");
                    gallery.WriteLine("<br/>");
                }
                else
                {
                    gallery.WriteLine("![Missing image '" + ped.Name + ".png'](" + imagePath + ped.Name.ToLower() + ".png)<br/>");
                    gallery.WriteLine("**Name:** " + ped.Name + "<br/>");
                    gallery.WriteLine("**Hash:** " + ped.HexHash + "<br/>");
                    gallery.WriteLine("**Type:** " + ped.Pedtype.ToUpper() + "<br/>");
                    gallery.WriteLine("**DLC:** " + ped.DlcName.ToLower() + "<br/>");
                    gallery.WriteLine("<br/>");
                }
            }

            gallery.WriteLine("<br/>Created with [GTA V Data Dumps from DurtyFree](https://github.com/DurtyFree/gta-v-data-dumps)<br/>");

            /*
             * Generate Snippets
             */

            gallery.WriteLine("## Snippets");

            gallery.WriteLine("# [JavaScript](#tab/tab1-0)");
            gallery.WriteLine("```js");
            gallery.WriteLine("let PedModel = {");

            var i = 0;
            foreach (var ped in sortedPedsByName)
            {
                if (i < sortedPedsByName.Count() - 1)
                    gallery.WriteLine($"\t{ped.Name.ToLower()}: {ped.HexHash},");
                else
                    gallery.WriteLine($"\t{ped.Name.ToLower()}: {ped.HexHash}");

                i++;
            }
            gallery.WriteLine("};");
            gallery.WriteLine("```");
            gallery.WriteLine("# [TypeScript](#tab/tab1-1)");
            gallery.WriteLine("```ts");
            gallery.WriteLine("export enum PedModel {");

            i = 0;
            foreach (var ped in sortedPedsByName)
            {
                if (i < sortedPedsByName.Count() - 1)
                    gallery.WriteLine($"\t{ped.Name.ToLower()} = {ped.HexHash},");
                else
                    gallery.WriteLine($"\t{ped.Name.ToLower()} = {ped.HexHash}");

                i++;
            }

            gallery.WriteLine("};");
            gallery.WriteLine("```");
            gallery.WriteLine("***");
            gallery.WriteLine("Created with [GTA V Data Dumps from DurtyFree](https://github.com/DurtyFree/gta-v-data-dumps)<br/>");

            gallery.Close();
            Console.WriteLine($"{pedModelsFile} created for {peds.Count} peds.");

            Console.WriteLine("This tool is using data files from https://github.com/DurtyFree/gta-v-data-dumps");
        }
    }
}
