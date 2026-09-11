import React from 'react';
import { createRoot } from 'react-dom/client';
import Tarjeta from './componentes/tarjeta/tarjeta.jsx';
import AdminDescuentos from './componentes/adminDescuentos/AdminDescuentos.jsx';
import AdminOfertas from './componentes/adminOfertas/AdminOfertas.jsx';
import AdminCampanas from './componentes/adminCampanas/AdminCampanas.jsx';

const registry = {
    tarjeta: Tarjeta,
    adminDescuentos: AdminDescuentos,
    adminOfertas: AdminOfertas,
    adminCampanas: AdminCampanas,
};

function mountAll() {
    document.querySelectorAll('[data-react]').forEach((el) => {
        if (el.dataset.reactMounted === 'true') return;

        const nombre = el.dataset.react;
        const Componente = registry[nombre];
        if (!Componente) {
            console.warn(`[react] No existe un componente registrado con el nombre "${nombre}".`);
            return;
        }

        let props = {};
        if (el.dataset.props) {
            try {
                props = JSON.parse(el.dataset.props);
            } catch (e) {
                console.error(`[react] data-props inválido en el componente "${nombre}"`, e);
            }
        }

        el.dataset.reactMounted = 'true';
        createRoot(el).render(<Componente {...props} />);
    });
}

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', mountAll);
} else {
    mountAll();
}