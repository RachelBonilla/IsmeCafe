import React, { useState } from 'react';

const fmt = new Intl.NumberFormat('es-CR', { style: 'currency', currency: 'CRC', minimumFractionDigits: 0 });
const fmtFecha = (f) => f ? new Date(f).toLocaleDateString('es-CR') : '—';

const TIPOS = ['2x1', '3x2', 'Combo', 'Personalizado'];

const VACIO = {
    nombre: '',
    descripcion: '',
    tipoOferta: '2x1',
    precioCombo: '',
    fechaInicio: '',
    fechaFin: '',
    activo: true,
    productos: [],
};

export default function AdminOfertas({ ofertas: inicial, productos, apiUrl }) {
    const [lista, setLista] = useState(inicial ?? []);
    const [modal, setModal] = useState(false);
    const [editando, setEditando] = useState(null);
    const [form, setForm] = useState(VACIO);
    const [error, setError] = useState('');
    const [cargando, setCargando] = useState(false);
    const [confirmar, setConfirmar] = useState(null);

    const abrirCrear = () => {
        setForm(VACIO);
        setEditando(null);
        setError('');
        setModal(true);
    };

    const abrirEditar = async (o) => {
        setCargando(true);
        try {
            const r = await fetch(`${apiUrl}/${o.id}`);
            const detalle = await r.json();
            setForm({
                nombre: detalle.nombre,
                descripcion: detalle.descripcion ?? '',
                tipoOferta: detalle.tipoOferta,
                precioCombo: detalle.precioCombo ?? '',
                fechaInicio: detalle.fechaInicio?.substring(0, 16) ?? '',
                fechaFin: detalle.fechaFin?.substring(0, 16) ?? '',
                activo: detalle.activo,
                productos: detalle.productos?.map(p => ({ idProducto: p.idProducto, cantidad: p.cantidad })) ?? [],
            });
            setEditando(o.id);
            setError('');
            setModal(true);
        } finally {
            setCargando(false);
        }
    };

    const cerrarModal = () => { setModal(false); setError(''); };

    const recargar = async () => {
        const r = await fetch(apiUrl);
        if (r.ok) setLista(await r.json());
    };

    const toggleProducto = (idProducto) => {
        const existe = form.productos.find(p => p.idProducto === idProducto);
        if (existe) {
            setForm({ ...form, productos: form.productos.filter(p => p.idProducto !== idProducto) });
        } else {
            setForm({ ...form, productos: [...form.productos, { idProducto, cantidad: 1 }] });
        }
    };

    const setCantidad = (idProducto, cantidad) => {
        setForm({
            ...form,
            productos: form.productos.map(p =>
                p.idProducto === idProducto ? { ...p, cantidad: parseInt(cantidad) || 1 } : p
            )
        });
    };

    const guardar = async (e) => {
        e.preventDefault();
        if (!form.nombre) { setError('El nombre es obligatorio'); return; }
        if (form.productos.length === 0) { setError('Agregá al menos un producto'); return; }
        if (!form.fechaInicio || !form.fechaFin) { setError('Las fechas son obligatorias'); return; }
        if (new Date(form.fechaInicio) >= new Date(form.fechaFin)) { setError('La fecha de inicio debe ser anterior a la de fin'); return; }

        setCargando(true);
        setError('');

        const body = {
            nombre: form.nombre,
            descripcion: form.descripcion,
            tipoOferta: form.tipoOferta,
            precioCombo: form.precioCombo ? parseFloat(form.precioCombo) : null,
            fechaInicio: form.fechaInicio,
            fechaFin: form.fechaFin,
            activo: form.activo,
            productos: form.productos,
        };

        try {
            const r = editando
                ? await fetch(`${apiUrl}/${editando}`, { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) })
                : await fetch(apiUrl, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });

            if (!r.ok) { setError('Error al guardar. Verificá los datos.'); return; }
            await recargar();
            cerrarModal();
        } catch {
            setError('No se pudo conectar con la API.');
        } finally {
            setCargando(false);
        }
    };

    const eliminar = async (id) => {
        setCargando(true);
        try {
            await fetch(`${apiUrl}/${id}`, { method: 'DELETE' });
            await recargar();
        } finally {
            setCargando(false);
            setConfirmar(null);
        }
    };

    const ahora = new Date();
    const esVigente = (o) => o.activo && new Date(o.fechaInicio) <= ahora && ahora <= new Date(o.fechaFin);

    return (
        <div>
            {/*Header*/}
            <div className="flex items-center justify-between mb-6">
                <div>
                    <h1 className="font-headline-md text-headline-md text-coffee-bean">Ofertas</h1>
                    <p className="text-body-sm text-on-surface-variant mt-1">Gestión de ofertas y combos</p>
                </div>
                <button onClick={abrirCrear}
                    className="flex items-center gap-2 bg-primary text-on-primary px-4 py-2 rounded-full font-label-bold text-label-bold hover:bg-primary/90 transition-colors">
                    <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>add</span>
                    Nueva oferta
                </button>
            </div>

            {/*Tabla*/}
            {lista.length === 0 ? (
                <div className="text-center py-16 text-on-surface-variant">
                    <span className="material-symbols-outlined" style={{ fontSize: '48px' }}>local_offer</span>
                    <p className="mt-2 text-body-md">No hay ofertas registradas.</p>
                </div>
            ) : (
                <div className="overflow-x-auto rounded-xl border border-outline-variant">
                    <table className="w-full text-body-sm">
                        <thead className="bg-surface-container text-on-surface-variant font-label-bold text-label-bold">
                            <tr>
                                <th className="text-left px-4 py-3">Nombre</th>
                                <th className="text-left px-4 py-3">Tipo</th>
                                <th className="text-left px-4 py-3">Precio combo</th>
                                <th className="text-left px-4 py-3">Vigencia</th>
                                <th className="text-left px-4 py-3">Estado</th>
                                <th className="text-left px-4 py-3">Acciones</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-outline-variant">
                            {lista.map((o) => (
                                <tr key={o.id} className="bg-surface-container-lowest hover:bg-surface-container-low transition-colors">
                                    <td className="px-4 py-3">
                                        <p className="font-medium text-coffee-bean">{o.nombre}</p>
                                        {o.descripcion && <p className="text-xs text-on-surface-variant">{o.descripcion}</p>}
                                    </td>
                                    <td className="px-4 py-3">
                                        <span className="bg-primary-container text-on-primary px-2 py-1 rounded-full text-xs font-bold">
                                            {o.tipoOferta}
                                        </span>
                                    </td>
                                    <td className="px-4 py-3 text-on-surface-variant">
                                        {o.precioCombo ? fmt.format(o.precioCombo) : '—'}
                                    </td>
                                    <td className="px-4 py-3 text-on-surface-variant">
                                        {fmtFecha(o.fechaInicio)} → {fmtFecha(o.fechaFin)}
                                    </td>
                                    <td className="px-4 py-3">
                                        {esVigente(o)
                                            ? <span className="bg-status-success/20 text-status-success px-2 py-1 rounded-full text-xs font-bold">Vigente</span>
                                            : <span className="bg-surface-container text-on-surface-variant px-2 py-1 rounded-full text-xs font-bold">Inactiva</span>
                                        }
                                    </td>
                                    <td className="px-4 py-3">
                                        <div className="flex items-center gap-2">
                                            <button onClick={() => abrirEditar(o)}
                                                className="p-1.5 rounded-lg text-on-surface-variant hover:bg-surface-container hover:text-primary transition-colors"
                                                title="Editar">
                                                <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>edit</span>
                                            </button>
                                            <button onClick={() => setConfirmar(o.id)}
                                                className="p-1.5 rounded-lg text-on-surface-variant hover:bg-error-container hover:text-error transition-colors"
                                                title="Desactivar">
                                                <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>block</span>
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}

            {/*Modal crear/editar*/}
            {modal && (
                <div className="fixed inset-0 bg-inverse-surface/50 flex items-center justify-center z-50 p-4">
                    <div className="bg-surface-container-lowest rounded-2xl shadow-xl w-full max-w-lg max-h-[90vh] overflow-y-auto">
                        <div className="flex items-center justify-between px-6 pt-6 pb-4 border-b border-outline-variant sticky top-0 bg-surface-container-lowest">
                            <h2 className="font-headline-sm text-headline-sm text-coffee-bean">
                                {editando ? 'Editar oferta' : 'Nueva oferta'}
                            </h2>
                            <button onClick={cerrarModal} className="text-on-surface-variant hover:text-primary transition-colors">
                                <span className="material-symbols-outlined">close</span>
                            </button>
                        </div>

                        <form onSubmit={guardar} className="px-6 py-4 flex flex-col gap-4">
                            {error && (
                                <div className="bg-error-container text-on-error-container text-body-sm px-4 py-2 rounded-lg">{error}</div>
                            )}

                            <div className="flex flex-col gap-1">
                                <label className="text-label-bold font-label-bold text-on-surface-variant">Nombre</label>
                                <input type="text" value={form.nombre}
                                    onChange={e => setForm({ ...form, nombre: e.target.value })}
                                    className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary"
                                    placeholder="Ej: 2x1 Café de altura" />
                            </div>

                            <div className="flex flex-col gap-1">
                                <label className="text-label-bold font-label-bold text-on-surface-variant">Descripción</label>
                                <textarea value={form.descripcion}
                                    onChange={e => setForm({ ...form, descripcion: e.target.value })}
                                    rows={2}
                                    className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary resize-none"
                                    placeholder="Descripción opcional" />
                            </div>

                            <div className="grid grid-cols-2 gap-3">
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Tipo de oferta</label>
                                    <select value={form.tipoOferta}
                                        onChange={e => setForm({ ...form, tipoOferta: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary">
                                        {TIPOS.map(t => <option key={t} value={t}>{t}</option>)}
                                    </select>
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Precio combo (opcional)</label>
                                    <input type="number" min="0" step="1" value={form.precioCombo}
                                        onChange={e => setForm({ ...form, precioCombo: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary"
                                        placeholder="₡0" />
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-3">
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Fecha inicio</label>
                                    <input type="datetime-local" value={form.fechaInicio}
                                        onChange={e => setForm({ ...form, fechaInicio: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-sm bg-surface focus:outline-none focus:border-primary" />
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Fecha fin</label>
                                    <input type="datetime-local" value={form.fechaFin}
                                        onChange={e => setForm({ ...form, fechaFin: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-sm bg-surface focus:outline-none focus:border-primary" />
                                </div>
                            </div>

                            {/*Productos*/}
                            <div className="flex flex-col gap-2">
                                <label className="text-label-bold font-label-bold text-on-surface-variant">
                                    Productos incluidos ({form.productos.length} seleccionados)
                                </label>
                                <div className="border border-outline-variant rounded-lg divide-y divide-outline-variant max-h-48 overflow-y-auto">
                                    {productos.map(p => {
                                        const sel = form.productos.find(fp => fp.idProducto === p.id);
                                        return (
                                            <div key={p.id} className={`flex items-center justify-between px-3 py-2 transition-colors ${sel ? 'bg-secondary-container/30' : 'bg-surface'}`}>
                                                <label className="flex items-center gap-2 cursor-pointer flex-1">
                                                    <input type="checkbox" checked={!!sel}
                                                        onChange={() => toggleProducto(p.id)}
                                                        className="w-4 h-4 accent-primary" />
                                                    <span className="text-body-sm text-on-surface">{p.nombre}</span>
                                                    <span className="text-xs text-on-surface-variant">{fmt.format(p.precio)}</span>
                                                </label>
                                                {sel && (
                                                    <div className="flex items-center gap-1">
                                                        <span className="text-xs text-on-surface-variant">Cant:</span>
                                                        <input type="number" min="1" max="99" value={sel.cantidad}
                                                            onChange={e => setCantidad(p.id, e.target.value)}
                                                            className="w-14 border border-outline-variant rounded px-2 py-0.5 text-body-sm bg-surface focus:outline-none focus:border-primary" />
                                                    </div>
                                                )}
                                            </div>
                                        );
                                    })}
                                </div>
                            </div>

                            <label className="flex items-center gap-2 cursor-pointer">
                                <input type="checkbox" checked={form.activo}
                                    onChange={e => setForm({ ...form, activo: e.target.checked })}
                                    className="w-4 h-4 accent-primary" />
                                <span className="text-body-md text-on-surface">Activa</span>
                            </label>

                            <div className="flex justify-end gap-3 pt-2 border-t border-outline-variant">
                                <button type="button" onClick={cerrarModal}
                                    className="px-4 py-2 rounded-full text-label-bold font-label-bold text-on-surface-variant hover:bg-surface-container transition-colors">
                                    Cancelar
                                </button>
                                <button type="submit" disabled={cargando}
                                    className="px-4 py-2 rounded-full text-label-bold font-label-bold bg-primary text-on-primary hover:bg-primary/90 transition-colors disabled:opacity-50">
                                    {cargando ? 'Guardando...' : 'Guardar'}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/*Modal confirmar*/}
            {confirmar && (
                <div className="fixed inset-0 bg-inverse-surface/50 flex items-center justify-center z-50 p-4">
                    <div className="bg-surface-container-lowest rounded-2xl shadow-xl w-full max-w-sm p-6">
                        <h2 className="font-headline-sm text-headline-sm text-coffee-bean mb-2">¿Desactivar oferta?</h2>
                        <p className="text-body-md text-on-surface-variant mb-6">Esta acción desactiva la oferta. Podés reactivarla editándola.</p>
                        <div className="flex justify-end gap-3">
                            <button onClick={() => setConfirmar(null)}
                                className="px-4 py-2 rounded-full text-label-bold font-label-bold text-on-surface-variant hover:bg-surface-container transition-colors">
                                Cancelar
                            </button>
                            <button onClick={() => eliminar(confirmar)} disabled={cargando}
                                className="px-4 py-2 rounded-full text-label-bold font-label-bold bg-error text-on-error hover:bg-error/90 transition-colors disabled:opacity-50">
                                {cargando ? 'Desactivando...' : 'Desactivar'}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}