#region Using

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.CSharp;

#endregion

namespace Willcraftia.Net.Box.BuildApiKey
{
    class Program
    {
        const string BoxAppSettingsXml = "BoxAppSettings.xml";

        static string Template = "namespace Willcraftia.Net.Box.${Id} { public static class ApiKey { const string Value = \"${ApiKey}\"; } }";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: BuildAppKey <BoxAppId>");
                return;
            }

            var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var settingsPath = Path.Combine(personalFolder, BoxAppSettingsXml);

            if (!File.Exists(settingsPath))
            {
                Console.WriteLine(string.Format("The file '{0}' does not exist.", settingsPath));
                return;
            }

            var settings = LoadBoxAppSettings(settingsPath);

            var id = args[0];
            var app = settings.FindBoxAppById(id);
            if (app == null)
            {
                Console.WriteLine(string.Format("The application '{0}' is not defined in '{1}'.", id, settingsPath));
                return;
            }

            Console.WriteLine(string.Format("Generate BoxApiKey.dll for the application '{0}'.", id));

            var source = GenerateSources(app);
            var outputAssembly = GetOutputAssembly(app);

            BuildApiKeyAssembly(source, outputAssembly);
        }

        static string GenerateSources(BoxApp app)
        {
            var source = Template.Replace("${Id}", app.Id);
            source = source.Replace("${ApiKey}", app.ApiKey);

            return source;
        }

        static string GetOutputAssembly(BoxApp app)
        {
            return "Willcraftia.Net.Box." + app.Id + ".ApiKey.dll";
        }

        static bool BuildApiKeyAssembly(string source, string outputAssembly)
        {
            var provier = CodeDomProvider.CreateProvider("CSharp");

            var options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.OutputAssembly = outputAssembly;
            options.GenerateInMemory = false;
            options.TreatWarningsAsErrors = false;

            var results = provier.CompileAssemblyFromSource(options, source);
            if (0 < results.Errors.Count)
            {
                Console.WriteLine("Build failed:", results.PathToAssembly);
                foreach (var error in results.Errors)
                {
                    Console.WriteLine("    {0}", error);
                }
                return false;
            }

            Console.WriteLine("Build succeeded: " + results.PathToAssembly);
            return true;
        }

        static BoxAppSettings LoadBoxAppSettings(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(BoxAppSettings));
            using (var stream = File.OpenRead(path))
            {
                return xmlSerializer.Deserialize(stream) as BoxAppSettings;
            }
        }
    }
}
