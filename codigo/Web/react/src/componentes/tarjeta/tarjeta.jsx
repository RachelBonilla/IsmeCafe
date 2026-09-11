import React from 'react';
import './styles.css';

const formateadorColones = new Intl.NumberFormat('es-CR', {
  style: 'currency',
  currency: 'CRC',
  minimumFractionDigits: 0,
});


export default function Tarjeta({ producto }) {
    const agotado = !(producto.cantidad > 0);
    const tieneDescuento = producto.porcentajeDescuento != null && producto.precioConDescuento != null;

  return (
    <article
      className={`bg-surface-container-lowest border border-outline-variant rounded-lg overflow-hidden flex flex-col h-full transition-all duration-300 ${
        agotado ? 'opacity-75' : 'product-card-hover'
      }`}
    >
      <div className={`relative h-64 bg-surface-container-low overflow-hidden ${agotado ? 'grayscale' : ''}`}>
        {producto.imagen ? (
          <img src={producto.imagen} alt={producto.nombre} className="w-full h-full object-cover" />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-outline">
            <span className="material-symbols-outlined" style={{ fontSize: '56px' }}>
              local_cafe
            </span>
          </div>
        )}

        {producto.categoria ? (
          <div className="absolute top-3 left-3">
            <span className="bg-primary/80 text-on-primary text-[10px] font-bold px-2 py-1 rounded uppercase tracking-wider backdrop-blur-sm">
              {producto.categoria}
            </span>
          </div>
              ) : null}

            
              {tieneDescuento && !agotado ? (
                  <div className="absolute top-3 right-3">
                      <span className="bg-secondary text-on-secondary text-[11px] font-bold px-2 py-1 rounded-full shadow-sm">
                          -{producto.porcentajeDescuento}%
                      </span>
                  </div>
              ) : null}

        {agotado ? (
          <div className="absolute inset-0 bg-inverse-surface/40 flex items-center justify-center backdrop-blur-[2px]">
            <span className="bg-surface text-on-surface px-4 py-2 rounded-full font-label-bold text-label-bold">
              Agotado Temporalmente
            </span>
          </div>
        ) : null}
      </div>

      <div className="p-5 flex flex-col flex-grow">
        <div className="flex justify-between items-start mb-2">
          <h3 className="font-headline-sm text-headline-sm text-coffee-bean line-clamp-2">{producto.nombre}</h3>
          <button className="text-on-surface-variant hover:text-gold-accent transition-colors" aria-label="Favorito">
            <span className="material-symbols-outlined">favorite_border</span>
          </button>
        </div>

        {producto.descripcion ? (
          <p className="text-body-sm text-on-surface-variant mb-4 flex-grow line-clamp-3">{producto.descripcion}</p>
        ) : (
          <div className="flex-grow" />
        )}

        <div className="flex items-center justify-between mt-auto pt-4 border-t border-outline-variant/30">
                  <div className="flex flex-col">
                      {tieneDescuento ? (
                          <>
                              <span className="text-xs text-on-surface-variant line-through">
                                  {formateadorColones.format(producto.precio)}
                              </span>
                              <span className="font-label-bold text-label-bold text-lg text-secondary">
                                  {formateadorColones.format(producto.precioConDescuento)}
                              </span>
                          </>
                      ) : (
                          <span className={`font-label-bold text-label-bold text-lg ${agotado ? 'text-on-surface-variant' : 'text-primary'}`}>
                              {formateadorColones.format(producto.precio)}
                          </span>
                      )}
                  </div>

          {agotado ? (
            <button
              disabled
              aria-label="Agotado"
              className="flex items-center justify-center bg-surface-container-high text-on-surface-variant/50 w-10 h-10 rounded-full cursor-not-allowed"
            >
              <span className="material-symbols-outlined">add_shopping_cart</span>
            </button>
          ) : (
            <button
              aria-label="Agregar al carrito"
              className="flex items-center justify-center bg-secondary hover:bg-secondary/90 text-on-secondary w-10 h-10 rounded-full transition-colors shadow-sm"
            >
              <span className="material-symbols-outlined" style={{ fontVariationSettings: "'FILL' 1" }}>
                add_shopping_cart
              </span>
            </button>
          )}
        </div>
      </div>
    </article>
  );
}
