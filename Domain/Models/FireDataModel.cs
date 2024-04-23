namespace FireDataWebService.Domain.Models
{
    public record FireDataModel
    {
        public int Id { get; set; }
        public int NewFireIdUnique { get; set; }
        public double Area { get; set; }
        public string Geometry { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime SinceStart { get; set; }
        public DateTime DtEnd { get; set; }
        public DateTime SinceEnd { get; set; }
        public string FireIds { get; set; }
        public string Ids { get; set; }
        public int CountPolygons { get; set; }
        public int Duration { get; set; }
        public string Centroid { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
    }
}
