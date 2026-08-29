from flask_wtf import FlaskForm
from wtforms import StringField, DecimalField, SubmitField
from wtforms.validators import DataRequired, Length, NumberRange, Regexp


class FacturacionForm(FlaskForm):
    """
    Formulario para registrar y editar facturas de AL WORK.
    Se reutiliza tanto para registro como para edición (ver ruta en app.py).
    """
    numero = StringField(
        'N° de factura',
        validators=[DataRequired(message='El número de factura es obligatorio.'),
                    Length(min=1, max=10, message='El número debe tener máximo 10 caracteres.'),
                    Regexp(r'^\d+$', message='El número de factura debe contener solo dígitos.')]
    )

    cliente = StringField(
        'Cliente',
        validators=[DataRequired(message='El cliente es obligatorio.'),
                    Length(min=3, max=80, message='El nombre del cliente debe tener entre 3 y 80 caracteres.')]
    )

    prenda = StringField(
        'Detalle / prenda facturada',
        validators=[DataRequired(message='El detalle de la factura es obligatorio.'),
                    Length(min=3, max=120, message='El detalle debe tener entre 3 y 120 caracteres.')]
    )

    total = DecimalField(
        'Total ($)',
        places=2,
        validators=[DataRequired(message='El total es obligatorio.'),
                    NumberRange(min=0.01, max=100000, message='El total debe ser mayor a 0.')]
    )

    fecha = StringField(
        'Fecha (dd/mm/aaaa)',
        validators=[DataRequired(message='La fecha es obligatoria.'),
                    Regexp(r'^\d{2}/\d{2}/\d{4}$', message='La fecha debe tener el formato dd/mm/aaaa.')]
    )

    submit = SubmitField('Guardar factura')
