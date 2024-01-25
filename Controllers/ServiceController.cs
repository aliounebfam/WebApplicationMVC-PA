using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
    public class ServiceController(DBConnect dbConnection) : Controller
    {
        private readonly DBConnect _dbConnection = dbConnection;

        public IActionResult Index()
        {
            List<ServiceModel> services = GetServicesFromDatabase();

            return View(services);
        }

        private ServiceModel? GetServiceById(int id)
        {
            using MySqlConnection mysqldbConnection = _dbConnection.CreateConnection();
            MySqlCommand sqlCommand = new("SELECT * FROM services WHERE id = @id", mysqldbConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                ServiceModel service = new()
                {
                    id = reader.GetInt32(0),
                    nom = reader.GetString(1),
                    description = reader.GetString(2)
                };

                return service;
            }

            return null; // Retourner null si le service n'est pas trouvé.
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ajout(ServiceModel model)
        {
            /*if (ModelState.IsValid)
            {*/
            try
            {
                    using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                    {
                        MySqlCommand sqlCommand = new("INSERT INTO services (nom, description) VALUES (@nom, @description)", mysqldbConnection);
                        sqlCommand.Parameters.AddWithValue("@nom", model.nom);
                        sqlCommand.Parameters.AddWithValue("@description", model.description);
                        sqlCommand.ExecuteNonQuery();
                    }

                    ViewBag.modalMessage = "Le service a été ajouté avec succès ✅";
                }
                catch (Exception e)
                {
                    ViewBag.modalMessage = $"Une erreur est survenue lors de l'ajout du service ❌ : {e.Message}";
                }
            /*}*/

            List<ServiceModel> services = GetServicesFromDatabase();
            return View("Index", services);
        }

        public IActionResult Edit(int id)
        {
            ServiceModel? service = GetServiceById(id);

            if (service == null)
            {
                return RedirectToAction("Index");
            }

            return View(service);
        }


        [HttpPost]
        public IActionResult Edit(ServiceModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                    {
                        MySqlCommand sqlCommand = new MySqlCommand("UPDATE services SET nom = @nom, description = @description WHERE id = @id", mysqldbConnection);
                        sqlCommand.Parameters.AddWithValue("@id", model.id);
                        sqlCommand.Parameters.AddWithValue("@nom", model.nom);
                        sqlCommand.Parameters.AddWithValue("@description", model.description);
                        sqlCommand.ExecuteNonQuery();
                    }

                    ViewBag.modalMessage = "Les modifications ont été enregistrées avec succès ✅";
                }
                catch (Exception e)
                {
                    ViewBag.modalMessage = $"Une erreur est survenue lors de l'enregistrement des modifications ❌ : {e.Message}";
                }
            }
            List<ServiceModel> services = GetServicesFromDatabase();
            return View("Index", services);
        }


        public IActionResult Delete(int id)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new("DELETE FROM services WHERE id = @id", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "Le service a été supprimé avec succès ✅";
            }

            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de la suppression du service ❌ : {e.Message}";
            }

            List<ServiceModel> services = GetServicesFromDatabase();
            return View("Index", services);
        }


        private List<ServiceModel> GetServicesFromDatabase()
        {
            List<ServiceModel> services = [];

            using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
            {

                MySqlCommand sqlCommand = new("SELECT * FROM services", mysqldbConnection);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    ServiceModel service = new()
                    {
                        id = reader.GetInt32(0),
                        nom = reader.GetString(1),
                        description = reader.GetString(2)
                    };

                    services.Add(service);
                }
            }
            return services;
        }
    }
}
