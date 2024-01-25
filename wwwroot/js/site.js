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

// Sélectionne tous les boutons avec la classe "addFiliereModalBtn"
const addFiliereModalBtns = document.querySelectorAll('.addFiliereModalBtn');

addFiliereModalBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        const addFiliereModal = new bootstrap.Modal('#addFiliereModal');
        addFiliereModal.show();
    });
});


const addPerModalBtns = document.querySelectorAll('.addPerModalBtn');

addPerModalBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        const addPerModal = new bootstrap.Modal('#addPerModal');
        addPerModal.show();
    });
});


const addVacataireModalBtns = document.querySelectorAll('.addVacataireModalBtn');

addVacataireModalBtns.forEach(btn => {
    btn.addEventListener('click', () => {
        const addVacataireModal = new bootstrap.Modal('#addVacataireModal');
        addVacataireModal.show();
    });
});