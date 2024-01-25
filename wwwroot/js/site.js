// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const addServiceModalBtn = document.getElementById('addServiceModalBtn');

addServiceModalBtn.addEventListener('click', () => {
    const addServiceModal = new bootstrap.Modal('#addServiceModal');
    addServiceModal.show();
});