// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Sélectionne tous les boutons avec la classe "addServiceModalBtn"
const addServiceModalBtns = document.querySelectorAll('.addServiceModalBtn');

addServiceModalBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        const addServiceModal = new bootstrap.Modal('#addServiceModal');
        addServiceModal.show();
    });
});



// Sélectionne tous les boutons avec la classe "addDepartementModalBtn"
const addDepartementModalBtns = document.querySelectorAll('.addDepartementModalBtn');

addDepartementModalBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        const addDepartementModal = new bootstrap.Modal('#addDepartementModal');
        addDepartementModal.show();
    });
});