
(function () {
    'use strict';

    // -----------------------------------------------------------------
    // Busqueda en vivo
    // -----------------------------------------------------------------
    function iniciarBusqueda() {
        document.querySelectorAll('[data-reportes-buscar]').forEach(function (campo) {
            var tabla = document.getElementById(campo.dataset.reportesTabla);
            if (!tabla) return;

            var contenedor = tabla.parentElement;
            var vacio = contenedor
                ? contenedor.querySelector('[data-reportes-sin-resultados]')
                : null;

            campo.addEventListener('input', function () {
                var texto = campo.value.trim().toLowerCase();
                var visibles = 0;

                tabla.tBodies[0].querySelectorAll('tr').forEach(function (fila) {
                    var coincide = texto === '' ||
                        fila.textContent.toLowerCase().indexOf(texto) !== -1;

                    fila.hidden = !coincide;
                    if (coincide) visibles++;
                });

                if (vacio) vacio.hidden = visibles !== 0;
            });
        });
    }

    // -----------------------------------------------------------------
    // Ordenamiento por columna
    // -----------------------------------------------------------------
    function valorDeCelda(celda, tipo) {
        if (!celda) return tipo === 'numero' ? 0 : '';

        // Las celdas numericas llevan el valor crudo en data-valor para
        // no depender del formato de moneda.
        if (tipo === 'numero') {
            var crudo = celda.dataset.valor !== undefined
                ? celda.dataset.valor
                : celda.textContent.replace(/[^0-9,.-]/g, '').replace(/,/g, '');

            var numero = parseFloat(crudo);
            return isNaN(numero) ? 0 : numero;
        }

        return celda.textContent.trim().toLowerCase();
    }

    function ordenarTabla(tabla, indice, tipo, ascendente) {
        var cuerpo = tabla.tBodies[0];
        var filas = Array.prototype.slice.call(cuerpo.querySelectorAll('tr'));

        filas.sort(function (a, b) {
            var va = valorDeCelda(a.cells[indice], tipo);
            var vb = valorDeCelda(b.cells[indice], tipo);

            if (va < vb) return ascendente ? -1 : 1;
            if (va > vb) return ascendente ? 1 : -1;
            return 0;
        });

        filas.forEach(function (fila) { cuerpo.appendChild(fila); });
    }

    function iniciarOrdenamiento() {
        document.querySelectorAll('.reportes-tabla').forEach(function (tabla) {
            if (!tabla.tBodies.length) return;

            tabla.querySelectorAll('th[data-reportes-orden]').forEach(function (encabezado) {
                // cellIndex da la posicion real de la columna, sin asumir que
                // todos los encabezados sean ordenables.
                var indice = encabezado.cellIndex;

                encabezado.setAttribute('role', 'button');
                encabezado.setAttribute('tabindex', '0');

                function alternar() {
                    var ascendente = encabezado.dataset.ordenActivo !== 'asc';

                    tabla.querySelectorAll('th[data-reportes-orden]').forEach(function (otro) {
                        delete otro.dataset.ordenActivo;
                    });

                    encabezado.dataset.ordenActivo = ascendente ? 'asc' : 'desc';
                    ordenarTabla(tabla, indice, encabezado.dataset.reportesOrden, ascendente);
                }

                encabezado.addEventListener('click', alternar);
                encabezado.addEventListener('keydown', function (evento) {
                    if (evento.key === 'Enter' || evento.key === ' ') {
                        evento.preventDefault();
                        alternar();
                    }
                });
            });
        });
    }

    // -----------------------------------------------------------------
    // validacion del rango de fechas
    // -----------------------------------------------------------------
    function iniciarValidacionFechas() {
        var formulario = document.querySelector('[data-reportes-form-ventas]');
        if (!formulario) return;

        var inicio = formulario.querySelector('[data-reportes-fecha-inicio]');
        var fin = formulario.querySelector('[data-reportes-fecha-fin]');
        var error = formulario.querySelector('[data-reportes-error-fechas]');
        if (!inicio || !fin) return;

        function rangoInvalido() {
            return inicio.value !== '' && fin.value !== '' && inicio.value > fin.value;
        }

        function revisar() {
            var invalido = rangoInvalido();
            if (error) error.hidden = !invalido;
            inicio.setCustomValidity(invalido ? 'El rango de fechas no es válido.' : '');
            return !invalido;
        }

        inicio.addEventListener('change', revisar);
        fin.addEventListener('change', revisar);

        formulario.addEventListener('submit', function (evento) {
            if (!revisar()) {
                evento.preventDefault();
                inicio.focus();
            }
        });
    }

    function iniciar() {
        iniciarBusqueda();
        iniciarOrdenamiento();
        iniciarValidacionFechas();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', iniciar);
    } else {
        iniciar();
    }
})();
