using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
    public class DepartementController(DBConnect dbConnection) : Controller
    {
        private readonly DBConnect _dbConnection = dbConnection;


        public List<DepartementModel> GetDepartementsFromDatabase()
        {
            List<DepartementModel> departements = [];

            using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
            {

                MySqlCommand sqlCommand = new("SELECT * FROM departements", mysqldbConnection);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    DepartementModel departement = new()
                    {
                        code = reader.GetInt32(0),
                        nom = reader.GetString(1)
                    };

                    departements.Add(departement);
                }
            }
            return departements;
        }

        public IActionResult Index()
        {
            List<DepartementModel> departements = GetDepartementsFromDatabase();

            return View(departements);
        }


        public DepartementModel? GetDepartementByCode(int code)
        {
            using MySqlConnection mysqldbConnection = _dbConnection.CreateConnection();
            MySqlCommand sqlCommand = new("SELECT * FROM departements WHERE code = @code", mysqldbConnection);
            sqlCommand.Parameters.AddWithValue("@code", code);

            using MySqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                DepartementModel departement = new()
                {
                    code = reader.GetInt32(0),
                    nom = reader.GetString(1)
                };

                return departement;
            }

            return null; // Retourner null si le departement n'est pas trouvé.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ajout(DepartementModel model)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new("INSERT INTO departements (code, nom) VALUES (@code, @nom)", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@code", model.code);
                    sqlCommand.Parameters.AddWithValue("@nom", model.nom);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "Le departement a été ajouté avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de l'ajout du departement ❌ : {e.Message}";
            }
            /*}*/

            List<DepartementModel> departements = GetDepartementsFromDatabase();
            return View("Index", departements);
        }

        public IActionResult Edit(int code)
        {
            DepartementModel? departement = GetDepartementByCode(code);

            if (departement == null)
            {
                return RedirectToAction("Index");
            }

            return View(departement);
        }


        [HttpPost]
        public IActionResult Edit(DepartementModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                    {
                        MySqlCommand sqlCommand = new MySqlCommand("UPDATE departements SET nom = @nom WHERE code = @code", mysqldbConnection);
                        sqlCommand.Parameters.AddWithValue("@code", model.code);
                        sqlCommand.Parameters.AddWithValue("@nom", model.nom);
                        sqlCommand.ExecuteNonQuery();
                    }

                    ViewBag.modalMessage = "Les modifications ont été enregistrées avec succès ✅";
                }
                catch (Exception e)
                {
                    ViewBag.modalMessage = $"Une erreur est survenue lors de l'enregistrement des modifications ❌ : {e.Message}";
                }
            }
            List<DepartementModel> departements = GetDepartementsFromDatabase();
            return View("Index", departements);
        }


        public IActionResult Delete(int code)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new("DELETE FROM departements WHERE code = @code", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@code", code);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "Le departement a été supprimé avec succès ✅";
            }

            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de la suppression du departement ❌ : {e.Message}";
            }

            List<DepartementModel> departements = GetDepartementsFromDatabase();
            return View("Index", departements);
        }

    }

}
