import React, { useState } from 'react';

const fmt = new Intl.NumberFormat('es-CR', { style: 'currency', currency: 'CRC', minimumFractionDigits: 0 });
const fmtFecha = (f) => f ? new Date(f).toLocaleDateString('es-CR') : '—';

const VACIO = {
    idProducto: '',
    porcentajeDescuento: '',
    fechaInicio: '',
    fechaFin: '',
    activo: true,
};

export default function AdminDescuentos({ descuentos: inicial, productos, apiUrl }) {
    const [lista, setLista] = useState(inicial ?? []);
    const [modal, setModal] = useState(false);
    const [editando, setEditando] = useState(null); 
    const [form, setForm] = useState(VACIO);
    const [error, setError] = useState('');
    const [cargando, setCargando] = useState(false);
    const [confirmar, setConfirmar] = useState(null); 

    //Helpers
    const abrirCrear = () => {
        setForm(VACIO);
        setEditando(null);
        setError('');
        setModal(true);
    };

    const abrirEditar = (d) => {
        setForm({
            idProducto: d.idProducto ?? '',
            porcentajeDescuento: d.porcentajeDescuento,
            fechaInicio: d.fechaInicio?.substring(0, 16) ?? '',
            fechaFin: d.fechaFin?.substring(0, 16) ?? '',
            activo: d.activo,
        });
        setEditando(d.id);
        setError('');
        setModal(true);
    };

    const cerrarModal = () => { setModal(false); setError(''); };

    const recargar = async () => {
        const r = await fetch(apiUrl);
        if (r.ok) setLista(await r.json());
    };

    //Submit
    const guardar = async (e) => {
        e.preventDefault();
        if (!form.idProducto) { setError('Seleccioná un producto'); return; }
        if (!form.fechaInicio || !form.fechaFin) { setError('Las fechas son obligatorias'); return; }
        if (new Date(form.fechaInicio) >= new Date(form.fechaFin)) { setError('La fecha de inicio debe ser anterior a la de fin'); return; }

        setCargando(true);
        setError('');

        const body = {
            idProducto: form.idProducto,
            porcentajeDescuento: parseFloat(form.porcentajeDescuento),
            fechaInicio: form.fechaInicio,
            fechaFin: form.fechaFin,
            activo: form.activo,
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

    //Eliminar
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
    const esVigente = (d) => d.activo && new Date(d.fechaInicio) <= ahora && ahora <= new Date(d.fechaFin);

    //Render
    return (
        <div>
            {/* Header */}
            <div className="flex items-center justify-between mb-6">
                <div>
                    <h1 className="font-headline-md text-headline-md text-coffee-bean">Descuentos</h1>
                    <p className="text-body-sm text-on-surface-variant mt-1">Gestión de descuentos por producto</p>
                </div>
                <button onClick={abrirCrear}
                    className="flex items-center gap-2 bg-primary text-on-primary px-4 py-2 rounded-full font-label-bold text-label-bold hover:bg-primary/90 transition-colors">
                    <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>add</span>
                    Nuevo descuento
                </button>
            </div>

            {/*Tabla*/}
            {lista.length === 0 ? (
                <div className="text-center py-16 text-on-surface-variant">
                    <span className="material-symbols-outlined" style={{ fontSize: '48px' }}>sell</span>
                    <p className="mt-2 text-body-md">No hay descuentos registrados.</p>
                </div>
            ) : (
                <div className="overflow-x-auto rounded-xl border border-outline-variant">
                    <table className="w-full text-body-sm">
                        <thead className="bg-surface-container text-on-surface-variant font-label-bold text-label-bold">
                            <tr>
                                <th className="text-left px-4 py-3">Producto</th>
                                <th className="text-left px-4 py-3">Precio</th>
                                <th className="text-left px-4 py-3">Descuento</th>
                                <th className="text-left px-4 py-3">Precio final</th>
                                <th className="text-left px-4 py-3">Vigencia</th>
                                <th className="text-left px-4 py-3">Estado</th>
                                <th className="text-left px-4 py-3">Acciones</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-outline-variant">
                            {lista.map((d) => {
                                const precioFinal = d.precioProducto * (1 - d.porcentajeDescuento / 100);
                                return (
                                    <tr key={d.id} className="bg-surface-container-lowest hover:bg-surface-container-low transition-colors">
                                        <td className="px-4 py-3 font-medium text-coffee-bean">{d.nombreProducto}</td>
                                        <td className="px-4 py-3 text-on-surface-variant line-through">{fmt.format(d.precioProducto)}</td>
                                        <td className="px-4 py-3">
                                            <span className="bg-secondary-container text-on-secondary-container px-2 py-1 rounded-full text-xs font-bold">
                                                -{d.porcentajeDescuento}%
                                            </span>
                                        </td>
                                        <td className="px-4 py-3 font-bold text-primary">{fmt.format(precioFinal)}</td>
                                        <td className="px-4 py-3 text-on-surface-variant">
                                            {fmtFecha(d.fechaInicio)} → {fmtFecha(d.fechaFin)}
                                        </td>
                                        <td className="px-4 py-3">
                                            {esVigente(d)
                                                ? <span className="bg-status-success/20 text-status-success px-2 py-1 rounded-full text-xs font-bold">Vigente</span>
                                                : <span className="bg-surface-container text-on-surface-variant px-2 py-1 rounded-full text-xs font-bold">Inactivo</span>
                                            }
                                        </td>
                                        <td className="px-4 py-3">
                                            <div className="flex items-center gap-2">
                                                <button onClick={() => abrirEditar(d)}
                                                    className="p-1.5 rounded-lg text-on-surface-variant hover:bg-surface-container hover:text-primary transition-colors"
                                                    title="Editar">
                                                    <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>edit</span>
                                                </button>
                                                <button onClick={() => setConfirmar(d.id)}
                                                    className="p-1.5 rounded-lg text-on-surface-variant hover:bg-error-container hover:text-error transition-colors"
                                                    title="Desactivar">
                                                    <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>block</span>
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
            )}

            {/*Modal crear/editar*/}
            {modal && (
                <div className="fixed inset-0 bg-inverse-surface/50 flex items-center justify-center z-50 p-4">
                    <div className="bg-surface-container-lowest rounded-2xl shadow-xl w-full max-w-md">
                        <div className="flex items-center justify-between px-6 pt-6 pb-4 border-b border-outline-variant">
                            <h2 className="font-headline-sm text-headline-sm text-coffee-bean">
                                {editando ? 'Editar descuento' : 'Nuevo descuento'}
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
                                <label className="text-label-bold font-label-bold text-on-surface-variant">Producto</label>
                                <select value={form.idProducto}
                                    onChange={e => setForm({ ...form, idProducto: e.target.value })}
                                    className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary">
                                    <option value="">— Seleccioná un producto —</option>
                                    {productos.map(p => (
                                        <option key={p.id} value={p.id}>{p.nombre} — {fmt.format(p.precio)}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="flex flex-col gap-1">
                                <label className="text-label-bold font-label-bold text-on-surface-variant">Porcentaje de descuento (%)</label>
                                <input type="number" min="0.01" max="100" step="0.01"
                                    value={form.porcentajeDescuento}
                                    onChange={e => setForm({ ...form, porcentajeDescuento: e.target.value })}
                                    className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary"
                                    placeholder="Ej: 15" />
                            </div>

                            <div className="grid grid-cols-2 gap-3">
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Fecha inicio</label>
                                    <input type="datetime-local"
                                        value={form.fechaInicio}
                                        onChange={e => setForm({ ...form, fechaInicio: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-sm bg-surface focus:outline-none focus:border-primary" />
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Fecha fin</label>
                                    <input type="datetime-local"
                                        value={form.fechaFin}
                                        onChange={e => setForm({ ...form, fechaFin: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-sm bg-surface focus:outline-none focus:border-primary" />
                                </div>
                            </div>

                            <label className="flex items-center gap-2 cursor-pointer">
                                <input type="checkbox" checked={form.activo}
                                    onChange={e => setForm({ ...form, activo: e.target.checked })}
                                    className="w-4 h-4 accent-primary" />
                                <span className="text-body-md text-on-surface">Activo</span>
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

            {/*Modal confirmar desactivar*/}
            {confirmar && (
                <div className="fixed inset-0 bg-inverse-surface/50 flex items-center justify-center z-50 p-4">
                    <div className="bg-surface-container-lowest rounded-2xl shadow-xl w-full max-w-sm p-6">
                        <h2 className="font-headline-sm text-headline-sm text-coffee-bean mb-2">¿Desactivar descuento?</h2>
                        <p className="text-body-md text-on-surface-variant mb-6">Esta acción desactiva el descuento. Podés reactivarlo editándolo.</p>
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