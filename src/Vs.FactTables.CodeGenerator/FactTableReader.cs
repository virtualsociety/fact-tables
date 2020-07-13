using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Vs.FactTables.CodeGenerator
{
    public class FactTableReader
    {
        public string Read()
        {
            StringBuilder source = new StringBuilder();
            source.Append(@"using System;
using System.Collections.Generic;
using System.Text;

namespace Vs.FactTables {
");
            var s = FindFactTableDirectory();
            foreach (var factTable in new DirectoryInfo(s).GetFiles())
            {
                using (var reader = new StreamReader(factTable.FullName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.MissingFieldFound = null;
                    var records = csv.GetRecords<dynamic>();
                    // create struct
                    string classSuffix = "_Entity";
                    System.Text.StringBuilder @struct = new System.Text.StringBuilder();
                    @struct.AppendLine($"public class {ToPascalCase(factTable.Name)}{classSuffix} {{");
                    foreach (var column in records.ElementAt(0))
                    {
                        @struct.AppendLine($"public string {ToPascalCase(column.Key)} {{get; set;}}");
                    }
                    @struct.AppendLine($"public static List<{ToPascalCase(factTable.Name)}{classSuffix}> Entities = new List<{ToPascalCase(factTable.Name)}{classSuffix}>();");
                    @struct.AppendLine("public static void Init(){");
                    foreach (var record in records)
                    {
                        @struct.Append($"Entities.Add(new {ToPascalCase(factTable.Name)}{classSuffix}(){{");
                        foreach (var column in record)
                        {
                            @struct.Append($"{ToPascalCase(column.Key)} = \"{column.Value}\",");
                        }
                        @struct.Length=@struct.Length - 1;
                        @struct.AppendLine("});");
                    }

                    @struct.AppendLine("}}");
                    source.Append(@struct);
                }

            }
            source.AppendLine("}");
            return source.ToString();
        }
        private static string ToPascalCase(string s)
        {
            var words = s.Split(new[] { '-', '_','0','1','2','3','4','5','6','7','8','9',' ','/' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(word => word.Substring(0, 1).ToUpper() +
                                         word.Substring(1).ToLower());

            var result = String.Concat(words);
            return result.Replace(".csv", "").Replace(".","");
        }

        private static string FindFactTableDirectory()
        {
            var directory = "fact-tables";
            for (int i = 0; i < 10; i++)
            {
                if (Directory.Exists(directory))
                {
                    return directory;
                }
                directory = $"../{directory}";
            }
            throw new Exception("Directory not found.");
        }
    }
}
