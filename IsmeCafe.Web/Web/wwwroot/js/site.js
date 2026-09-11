// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Muestra/oculta un modal por su id 
function abrirModal(id) {
    var modal = document.getElementById(id);
    if (modal) modal.classList.remove('hidden');
}
function cerrarModal(id) {
    var modal = document.getElementById(id);
    if (modal) modal.classList.add('hidden');
}
