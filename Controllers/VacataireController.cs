using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WebApplicationMVC.Models;
using WebApplicationMVC.Utils;

namespace WebApplicationMVC.Controllers
{
    public class VacataireController : Controller
    {
        private readonly DBConnect _dbConnection;
        private readonly FiliereController _filiereController;

        public VacataireController(DBConnect dbConnection, FiliereController filiereController)
        {
            _dbConnection = dbConnection;
            _filiereController = filiereController;
        }

        private List<VacataireModel> GetVacatairesFromDatabase()
        {
            List<VacataireModel> vacataires = new List<VacataireModel>();

            using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
            {
                MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM vacantaires", mysqldbConnection);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    FiliereModel? filiere = _filiereController.GetFiliereByCode(reader.GetInt32(3));
                    VacataireModel vacataire = new VacataireModel
                    {
                        Id = reader.GetInt32(0),
                        Nom = reader.GetString(1),
                        Specialite = reader.GetString(2),
                        CodeFiliere = reader.GetInt32(3),
                        Filiere = filiere
                    };

                    vacataires.Add(vacataire);
                }
            }

            return vacataires;
        }

        public IActionResult Index()
        {
            ViewBag.Filieres = _filiereController.GetFilieresFromDatabase();

            List<VacataireModel> vacataires = GetVacatairesFromDatabase();
            return View(vacataires);
        }

        private VacataireModel? GetVacataireById(int id)
        {
            using MySqlConnection mysqldbConnection = _dbConnection.CreateConnection();
            MySqlCommand sqlCommand = new MySqlCommand("SELECT * FROM vacantaires WHERE id = @id", mysqldbConnection);
            sqlCommand.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                FiliereModel? filiere = _filiereController.GetFiliereByCode(reader.GetInt32(3));

                VacataireModel vacataire = new VacataireModel
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Specialite = reader.GetString(2),
                    CodeFiliere = reader.GetInt32(3),
                    Filiere = filiere
                };

                return vacataire;
            }

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ajout(VacataireModel model)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO vacantaires (nom, specialite, code_filiere) VALUES (@nom, @specialite, @code_filiere)", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@nom", model.Nom);
                    sqlCommand.Parameters.AddWithValue("@specialite", model.Specialite);
                    sqlCommand.Parameters.AddWithValue("@code_filiere", model.CodeFiliere);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "Le vacataire a été ajouté avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de l'ajout du vacataire ❌ : {e.Message}";
            }

            List<VacataireModel> vacataires = GetVacatairesFromDatabase();
            return View("Index", vacataires);
        }

        public IActionResult Edit(int id)
        {
            VacataireModel? vacataire = GetVacataireById(id);

            if (vacataire == null)
            {
                return RedirectToAction("Index");
            }

            vacataire.Filieres = _filiereController.GetFilieresFromDatabase();

            return View(vacataire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VacataireModel model)
        {
            try
            {
                using (MySqlConnection mysqldbConnection = _dbConnection.CreateConnection())
                {
                    MySqlCommand sqlCommand = new MySqlCommand("UPDATE vacantaires SET nom = @nom, specialite = @specialite, code_filiere = @code_filiere WHERE id = @id", mysqldbConnection);
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
                    MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM vacantaires WHERE id = @id", mysqldbConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.ExecuteNonQuery();
                }

                ViewBag.modalMessage = "Le vacataire a été supprimé avec succès ✅";
            }
            catch (Exception e)
            {
                ViewBag.modalMessage = $"Une erreur est survenue lors de la suppression du vacataire ❌ : {e.Message}";
            }

            List<VacataireModel> vacataires = GetVacatairesFromDatabase();
            return View("Index", vacataires);
        }
    }
}
