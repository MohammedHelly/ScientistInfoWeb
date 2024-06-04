namespace ScientistInfoWeb.Models
{
    public class ScientistInfo
    {
        public string Name { get; set; }
        public string Affiliation { get; set; }
        public string Title { get; set; }
        public string AuthorInfo { get; set; }
        public string Snippet { get; set; }
        public string Link { get; set; }
    }
  public class ScientistResult
  {
    public List<ScientistInfo> Results { get; set; }
    public int TotalRecords { get; set; }
  }
}
