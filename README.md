# Inventario Simple

Este proyecto consiste en un inventario simple para el taller de POO en C# de Programaci�n Distribuida.

## Integrantes

- Dairo Andres Mosquera Blandon
- Harrison Alonso Arroyave Gaviria

## Requisitos

Para ejecutar y desarrollar esta aplicaci�n necesitas:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- Un editor de c�digo como Visual Studio 2022, Visual Studio Code o similar
- Acceso al sistema de archivos para almacenar datos en formato JSON

## Dependencias

El proyecto utiliza las siguientes bibliotecas:

- **Microsoft.Extensions.Logging** (v9.0.9): Para registro de eventos y mensajes
- **System.Text.Json** (v9.0.9): Para serializaci�n y deserializaci�n de datos en formato JSON
- **xunit** (v2.9.3): Para pruebas unitarias

## Instalaci�n y Ejecuci�n

1. **Clonar el repositorio**:

```bash
git clone <url-del-repositorio>
cd InventarioApp
```

2. **Restaurar dependencias**:
```bash
dotnet restore
```

3. **Compilar el proyecto**:
```bash
dotnet build
```

4. **Ejecutar la aplicaci�n**:
```bash
dotnet run
```

5. **Ejecutar pruebas unitarias**:
```bash
dotnet test
```

## Estructura del Proyecto

El proyecto est� organizado en varias capas siguiendo los principios de Clean Architecture:
- **Domain**: Contiene las entidades y l�gica de negocio.
- **Application**: Contiene los casos de uso y servicios de aplicaci�n.
- **Infrastructure**: Implementa la persistencia de datos y otras dependencias externas.

## Uso de la Aplicaci�n

Al ejecutar la aplicaci�n, se muestra un men� interactivo con las siguientes opciones:

1. **Registrar producto**: A�adir un nuevo producto al inventario
2. **Consultar productos**: Ver todos los productos en el inventario
3. **Actualizar stock de un producto**: Modificar la cantidad disponible de un producto
4. **Eliminar producto**: Eliminar un producto del inventario
5. **Reporte de stock bajo**: Ver productos con stock inferior a un umbral especificado
6. **Buscar productos por nombre**: Encontrar productos por coincidencia en el nombre
7. **Salir**: Terminar la aplicaci�n

## Ejemplos de Uso

### A�adir un producto perecedero

```
=== Registrar producto ===
Seleccione el tipo de producto:
1. Perecedero
2. No perecedero
Ingrese la opci�n (1 o 2): 1
Ingrese el nombre del producto: Leche
Ingrese la descripci�n del producto: Leche entera
Ingrese el precio del producto: 2.50
Ingrese el stock disponible del producto: 10
Ingrese la fecha de expiraci�n (yyyy-MM-dd): 2025-12-31
```

### A�adir un producto no perecedero

```
=== Registrar producto ===
Seleccione el tipo de producto:
1. Perecedero
2. No perecedero
Ingrese la opci�n (1 o 2): 2
Ingrese el nombre del producto: Calculadora
Ingrese la descripci�n del producto: Calculadora cient�fica
Ingrese el precio del producto: 15.00
Ingrese el stock disponible del producto: 5
Ingrese la categor�a del producto: Electr�nica
```

### Consultar productos

```
=== Consultar productos ===
ID: P001, Nombre: Leche, Precio: $2.50, Cantidad: 10, Expira en: 31/12/2025
ID: NP001, Nombre: Arroz, Precio: $3.75, Cantidad: 50, Categor�a: Alimentos b�sicos
```

### Generar reporte de stock bajo
=== Reporte de stock bajo ===
Ingrese el umbral de stock bajo (por defecto 5): 15

Productos con stock menor a 15:
 - Leche (Perecedero) (Cantidad: 10)

## Almacenamiento de Datos

La aplicaci�n utiliza almacenamiento en formato JSON. Por defecto, los datos se guardan en:

**C:\temp\inventory.json**
