namespace WebApplicationMVC.Models
{
    public class FiliereModel
    {
        public required int Code { get; set; }
        public required string Nom { get; set; }
        public int CodeDepartement { get; set; }

        public int? SelectedDepartementCode { get; set; }
        public required DepartementModel Departement { get; set; }

        public List <DepartementModel>? Departements { get; set; }
    }
}
