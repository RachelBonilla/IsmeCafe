[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/BB1zr2w8)
# Proyecto Final - Indicaciones Generales


## 1. Índice
1. Índice
2. Instrucciones generales
3. Proyecto
4. Parte 1: Anteproyecto
5. Parte 2: Avance 1 programado del proyecto
6. Parte 3: Avance 2 (Final) programado del proyecto
7. Detalle del puntaje

## 2. Instrucciones generales

El presente documento define las pautas para la elaboración del primer avance del Proyecto del curso Programación Avanzada Web (SC-701)

Esta evaluación está incluida dentro de las evaluaciones de la Directriz sobre Honestidad Académica, presentada y aceptada en el Programa del Curso, el incumplimiento con la directriz mencionada generará la aplicación correspondiente del artículo 31 del reglamento estudiantil vigente.

La entrega del proyecto se debe realizar de la siguiente manera:
- Indicar el url donde está ubicado el código completo a la sección correspondiente en el campus virtual
- Todo el código del proyecto debe estar en GitHub en la asignación correspondiente en el classroom
- Es indispensable que el código compile sin errores para poder evaluarlo

## 3. Proyecto

Se requiere construir un sistema web de acuerdo con la propuesta presentada por los estudiantes y aprobada por el profesor, la propuesta debe poseer las funcionalidades suficientes para que cada integrante sea responsable de al menos un conjunto de operaciones CRUD.

### 3.1. Componentes tecnológicos requeridos

La solución debe utilizar los siguientes componentes tecnológicos:
- .NET 8 o superior
- Lenguaje de Programación C#
- Web API basada en Controladores
- Dapper o EF para la gestión del modelo de datos
- Swagger para la documentación y uso de la aplicación
- Razor Pages (.NET 8 o superior) o React (17.0 o superior)
- Principios SOLID y Clean Architecture

### 3.2. Arquitectura de la aplicación

La aplicación debe cumplir con la siguiente arquitectura de componentes:

| Proyecto | Tipo | Descripción | Observaciones |
|----------|------|-------------|---------------|
| Abstracciones | Biblioteca de clases | Encargado de definir los modelos e interfaces | Requerido |
| DA | Biblioteca de clases | Encargada del acceso a la base de datos | Requerido |
| Flujo | Biblioteca de clases | Encargado de dirigir el flujo de información entre las capas | Requerido |
| Servicios | Biblioteca de clases | Encargado gestionar el consumo de servicios externos | Requerido en caso de requerir consumir servicios externos |
| Reglas | Biblioteca de clases | Encargado de la aplicación de las reglas de negocio | Opcional |
| API | ASP.NET Core Web API | Encargado de gestionar las solicitudes y respuestas HTTP | Requerido |
| Interweb MVC o SPA | Aplicación Web | Interfaz de usuario | Requerido |

### 3.3. Prácticas de desarrollo

Se debe realizar la aplicación siguiendo prácticas de desarrollo de programación orientada a objetos que permiten que el código sea fácil de leer, mantener y probar (Se puede tomar de referencia los principios SOLID), en todo momento se debe hacer uso de interfaz e inyección de dependencias.

### 3.4. Conocimientos por aplicar

**Tabla: Conocimientos por aplicar**

| Semana | Temas |
|--------|-------|
| 2 | Response, Status, Headers, Request, Postman, Get, Post |
| 3 | Parámetros, Restricciones de enrutamiento, Web root y archivos estáticos |
| 4 | Creación de controles, Action Methods, ContentResult, JsonResult, IActionResult, Redireccionamiento |
| 5 | Introducción, Model Class, Model Validations, Validaciones Personalizadas, Bind vs BindNever |
| 6 | Estudio Práctico |
| 7 | Layout Views para uso singular o múltiple, ViewData, Vistas Dinámicas, Vistas Anidadas, Vistas Parciales, Vistas Parciales con ViewData |
| 8 | FromService, Transient/Scoped/Singleton, View Injection, Excepciones y excepciones personalizadas |
| 9 | Configuración de inicio, Tag Helper, IConfiguration, Configuraciones como servicio, Configuración con JSON |
| 10 | Interfaz de usuario, Vista de listas (buscar, ordenar), Vistas de creación |
| 11 | DbContext, DbSet, Cadenas de Conexión, Migraciones, CRUD, Procedimientos Almacenados |

## 4. Parte 1: Anteproyecto

Elaboración de una propuesta que atienda la necesidad planteada mediante el uso de herramientas tecnológicas (ver componentes tecnológicos a utilizar).

### Entregables

Documento en formato IEEE que incluya:
- Objetivo
- Diagrama de base de datos (Entidad - Relación)
- Diagrama de casos de uso
- Historias de usuario
- Prototipo visual de la aplicación (en cualquier formato, Figma, PowerPoint, etc)

## 5. Parte 2: Avance 1 programado del proyecto

Se debe presentar un avance del funcionamiento alcanzado hasta el momento (al menos un 50% del proyecto), se busca un balance en términos de diseño, desarrollo y base de datos.

Se determinará el avance de al menos el 50% en los siguientes términos:
- 50% de las historias propuestas finalizadas

El criterio para determinar si una historia está finalizada es siguiente:
- La funcionalidad está implementada en un RestAPI
- La funcionalidad cuenta con una interfaz web en Razor o React la cual todas las acciones las realiza invocando EndPoints expuestos por el Rest API

### 5.1. Detalle de los requerimientos técnicos

#### 5.1.1. Base de datos
- Base de datos de tipo relacional
- Normalizada hasta la Tercera forma normal
- Uso de Procedimientos almacenados
- Tablas relacionadas entre sí

#### 5.1.2. RestAPI

Implementada mediante el uso de arquitectura limpia, debe poseer al menos los siguientes proyectos:

| Proyecto | Tipo | Descripción | Observaciones |
|----------|------|-------------|---------------|
| Abstracciones | Biblioteca de clases | Encargado de definir los modelos e interfaces | Requerido |
| DA | Biblioteca de clases | Encargada del acceso a la base de datos | Requerido |
| Flujo | Biblioteca de clases | Encargado de dirigir el flujo de información entre las capas | Requerido |
| Servicios | Biblioteca de clases | Encargado gestionar el consumo de servicios externos | Requerido en caso de requerir consumir servicios externos |
| Reglas | Biblioteca de clases | Encargado de la aplicación de las reglas de negocio | Opcional |
| API | ASP.NET Core Web API | Encargado de gestionar las solicitudes y respuestas HTTP | Requerido |

**Consideraciones para la implementación:**

- Toda clase no estática debe poseer una interfaz
- Uso correcto de parámetros, indicando el atributo correcto para la obtención de la información (FromHeader, FromBody, FromRoute, etc.)
- Uso correcto del método a utilizar (GET, POST, PUT, etc.)
- Todas las validaciones se deben realizar mediante el uso de anotaciones en los modelos
- Establecer Routes en cada EndPoint que faciliten el uso del API
- Las respuestas de los modelos deben ser por medio de action methods y el contenido debe estar en formato JSON
- Uso correcto de códigos HTTP en las respuestas de los métodos
- Uso de métodos asíncronos
- No realizar instancias de implementaciones concretas de objetos con excepción de las clases estáticas
- Hacer referencia a las abstracciones en forma de Interfaces por medio de la inyección de dependencias
- Todos los parámetros de configuración como cadenas de conexión, rutas o urls deben ser gestionados utilizando el appsettings

#### 5.1.3. Interfaz Web

Implementada mediante el uso de arquitectura limpia, debe poseer al menos los siguientes proyectos:

| Proyecto | Tipo | Descripción | Observaciones |
|----------|------|-------------|---------------|
| Abstracciones | Biblioteca de clases | Encargado de definir los modelos e interfaces | Requerido |
| Web App | ASP.NET Core Web APP (Razor Pages) o React | Encargado de implementar la interfaz web mediante los llamados al API RestFul | Requerido |

**Consideraciones para la interfaz web:**

- Solución aparte al Web API
- Responsable únicamente de presentar la información y solicitar su modificación al API Rest
- No debe transformar la información o implementar lógica de negocio
- Comunicación con el Rest API por medio del protocolo HTTP
- Validaciones en los formularios de entrada de datos mediante anotaciones en los modelos
- Validar: tipo de datos, longitud, formato, si es requerido y reglas de negocio
- El diseño visual debe ser responsivo y acorde a lo presentado en el prototipo
- Manejo de la información en las vistas por medio de Tag Helper
- Vistas separadas por acción
- Uso de métodos asíncronos

## 6. Parte 3: Avance 2 (Final) programado del proyecto

Se debe presentar el funcionamiento completo del proyecto, se busca un balance en términos de diseño, desarrollo y base de datos.

Se determinará el avance del 100% en los siguientes términos:
- 100% de las historias propuestas finalizadas

El criterio para determinar si una historia está finalizada es siguiente:
- La funcionalidad está implementada en un RestAPI
- La funcionalidad cuenta con una interfaz web en Razor o React la cual todas las acciones las realiza invocando EndPoints expuestos por el Rest API

### 6.1. Detalle de los requerimientos técnicos

Los requerimientos técnicos son los mismos que para el Avance 1, pero aplicados al 100% de las funcionalidades del proyecto.

#### 6.1.1. Base de datos
- Base de datos de tipo relacional
- Normalizada hasta la Tercera forma normal
- Uso de Procedimientos almacenados
- Tablas relacionadas entre sí

#### 6.1.2. RestAPI
- Mismas especificaciones del Avance 1, aplicadas a todas las funcionalidades

#### 6.1.3. Interfaz Web
- Mismas especificaciones del Avance 1, aplicadas a todas las funcionalidades

## 7. Detalle del puntaje

**Tabla 1: Rúbrica de evaluación**

| Rubro | 0% Cumplió de forma deficiente | 50% Cumplió de forma Regular | 75% Cumplió de forma buena | 100% Cumplió de forma Excelente |
|-------|-------------------------------|------------------------------|---------------------------|--------------------------------|
| Parte 1: Propuesta | Documento con 2 o menos de los componentes requeridos o mal implementados | Se incluye el documento completo pero algunas secciones no corresponden a lo indicado | Documento completo y secciones correspondientes a lo indicado | Documento completo y secciones correspondientes a lo indicado y tienen coherencia entre sí |
| Parte 2 y 3: Requerimientos no funcionales | Cumplimiento de menos del 40% de los requerimientos | Cumplimiento de más del 40% de los requerimientos | Cumplimiento de más del 70% de los requerimientos | Cumplimiento del 100% de los requerimientos |
| Parte 2 y 3: Funcionales | Menos de 33% de las funcionalidades cumplen con lo indicado en las historias de usuario | Al menos un 50% de las funcionalidades cumplen con lo indicado en las historias de usuario | Al menos un 75% de las funcionalidades cumplen con lo indicado en las historias de usuario | El 100% de las funcionalidades cumplen con lo indicado en las historias de usuario |

**Tabla 2: Detalle de puntaje**

| Funcionalidad | Puntos |
|---------------|--------|
| Parte 1: Anteproyecto | 10% |
| Parte 2: Avance 1 | 15% |
| Parte 3: Avance 2 | 25% |
| **Total** | **50%** |
