namespace Admin.Data
{
    public class CountryInfo
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public int TotalCases { get; set; } = 0;
        public int TotalDeaths { get; set; } = 0;
        public string DeathPercentage => (this.TotalDeaths * 100) / this.TotalCases + "%";
        public List<CountryInfo> GetCountry()
        {
            var list = new List<CountryInfo>();
            list.Add(new CountryInfo() { Id = 1, Name = "USA", TotalCases = 12000, TotalDeaths = 2000 });
            list.Add(new CountryInfo() { Id = 2, Name = "VN", TotalCases = 13000, TotalDeaths = 2060 });
            list.Add(new CountryInfo() { Id = 3, Name = "China", TotalCases = 14000, TotalDeaths = 2300 });
            list.Add(new CountryInfo() { Id = 4, Name = "Japan", TotalCases = 178000, TotalDeaths = 2254 });
            return list;
        }

    }
}
