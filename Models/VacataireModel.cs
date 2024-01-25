namespace WebApplicationMVC.Models
{
    public class VacataireModel
    {
        public int Id { get; set; }

        public string Nom { get; set; }

        public string Specialite { get; set; }

        public int CodeFiliere { get; set; }

        public FiliereModel Filiere { get; set; }

        public int? SelectedFiliereCode { get; set; }

        public List<FiliereModel>? Filieres { get; set; }
    }
}
