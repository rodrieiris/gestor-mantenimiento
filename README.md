# Gestor de Mantenimiento Preventivo Industrial
## Descripción del Proyecto
Este proyecto consiste en un sistema para la gestión del mantenimiento preventivo de maquinaria industrial. Incluye:

* **Backend**: API REST construida con .NET 8 y Entity Framework Core sobre SQLite.
* **Frontend**: SPA Angular 20 con autenticación JWT, roles y diseño moderno con SCSS.

## Características Principales
* Gestión de **Máquinas**: CRUD de máquinas con nombre, número de serie, fecha de instalación y estado.
* Gestión de **Tareas de Mantenimiento**: CRUD de tareas, marcación de completadas.
* **Dashboard** de estadísticas: total de tareas y conteos por estado.
* **Autenticación JWT**: login con roles (`admin` y `tech`) y rutas protegidas.
* **Route Guards** en Angular para restringir accesos según rol.
* Diseño responsivo y moderno usando SCSS y CSS variables.

## Tecnologías Utilizadas
* **Backend**:
  * .NET 8
  * C#
  * Entity Framework Core (SQLite)
  * JWT para autenticación

* **Frontend**:
  * Angular 20 (Standalone Components)
  * TypeScript
  * SCSS

* **Herramientas**:
  * Angular CLI
  * dotnet CLI
  * Visual Studio Code

## Requisitos Previos
* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [Node.js 18+ y npm](https://nodejs.org/)
* [Angular CLI 20.x](https://angular.io/cli)

## Instalación y Ejecución en Local
Sigue estos pasos para levantar el proyecto completo en tu máquina:

### 1. Clonar el repositorio
```bash
git clone git@github.com:rodrieiris/gestor-mantenimiento.git
cd gestor-mantenimiento
```

### 2. Configurar y ejecutar el Backend
```bash
cd GestorMantenimiento.API
# Restaurar paquetes y aplicar migraciones
dotnet restore
# (Opcional) Si tienes migraciones pendientes:
# dotnet ef database update

# Ejecutar la API
dotnet run
```

La API quedará escuchando en `http://localhost:5294`.

### 3. Configurar y ejecutar el Frontend
```bash
cd ../GestorMantenimiento.Frontend
# Instalar dependencias
npm install

# Levantar la aplicación Angular
ng serve
```

El frontend estará disponible en `http://localhost:4200`.

## Configuración de JWT
En el backend (appsettings.json), define tu secreto y parámetros de JWT:

```json
"Jwt": {
    "Key": "MiClaveSuperSecretaDeAlMenos32Caracteres!!",
    "Issuer": "GestorMantenimientoAPI",
    "Audience": "GestorMantenimientoFrontend"
}
```

## Uso Básico
1. Accede a `http://localhost:4200/login`.
2. Inicia sesión como **Admin** (`admin`/`admin123`) o **Technician** (`tech`/`tech123`).
3. Explora las rutas:

   * **Dashboard**: estadísticas de tareas.
   * **Máquinas**: alta, edición y baja de máquinas (solo Admin).
   * **Tareas**: gestión completa de tareas.

## Endpoints Principales de la API
* `POST /api/auth/login` – autenticar y obtener JWT.
* `GET /api/machine` – lista de máquinas.
* `POST /api/machine` – crear máquina.
* `PUT /api/machine/{id}` – actualizar máquina.
* `DELETE /api/machine/{id}` – eliminar máquina.
* `GET /api/maintenancetask` – lista de tareas.
* `POST /api/maintenancetask` – crear tarea.
* `PUT /api/maintenancetask/{id}` – actualizar tarea.
* `PATCH /api/maintenancetask/{id}/complete` – marcar completada.
* `GET /api/maintenancetask/stats` – estadísticas de tareas.
* `GET /api/maintenancetask/dashboard` – datos de dashboard.

## Estructura de Directorios
```
gestor-mantenimiento/
├── GestorMantenimiento.API/       # Backend .NET 8
├── GestorMantenimiento.Frontend/  # Frontend Angular 20
└── README.md                      # Este documento
```