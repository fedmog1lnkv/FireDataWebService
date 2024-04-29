namespace FireDataWebService.Domain.Models
{
    public class WeatherDataModel
    {
        public int WeatherStationId { get; set; }
        public DateTime Date { get; set; }
        public string? WW { get; set; }
        public double? T { get; set; }
        public double? Ff { get; set; }
        public double? P { get; set; }
        public double? U { get; set; }
        public double? V { get; set; }
        public double? VV { get; set; }
        public double? Td { get; set; }
        public double? RRR { get; set; }
        public double? WWCode { get; set; }
        public double? Lon { get; set; }
        public double? Lat { get; set; }
        public string? WWType { get; set; }
    }
}
