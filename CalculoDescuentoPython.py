def calcular_descuento(monto_total, porcentaje_descuento=10):
    """
    Calcula el descuento basado en un porcentaje y retorna el monto descontado.
    :param monto_total: Total de la compra
    :param porcentaje_descuento: Porcentaje de descuento (por defecto 10%)
    :return: Monto del descuento
    """
    descuento = (monto_total * porcentaje_descuento) / 100
    return descuento

# Llamadas a la función con diferentes valores
monto1 = 100  # Monto total de la compra
monto2 = 200  # Otro monto total de la compra
porcentaje_personalizado = 15  # Descuento personalizado

descuento1 = calcular_descuento(monto1)
descuento2 = calcular_descuento(monto2, porcentaje_personalizado)

# Cálculo del monto final a pagar por el cliente.
total1 = monto1 - descuento1
total2 = monto2 - descuento2

# Mostrar resultados
print(f"Monto total: ${monto1}, Descuento aplicado al cliente: ${descuento1}, Monto final a pagar por el cliente: ${total1}")
print(f"Monto total: ${monto2}, Descuento aplicado al cliente: ${descuento2}, Monto final a pagar por el cliente: ${total2}")
print("Gracias por tu compra en Supermercados RamÍrez")
