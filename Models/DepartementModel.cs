namespace WebApplicationMVC.Models
{
    public class DepartementModel
    {
        public required int code { get; set; }
        public required string nom { get; set; }
        public List<FiliereModel>? Filieres { get; set; }
    }
}
