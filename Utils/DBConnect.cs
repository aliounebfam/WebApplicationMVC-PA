using MySql.Data.MySqlClient;

namespace WebApplicationMVC.Utils
{
    public class DBConnect
    {
        private readonly IConfiguration _configuration;

        public DBConnect(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MySqlConnection CreateConnection()
        {
            try{
                string? connectionString = _configuration.GetConnectionString("GestionEtudiantDBConnection");
                var dbconnection = new MySqlConnection(connectionString);
                dbconnection.Open();
                return dbconnection;
            }
            catch(MySqlException ex) {
                Console.WriteLine($"Erreur lors de la création de la connexion à la base de données : {ex.Message}");
                throw;
            }
        }
    }
}
