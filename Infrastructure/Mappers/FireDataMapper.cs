using FireDataWebService.Domain.Models;
using System.Globalization;

namespace FireDataWebService.Domain.Mappers
{
    public static class FireDataMapper
    {
        public static FireDataModel FromCsv(string csvLine)
        {
            // Разбиваем строку CSV на массив значений, учитывая экранированные кавычки и запятые внутри значений
            List<string> values = ParseCsvLine(csvLine);

            // Убираем кавычки из значений, если они присутствуют
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = values[i].Trim('"');
            }

            // Парсим каждое значение в соответствующий тип данных и создаем объект FireDataModel
            return new FireDataModel
            {
                Id = int.Parse(values[0]),
                NewFireIdUnique = int.Parse(values[1]),
                Area = double.Parse(values[2], CultureInfo.InvariantCulture),
                Geometry = values[3],
                DtStart = DateTime.Parse(values[4]),
                SinceStart = DateTime.Parse(values[5]),
                DtEnd = DateTime.Parse(values[6]),
                SinceEnd = DateTime.Parse(values[7]),
                FireIds = values[8],
                Ids = values[9],
                CountPolygons = int.Parse(values[10]),
                Duration = int.Parse(values[11]),
                Centroid = values[12],
                Type = values[13],
                Year = int.Parse(values[14])
            };
        }

        private static List<string> ParseCsvLine(string csvLine)
        {
            List<string> values = new List<string>();
            bool inQuotes = false;
            string currentValue = "";

            // Удаление первых и последних двух кавычек
            csvLine = csvLine.Substring(1);
            csvLine = csvLine.Substring(0, csvLine.Length - 1);


            for (int i = 0; i < csvLine.Length; i++)
            {
                char c = csvLine[i];
                if (c == '"')
                {
                    if (csvLine[i + 1] != '"')
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue);
                    currentValue = "";
                }
                else
                {
                    currentValue += c;
                }
            }
            
            // Добавляем последнее значение после завершения строки CSV
            values.Add(currentValue);

            return values;
        }
    }
}
