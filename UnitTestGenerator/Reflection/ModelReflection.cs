using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTestGenerator.Reflection
{
    public class ModelReflection
    {
        public void GenerateTest(string assemblyPath, string targetoutput)
        {
            GetModels(assemblyPath, targetoutput);
        }

        public void GetModels(string assemblyPath, string targetoutput)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath).MainModule.Types.Where(_ => _.IsPublic);

            if (!Directory.Exists(Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + targetoutput))
                Directory.CreateDirectory(targetoutput);

            var propertyAvailable = false;

            foreach (var item in assemblyDefinition)
            {
                propertyAvailable = false;

                // Read content // 
                string unitTestText = File.ReadAllText(Environment.CurrentDirectory + @"\template\xunit_template.txt");

                // parse target output

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("var target = new " + item.Name + "();\n");

                foreach (var propitem in item.Properties)
                {
                    propertyAvailable = true;

                    var setValueForTargetType = "null";

                    if (propitem.PropertyType.Name.ToLowerInvariant() == "string")
                    {
                        setValueForTargetType = "\"fakeValue\"";
                    }
                    else if (propitem.PropertyType.Name.ToLowerInvariant() == "int32")
                    {
                        setValueForTargetType = "0";
                    }
                    else if (propitem.PropertyType.Name.ToLowerInvariant() == "boolean")
                    {
                        setValueForTargetType = "true";
                    }
                    else if (propitem.PropertyType.Name.ToLowerInvariant() == "int64")
                    {
                        setValueForTargetType = "0";
                    }

                    stringBuilder.Append("\t\t target." + propitem.Name + " = " + setValueForTargetType + ";\n");

                }

                if (propertyAvailable)
                {
                    unitTestText = unitTestText.Replace("###Head", item.Name + "Tests");

                    var finaloutput  = unitTestText.Replace("#####", stringBuilder.ToString());
                    var unitTestOutputName = item.Name + "Tests.cs";
                    File.WriteAllText($@"{Environment.CurrentDirectory}\{targetoutput}\{unitTestOutputName}", finaloutput);
                }
            }
        }
    }
}
