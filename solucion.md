# Solución

## Tecnologías

Para la solución se usaran tecnologías como .Net Core para el backend y Angular para el frontend, para el almacenamiento de la información se usara MongoDB, para el desarrollo se usara Visual Studio Code y para el testing se usara Postman.

El código se almacenará en el siguiente repositorio de github: [https://github.com/sebCarabali/prueba_ceiba](https://github.com/sebCarabali/prueba_ceiba).

Para el despliegue se usara AWS CloudFormation para desplegar el frontend y el backend.

## Diseño de la Base de Datos

Este es el diseño de la base de datos sugerido para el sistema de gestión de fondos de inversión.

### 📚 Colección `clientes`

Piensa en esta colección como el **perfil de un cliente**. Cada documento es una ficha individual que contiene toda su información clave.

*   `_id`: Es el **número de identificación único** del cliente.
*   `nombre`: El nombre del cliente.
*   `balance`: El saldo actual del cliente, que comienza en COP \$500,000.
*   `fondosSuscritos`: Una **lista de los fondos** a los que el cliente está suscrito.
    *   `id`: El número de identificación de ese fondo específico.
    *   `nombreFondo`: El nombre del fondo (por ejemplo, "FPV_BTG_PACTUAL_RECAUDADORA").
    *   `cantidadInvertida`: El monto que se invirtió para vincularse al fondo[cite: 20].
    *   `fechaSuscripcion`: La fecha en que se realizó la suscripción.
*   `historialTransacciones`: Un **historial detallado** de todas las transacciones, como aperturas y cancelaciones.
    *   `id`: Un código único para cada movimiento[cite: 19].
    *   `tipo`: El tipo de movimiento, que puede ser "suscripción" o "cancelación".
    *   `idFondo`: El fondo al que se asocia esta transacción.
    *   `cantidadInvertida`: El monto de la transacción.
    *   `fecha`: La fecha y hora exactas en que ocurrió el movimiento.

---

### 📚 Colección `fondos`

Esta colección funciona como el **catálogo de productos**. Cada documento aquí representa uno de los fondos disponibles para que los clientes inviertan[cite: 30].

*   `_id`: El **número de identificación único** para cada fondo.
*   `nombre`: El nombre oficial del fondo.
*   `montoMinimo`: El **costo mínimo de entrada** para unirse a este fondo.
*   `categoria`: El **tipo de fondo** al que pertenece, como "FPV" o "FIC".

## Despliegue en cloudformation

El despliegue se encuentra en el siguiente repositorio de github: [https://github.com/sebCarabali/prueba_ceiba/despliegue.md](https://github.com/sebCarabali/prueba_ceiba/despliegue.md).
---
## Solución consulta

```sql
SELECT DISTINCT c.nombre, c.apellidos
FROM Cliente c
JOIN Inscripción i ON c.id = i.idCliente
JOIN Producto p ON i.idProducto = p.id
JOIN Disponibilidad d ON p.id = d.idProducto
JOIN Visitan v ON c.id = v.idCliente AND d.idSucursal = v.idSucursal;
```

---