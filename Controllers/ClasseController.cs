using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
    
    public class ClasseController(DBConnect dbConnection) : Controller
    {
        private readonly DBConnect _dbConnection = dbConnection;

        public IActionResult Index()
        {
            List<ClasseModel> classes = GetClassesFromDatabase();

            return View(classes);
        }

        public IActionResult Delete(int code)
        {

            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM classes WHERE code = @code", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@code", code);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.SuppressionMessage = "La classe a été supprimée avec succès ✅";
            }

            catch (Exception e)
            {
                ViewBag.SuppressionMessage = $"Une erreur est survenue lors de la suppression de la classe ❌ : {e.Message}";
            }
             
            List<ClasseModel> classes = GetClassesFromDatabase();
            return View("Index", classes);
  
        }


        private List<ClasseModel> GetClassesFromDatabase()
        {
            List<ClasseModel> classes = new List<ClasseModel>();

            using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
            {

                MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM classes", mysqldbConnection);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ClasseModel classe = new()
                    {
                        code = reader.GetInt32(0),
                        libelle = reader.GetString(1),
                        description = reader.GetString(2)
                    };

                    classes.Add(classe);
                }

            }
            return classes;
        }
    }
}
