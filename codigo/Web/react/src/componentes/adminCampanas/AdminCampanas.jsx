import React, { useState } from 'react';

const fmtFecha = (f) => f ? new Date(f).toLocaleString('es-CR') : '—';

const VACIO_OFERTA = { idOferta: '', asunto: '', contenido: '' };
const VACIO_DESCUENTO = { idDescuento: '', asunto: '', contenido: '' };

export default function AdminCampanas({ campanas: inicial, descuentos, ofertas, apiUrl }) {
    const [lista, setLista] = useState(inicial ?? []);
    const [tipo, setTipo] = useState('Oferta'); // 'Oferta' | 'Descuento'
    const [formOferta, setFormOferta] = useState(VACIO_OFERTA);
    const [formDescuento, setFormDescuento] = useState(VACIO_DESCUENTO);
    const [error, setError] = useState('');
    const [exito, setExito] = useState('');
    const [cargando, setCargando] = useState(false);

    const recargar = async () => {
        const r = await fetch(apiUrl);
        if (r.ok) setLista(await r.json());
    };

    const enviar = async (e) => {
        e.preventDefault();
        setError('');
        setExito('');

        if (tipo === 'Oferta') {
            if (!formOferta.idOferta) { setError('Seleccioná una oferta'); return; }
            if (!formOferta.asunto) { setError('El asunto es obligatorio'); return; }
        } else {
            if (!formDescuento.idDescuento) { setError('Seleccioná un descuento'); return; }
            if (!formDescuento.asunto) { setError('El asunto es obligatorio'); return; }
        }

        setCargando(true);
        try {
            const endpoint = tipo === 'Oferta' ? `${apiUrl}/enviar-oferta` : `${apiUrl}/enviar-descuento`;
            const body = tipo === 'Oferta' ? formOferta : formDescuento;

            const r = await fetch(endpoint, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });

            if (!r.ok) {
                const msg = await r.text();
                setError(msg || 'Error al enviar la campaña');
                return;
            }

            const resultado = await r.json();
            setExito(`Campaña enviada correctamente a ${resultado.cantidadDestinatarios} destinatario(s).`);
            setFormOferta(VACIO_OFERTA);
            setFormDescuento(VACIO_DESCUENTO);
            await recargar();
        } catch {
            setError('No se pudo conectar con la API.');
        } finally {
            setCargando(false);
        }
    };

    return (
        <div className="flex flex-col gap-8">
            {/* Header */}
            <div>
                <h1 className="font-headline-md text-headline-md text-coffee-bean">Campañas de marketing</h1>
                <p className="text-body-sm text-on-surface-variant mt-1">Enviá correos de ofertas o descuentos a los clientes suscritos</p>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                {/* Formulario envío */}
                <div className="bg-surface-container-lowest border border-outline-variant rounded-2xl p-6">
                    <h2 className="font-headline-sm text-headline-sm text-coffee-bean mb-4">Nueva campaña</h2>

                    {/* Selector tipo */}
                    <div className="flex gap-2 mb-6 p-1 bg-surface-container rounded-full w-fit">
                        {['Oferta', 'Descuento'].map(t => (
                            <button key={t} type="button"
                                onClick={() => { setTipo(t); setError(''); setExito(''); }}
                                className={`px-4 py-1.5 rounded-full text-label-bold font-label-bold transition-colors ${tipo === t ? 'bg-primary text-on-primary' : 'text-on-surface-variant hover:text-on-surface'}`}>
                                {t}
                            </button>
                        ))}
                    </div>

                    <form onSubmit={enviar} className="flex flex-col gap-4">
                        {error && <div className="bg-error-container text-on-error-container text-body-sm px-4 py-2 rounded-lg">{error}</div>}
                        {exito && <div className="bg-status-success/20 text-status-success text-body-sm px-4 py-2 rounded-lg">{exito}</div>}

                        {tipo === 'Oferta' ? (
                            <>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Oferta vigente</label>
                                    <select value={formOferta.idOferta}
                                        onChange={e => setFormOferta({ ...formOferta, idOferta: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary">
                                        <option value="">— Seleccioná una oferta —</option>
                                        {ofertas.map(o => <option key={o.id} value={o.id}>{o.nombre} ({o.tipoOferta})</option>)}
                                    </select>
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Asunto del correo</label>
                                    <input type="text" value={formOferta.asunto}
                                        onChange={e => setFormOferta({ ...formOferta, asunto: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary"
                                        placeholder="¡Nueva oferta en Isme Café!" />
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Mensaje adicional (opcional)</label>
                                    <textarea value={formOferta.contenido}
                                        onChange={e => setFormOferta({ ...formOferta, contenido: e.target.value })}
                                        rows={3}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary resize-none"
                                        placeholder="Texto introductorio del correo..." />
                                </div>
                            </>
                        ) : (
                            <>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Descuento vigente</label>
                                    <select value={formDescuento.idDescuento}
                                        onChange={e => setFormDescuento({ ...formDescuento, idDescuento: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary">
                                        <option value="">— Seleccioná un descuento —</option>
                                        {descuentos.map(d => <option key={d.id} value={d.id}>{d.nombreProducto} — {d.porcentajeDescuento}% off</option>)}
                                    </select>
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Asunto del correo</label>
                                    <input type="text" value={formDescuento.asunto}
                                        onChange={e => setFormDescuento({ ...formDescuento, asunto: e.target.value })}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary"
                                        placeholder="¡Descuento especial en Isme Café!" />
                                </div>
                                <div className="flex flex-col gap-1">
                                    <label className="text-label-bold font-label-bold text-on-surface-variant">Mensaje adicional (opcional)</label>
                                    <textarea value={formDescuento.contenido}
                                        onChange={e => setFormDescuento({ ...formDescuento, contenido: e.target.value })}
                                        rows={3}
                                        className="border border-outline-variant rounded-lg px-3 py-2 text-body-md bg-surface focus:outline-none focus:border-primary resize-none"
                                        placeholder="Texto introductorio del correo..." />
                                </div>
                            </>
                        )}

                        <button type="submit" disabled={cargando}
                            className="flex items-center justify-center gap-2 bg-primary text-on-primary px-4 py-2 rounded-full font-label-bold text-label-bold hover:bg-primary/90 transition-colors disabled:opacity-50 mt-2">
                            <span className="material-symbols-outlined" style={{ fontSize: '18px' }}>send</span>
                            {cargando ? 'Enviando...' : 'Enviar campaña'}
                        </button>
                    </form>
                </div>

                {/* Historial */}
                <div>
                    <h2 className="font-headline-sm text-headline-sm text-coffee-bean mb-4">Historial de envíos</h2>
                    {lista.length === 0 ? (
                        <div className="text-center py-12 text-on-surface-variant border border-outline-variant rounded-2xl">
                            <span className="material-symbols-outlined" style={{ fontSize: '48px' }}>mail</span>
                            <p className="mt-2 text-body-md">No hay campañas enviadas.</p>
                        </div>
                    ) : (
                        <div className="flex flex-col gap-3">
                            {lista.map((c) => (
                                <div key={c.id} className="bg-surface-container-lowest border border-outline-variant rounded-xl p-4">
                                    <div className="flex items-start justify-between gap-2">
                                        <div className="flex-1">
                                            <div className="flex items-center gap-2 mb-1">
                                                <span className={`px-2 py-0.5 rounded-full text-xs font-bold ${c.tipo === 'Oferta' ? 'bg-primary-container text-on-primary' : 'bg-secondary-container text-on-secondary-container'}`}>
                                                    {c.tipo}
                                                </span>
                                                {c.exitosa
                                                    ? <span className="bg-status-success/20 text-status-success px-2 py-0.5 rounded-full text-xs font-bold">Enviada</span>
                                                    : <span className="bg-error-container text-error px-2 py-0.5 rounded-full text-xs font-bold">Fallida</span>
                                                }
                                            </div>
                                            <p className="font-medium text-on-surface text-body-md">{c.asunto}</p>
                                            <p className="text-body-sm text-on-surface-variant mt-1">
                                                {fmtFecha(c.fechaEnvio)} · {c.cantidadDestinatarios} destinatario(s)
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}