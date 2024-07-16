namespace IotTelemetrySimulator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;

    public class TelemetryCsvValues : IDisposable
    {
        private readonly Dictionary<TelemetryCsvVariable, CsvReader> filePathCsvReaderMap = new Dictionary<TelemetryCsvVariable, CsvReader>();

        public IList<TelemetryVariable> Variables { get; }

        public TelemetryCsvValues(IList<TelemetryVariable> variables)
        {
            this.Variables = variables;
        }

        public Dictionary<string, object> NextValues(Dictionary<string, object> previous)
        {
            var next = new Dictionary<string, object>();

            foreach (var entry in this.filePathCsvReaderMap.ToList())
            {
            }

            foreach (var val in this.Variables.Where(x => x.Csv != null))
            {
                if (!this.filePathCsvReaderMap.TryGetValue(val.Csv, out var csvReader))
                {
                    csvReader = this.CreateReadCsvReader(val.Csv);
                    this.filePathCsvReaderMap[val.Csv] = csvReader;
                }

                if (!csvReader.Read())
                {
                    csvReader.Dispose();
                    csvReader = this.CreateReadCsvReader(val.Csv);
                    this.filePathCsvReaderMap[val.Csv] = csvReader;
                    csvReader.Read();
                }

                if (val.Csv.FieldIndex != null)
                {
                    next[val.Name] = csvReader.GetField(this.GetFieldType(val.Csv), val.Csv.FieldIndex.Value);
                }
                else
                {
                    next[val.Name] = csvReader.GetField(this.GetFieldType(val.Csv), val.Csv.FieldName ?? throw new InvalidOperationException("csv: missing 'fieldName' or 'fieldIndex'"));
                }
            }

            return next;
        }

        private Type GetFieldType(TelemetryCsvVariable variable)
        {
            if (variable.Int)
            {
                return typeof(int);
            }

            if (variable.Boolean)
            {
                return typeof(bool);
            }

            if (variable.Double)
            {
                return typeof(double);
            }

            return typeof(string);
        }

        private CsvReader CreateReadCsvReader(TelemetryCsvVariable variable)
        {
            TextReader reader;
            if (!string.IsNullOrEmpty(variable.Contents))
            {
                reader = new StringReader(variable.Contents);
            }
            else
            {
                reader = new StreamReader(variable.FilePath);
            }

            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            if (!csvReader.Read())
            {
                throw new InvalidOperationException($"csv: unable to read row from: {variable.FilePath}");
            }

            if (variable.Headers || !string.IsNullOrEmpty(variable.FieldName))
            {
                if (!csvReader.ReadHeader())
                {
                    throw new InvalidOperationException($"csv: unable to read header from: {variable.FilePath}");
                }
            }

            return csvReader;
        }

        public void Dispose()
        {
            var map = this.filePathCsvReaderMap.ToList();
            this.filePathCsvReaderMap.Clear();

            foreach (var entry in map)
            {
                entry.Value.Dispose();
            }
        }
    }
}
