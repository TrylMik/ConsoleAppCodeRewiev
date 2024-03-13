namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DataReader
    {
        private IEnumerable<ImportedObject> ImportedObjects;

        public void ImportAndPrintData(string fileToImport)
        {
            ImportedObjects = new List<ImportedObject>();

            var streamReader = new StreamReader(fileToImport);

            var importedLines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                importedLines.Add(line);
            }

            for (int i = 1; i < importedLines.Count; i++)
            {
                var importedLine = importedLines[i];
                if(importedLine.Length > 0)
                {
                    var values = importedLine.Split(';');
                    var importedObject = new ImportedObject();
                    importedObject.Type = values[0];
                    importedObject.Name = values[1];
                    importedObject.Schema = values[2];
                    importedObject.ParentName = values[3];
                    importedObject.ParentType = values[4];
                    importedObject.DataType = values[5];
                    try { importedObject.IsNullable = values[6]; }
                    catch { importedObject.IsNullable = "1"; }            
                    ((List<ImportedObject>)ImportedObjects).Add(importedObject);
                }
                
            }

            ClearAndCorrectImportedData();
            AssignNumerOfChildren();
            PrintTablesAndColumns();

            Console.ReadLine();
        }

        private void ClearAndCorrectImportedData()
        {
            foreach (var importedObject in ImportedObjects)
            {
                importedObject.Type = importedObject.Type.Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper();
                importedObject.Name = importedObject.Name.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.Schema = importedObject.Schema.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentName = importedObject.ParentName.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentType = importedObject.ParentType.Trim().Replace(" ", "").Replace(Environment.NewLine, "");

                ////Przykład innego zapisania tej funkcji
                //var properties = typeof(ImportedObject).GetProperties();
                //foreach (var property in properties)
                //{
                //    if (property.Name == "Type" ||
                //        property.Name == "Name" ||
                //        property.Name == "Schema" ||
                //        property.Name == "ParentName" ||
                //        property.Name == "ParentType")
                //    {
                //        var value = (string)property.GetValue(importedObject);
                //        value = property.Name == "Type" ?
                //            value.Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper() :
                //            value.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                //        property.SetValue(importedObject, value);
                //    }    
                //}
            }
        }

        private void AssignNumerOfChildren()
        {
            for (int i = 0; i < ImportedObjects.Count(); i++)
            {
                var importedObject = ImportedObjects.ToArray()[i];
                foreach (var impObj in ImportedObjects)
                {
                    if (impObj.ParentType == importedObject.Type && impObj.ParentName == importedObject.Name)
                        importedObject.NumberOfChildren = 1 + importedObject.NumberOfChildren;
                }
            }
        }

        private void PrintTablesAndColumns()
        {
            foreach (var database in ImportedObjects)
            {
                if (database.Type == "DATABASE")
                {
                    Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                    // print all database's tables
                    foreach (var table in ImportedObjects)
                    {
                        if (table.ParentType.ToUpper() == database.Type && table.ParentName == database.Name)
                        {
                            Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                            // print all table's columns
                            foreach (var column in ImportedObjects)
                            {
                                if (column.ParentType.ToUpper() == table.Type && column.ParentName == table.Name)
                                    Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable == "1" ? "accepts nulls" : "with no nulls")}");
                            }
                        }
                    }
                }
            }
        }
    }

    internal class ImportedObject : ImportedObjectBaseClass
    {
        public string Schema;
        public string ParentName;
        public string ParentType { get; set; }
        public string DataType { get; set; }
        public string IsNullable { get; set; }

        public double NumberOfChildren;
    }

    internal class ImportedObjectBaseClass
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
