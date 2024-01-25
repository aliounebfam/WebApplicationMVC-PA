using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
    public class PerController : Controller
    {
        private readonly DBConnect _dbConnection;
        private readonly FiliereController _filiereController;

        public PerController(DBConnect dbConnection, FiliereController filiereController)
        {
            _dbConnection = dbConnection;
            _filiereController = filiereController;
        }

        private List<PerModel> GetPersFromDatabase()
        {
            List<PerModel> pers = new List<PerModel>();

            using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
            {
                MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM pers", mysqldbConnection);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    FiliereModel? filiere = _filiereController.GetFiliereByCode(reader.GetInt32(3));
                    PerModel per = new PerModel
                    {
                        Id = reader.GetInt32(0),
                        Nom = reader.GetString(1),
                        Specialite = reader.GetString(2),
                        CodeFiliere = reader.GetInt32(3),
                        Filiere = filiere
                    };

                    pers.Add(per);
                }
            }

            return pers;
        }

        public IActionResult Index()
        {
            ViewBag.Filieres = _filiereController.GetFilieresFromDatabase();

            List<PerModel> pers = GetPersFromDatabase();
            return View(pers);
        }

        private PerModel? GetPerById(int id)
        {
            using MySqlConnection mysqldbConnection = _dbConnection.CreateConnection();
            MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM pers WHERE id = @id", mysqldbConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                FiliereModel? filiere = _filiereController.GetFiliereByCode(reader.GetInt32(3));

                PerModel per = new PerModel
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Specialite = reader.GetString(2),
                    CodeFiliere = reader.GetInt32(3),
                    Filiere = filiere
                };

                return per;
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ajout(PerModel model)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO pers (nom, specialite, code_filiere) VALUES (@nom, @specialite, @code_filiere)", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@nom", model.Nom);
                    sqlCommand.Parameters.AddWithValue("@specialite", model.Specialite);
                    sqlCommand.Parameters.AddWithValue("@code_filiere", model.CodeFiliere);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "La personne a été ajoutée avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de l'ajout de la personne ❌ : {e.Message}";
            }

            List<PerModel> pers = GetPersFromDatabase();
            return View("Index", pers);
        }

        public IActionResult Edit(int id)
        {
            PerModel? per = GetPerById(id);

            if (per == null)
            {
                return RedirectToAction("Index");
            }

            per.Filieres = _filiereController.GetFilieresFromDatabase();

            return View(per);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PerModel model)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("UPDATE pers SET nom = @nom, specialite = @specialite, code_filiere = @code_filiere WHERE id = @id", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@id", model.Id);
                    sqlCommand.Parameters.AddWithValue("@nom", model.Nom);
                    sqlCommand.Parameters.AddWithValue("@specialite", model.Specialite);
                    sqlCommand.Parameters.AddWithValue("@code_filiere", model.SelectedFiliereCode);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "Les modifications ont été enregistrées avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de l'enregistrement des modifications ❌ : {e.Message}";
            }

            model.Filieres = _filiereController.GetFilieresFromDatabase();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM pers WHERE id = @id", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "La personne a été supprimée avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de la suppression de la personne ❌ : {e.Message}";
            }

            List<PerModel> pers = GetPersFromDatabase();
            return View("Index", pers);
        }
    }
}
