using FireDataWebService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using InteractiveMapWeb.Infrastructure.InMemoryStorage;
using Newtonsoft.Json;

namespace FireDataWebService.Controllers
{
    [Route("fires")]
    [ApiController]
    public class FiresController : ControllerBase
    {
        private readonly FireInMemoryStorage _fireStorage;

        public FiresController(FireInMemoryStorage fireStorage)
        {
            _fireStorage = fireStorage;
        }

        [HttpGet("geojson")]
        public async Task<IActionResult> GetFiresGeoJson(DateTime? requestedDate = null, DateTime? endDate = null)
        {
            List<FireDataModel> fires;
            if (requestedDate == null && endDate == null)
            {
                fires = _fireStorage.GetAllFires();
            }
            else if (requestedDate != null && endDate == null)
            {
                fires = _fireStorage.FilterFiresByDate(requestedDate);
            }
            else
            {
                Console.WriteLine("FilterFiresByDateRange");
                fires = _fireStorage.FilterFiresByDateRange(requestedDate, endDate);
            }

            Console.WriteLine($"Number of fires: {fires.Count()}");

            var geoJson = ConvertToFiresGeoJson(fires);
            Console.WriteLine("geoJson");

            return Ok(geoJson);
        }

        public static string ConvertToFiresGeoJson(IEnumerable<FireDataModel> fireDataList)
        {
            var features = new List<Feature>();

            foreach (var fireData in fireDataList)
            {
                List<IPosition> coordinates;
                try
                {
                    coordinates = ParseMultiPolygonCoordinates(fireData.Geometry);
                }
                catch (Exception e)
                {
                    continue;
                }

                var properties = new Dictionary<string, object>
                {
                    { "id", fireData.Id },
                    { "new_fire_id_unique", fireData.NewFireIdUnique },
                    { "area", fireData.Area },
                    { "dt_start", fireData.DtStart.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "since_start", fireData.SinceStart.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "dt_end", fireData.DtEnd.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "since_end", fireData.SinceEnd.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "fire_ids", fireData.FireIds },
                    { "ids", fireData.Ids },
                    { "count_polygons", fireData.CountPolygons },
                    { "duration", fireData.Duration },
                    { "centroid", fireData.Centroid },
                    { "type", fireData.Type },
                    { "year", fireData.Year }
                };

                var geometries = new List<IGeometryObject>();

                // Создаем MultiPolygon
                var multiPolygon = new MultiPolygon(new List<Polygon>
                {
                    new Polygon(new List<LineString>
                    {
                        new LineString(coordinates)
                    })
                });
                geometries.Add(multiPolygon);

                // Создаем точку
                var pointCoordinates = ParsePointCoordinates(fireData.Centroid);
                var point = new Point(new Position(pointCoordinates[1], pointCoordinates[0])); // Обратите внимание на порядок координат
                geometries.Add(point);

                // Создаем объект GeometryCollection
                var geometryCollection = new GeometryCollection(geometries);

                // Создаем объект Feature
                var feature = new Feature(geometryCollection, properties);
                features.Add(feature);
            }

            var featureCollection = new FeatureCollection(features);
            return JsonConvert.SerializeObject(featureCollection);
        }

        private static List<IPosition> ParseMultiPolygonCoordinates(string geometry)
        {
            var coordsStart = geometry.IndexOf("(((") + 3;
            var coordsEnd = geometry.LastIndexOf(")))");
            var coordsString = geometry.Substring(coordsStart, coordsEnd - coordsStart);

            var coords = coordsString.Split(',');
            var coordinates = new List<IPosition>();

            foreach (var coord in coords)
            {
                var xy = coord.Trim().Split(' ');
                coordinates.Add(new Position(double.Parse(xy[1], CultureInfo.InvariantCulture), double.Parse(xy[0], CultureInfo.InvariantCulture))); // GeoJSON.Net uses (latitude, longitude) format
            }

            return coordinates;
        }

        private static List<double> ParsePointCoordinates(string geometry)
        {
            var coordsStart = geometry.IndexOf("(") + 1;
            var coordsEnd = geometry.LastIndexOf(")");
            var coordsString = geometry.Substring(coordsStart, coordsEnd - coordsStart);

            var xy = coordsString.Trim().Split(' ');
            var coordinates = new List<double>();

            foreach (var coord in xy)
            {
                coordinates.Add(double.Parse(coord, CultureInfo.InvariantCulture));
            }

            return coordinates;
        }
    }
}
