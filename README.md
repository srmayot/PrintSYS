# PrintSYS - Sistema de Gestión de Reclamos IT

Aplicación de escritorio desarrollada en **C# (Windows Forms)** y **SQL Server** diseñada para optimizar el control, seguimiento y resolución de incidencias y reclamos de impresoras Xerox en el área de Soporte Técnico.

---

## Características Principales

- **Gestión CRUD Completa:** Registro, consulta, actualización y eliminación de reclamos de forma dinámica.
- **Control de Incidencias:** Almacenamiento de datos del usuario, modelo de impresora, número de serie, sucursal, estado y fechas de inicio/cierre.
- **Generación de Reportes:** Exportación de reportes detallados (incluyendo soporte para formatos y reportes en Excel).
- **Interfaz Intuitiva:** Desarrollada con Windows Forms, enfocada en la velocidad de atención del equipo de soporte.

---

## Tecnologías Utilizadas

- **Lenguaje:** C# (.NET Framework / .NET)
- **Interfaz Gráfica:** Windows Forms
- **Base de Datos:** Microsoft SQL Server
- **Reportes:** Módulo de exportación a Excel / Generación de reportes

---

## Estructura de la Base de Datos (SQL Server)

La tabla principal del sistema (`Reclamos`) cuenta con los siguientes campos principales:
- `IdReclamo` (PK - Autoincrementable)
- `ModeloImpresora`
- `NumeroSerie`
- `UsuarioNombre`
- `Telefono` / `Sucursal`
- `DetalleProblema`
- `FechaInicio` / `FechaFin`
- `Estado` (Pendiente, En Proceso, Resuelto)
- `MedioReclamo` (Correo, Llamada, Presencial)
- `Observaciones`

---

## Configuración e Instalación

1. Clonar este repositorio o descargar el código fuente.
2. Abrir la solución (`.sln`) en **Visual Studio**.
3. Configurar la cadena de conexión a la base de datos en el archivo de configuración (`App.config`) por defecto está localhost
4. Ejecutar el script de SQL Server para crear la base de datos y la tabla correspondiente.
5. Compilar y ejecutar el proyecto

---
*Desarrollado para optimizar los procesos del área de Soporte TI.*
