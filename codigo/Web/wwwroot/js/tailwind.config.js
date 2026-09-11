// Configuración del tema de la marca (Isme Café) para Tailwind CDN.
// Debe cargarse después de https://cdn.tailwindcss.com (usa el global `tailwind`).
tailwind.config = {
    darkMode: 'class',
    theme: {
        extend: {
            colors: {
                'status-success': '#3E5C3E', 'inverse-surface': '#33302f', 'secondary': '#745b18',
                'inverse-on-surface': '#f6efed', 'error': '#ba1a1a', 'on-secondary': '#ffffff',
                'surface-bright': '#fff8f6', 'surface-container': '#f3ecea', 'on-tertiary': '#ffffff',
                'on-error-container': '#93000a', 'on-surface': '#1e1b1a', 'on-error': '#ffffff',
                'tertiary': '#040402', 'on-surface-variant': '#4f4540', 'earth-cream': '#EBE3D5',
                'surface-container-low': '#f9f2f0', 'coffee-bean': '#3D2B1F', 'secondary-fixed-dim': '#e5c275',
                'surface-container-lowest': '#ffffff', 'on-primary': '#ffffff', 'surface': '#fff8f6',
                'surface-container-highest': '#e8e1df', 'primary-container': '#2b1a12', 'secondary-container': '#fcd989',
                'inverse-primary': '#dfc0b3', 'status-warning': '#B48924', 'outline-variant': '#d3c3bd',
                'gold-accent': '#D4AF37', 'status-error': '#8E3232', 'surface-container-high': '#eee7e5',
                'surface-tint': '#71594f', 'outline': '#81746f', 'on-secondary-container': '#775d1a',
                'surface-dim': '#dfd9d7', 'primary': '#0c0301', 'background': '#fff8f6',
                'deep-black': '#121212', 'surface-variant': '#e8e1df', 'primary-fixed': '#fcdcce',
                'on-background': '#1e1b1a', 'error-container': '#ffdad6'
            },
            borderRadius: { 'DEFAULT': '0.25rem', 'lg': '0.5rem', 'xl': '0.75rem', 'full': '9999px' },
            spacing: { 'container-max': '1280px', 'gutter': '1.5rem', 'margin-mobile': '1rem',
                'stack-sm': '0.5rem', 'stack-md': '1.5rem', 'stack-lg': '4rem' },
            maxWidth: { 'container-max': '1280px' },
            fontFamily: {
                'label-bold': ['Hanken Grotesk'], 'body-md': ['Hanken Grotesk'], 'body-lg': ['Hanken Grotesk'],
                'headline-lg': ['EB Garamond'], 'headline-sm': ['EB Garamond'], 'display-hero': ['EB Garamond'],
                'data-table': ['Hanken Grotesk'], 'body-sm': ['Hanken Grotesk'], 'headline-md': ['EB Garamond']
            },
            fontSize: {
                'label-bold': ['14px', { lineHeight: '1.2', letterSpacing: '0.05em', fontWeight: '700' }],
                'body-md': ['16px', { lineHeight: '1.6', fontWeight: '400' }],
                'body-lg': ['18px', { lineHeight: '1.6', fontWeight: '400' }],
                'headline-lg': ['40px', { lineHeight: '1.2', fontWeight: '600' }],
                'headline-sm': ['24px', { lineHeight: '1.4', fontWeight: '500' }],
                'display-hero': ['64px', { lineHeight: '1.1', letterSpacing: '-0.02em', fontWeight: '600' }],
                'body-sm': ['14px', { lineHeight: '1.5', fontWeight: '400' }],
                'headline-md': ['32px', { lineHeight: '1.3', fontWeight: '500' }]
            }
        }
    }
};
