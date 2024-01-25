using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data.Common;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
    public class FiliereController : Controller
    {
        private readonly DBConnect _dbConnection;
        private readonly DepartementController _departementController;

        public FiliereController(DBConnect dbConnection, DepartementController departementController)
        {
            _dbConnection = dbConnection;
            _departementController = departementController;
        }

        public List<FiliereModel> GetFilieresFromDatabase()
        {
            List<FiliereModel> filieres = new List<FiliereModel>();

            using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
            {
                MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM filieres", mysqldbConnection);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    DepartementModel? departement = _departementController.GetDepartementByCode(reader.GetInt32(2));
                    FiliereModel filiere = new FiliereModel
                    {
                        Code = reader.GetInt32(0),
                        Nom = reader.GetString(1),
                        CodeDepartement = reader.GetInt32(2),
                        Departement = departement
                    };

                    filieres.Add(filiere);
                }
            }

            return filieres;
        }

        public IActionResult Index()
        {
            ViewBag.Departements = _departementController.GetDepartementsFromDatabase();

            List<FiliereModel> filieres = GetFilieresFromDatabase();
            return View(filieres);
        }

        public FiliereModel? GetFiliereByCode(int code)
        {
            using MySqlConnection mysqldbConnection = _dbConnection.CreateConnection();
            MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM filieres WHERE code = @code", mysqldbConnection);
            sqlCommand.Parameters.AddWithValue("@code", code);

            using MySqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                DepartementModel? departement = _departementController.GetDepartementByCode(reader.GetInt32(2));

                FiliereModel filiere = new FiliereModel
                {
                    Code = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    CodeDepartement = reader.GetInt32(2),
                    Departement = departement
                };

                return filiere;
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ajout(FiliereModel model)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO filieres (code, nom, code_departement) VALUES (@code, @nom, @code_departement)", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@code", model.Code);
                    sqlCommand.Parameters.AddWithValue("@nom", model.Nom);
                    sqlCommand.Parameters.AddWithValue("@code_departement", model.CodeDepartement);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "La filière a été ajoutée avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de l'ajout de la filière ❌ : {e.Message}";
            }

            List<FiliereModel> filieres = GetFilieresFromDatabase();
            return View("Index", filieres);
        }

        public IActionResult Edit(int code)
        {
            FiliereModel? filiere = GetFiliereByCode(code);

            if (filiere == null)
            {
                return RedirectToAction("Index");
            }

            filiere.Departements = _departementController.GetDepartementsFromDatabase();

            return View(filiere);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FiliereModel model)
        {
            /*if (ModelState.IsValid)*/
            {
                try
                {
                    using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                    {
                        MySqlCommand sqlCommand = new MySqlCommand("UPDATE filieres SET nom = @nom, code_departement = @code_departement WHERE code = @code", mysqldbConnection);
                        sqlCommand.Parameters.AddWithValue("@code", model.Code);
                        sqlCommand.Parameters.AddWithValue("@nom", model.Nom);
                        sqlCommand.Parameters.AddWithValue("@code_departement", model.SelectedDepartementCode);
                        sqlCommand.ExecuteNonQuery();
                    }

                    ViewBag.modalMessage = "Les modifications ont été enregistrées avec succès ✅";
                }
                catch (Exception e)
                {
                    ViewBag.modalMessage = $"Une erreur est survenue lors de l'enregistrement des modifications ❌ : {e.Message}";
                }
            }
            model.Departements = _departementController.GetDepartementsFromDatabase();
            return View(model);
        }/**/


        public IActionResult Delete(int code)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM filieres WHERE code = @code", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@code", code);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "La filière a été supprimée avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de la suppression de la filière ❌ : {e.Message}";
            }

            List<FiliereModel> filieres = GetFilieresFromDatabase();
            return View("Index", filieres);
        }
    }



}
