from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired, Length, Regexp


class ClienteForm(FlaskForm):
    """
    Formulario para registrar y editar clientes de AL WORK.
    Se reutiliza tanto para registro como para edición (ver ruta en app.py).
    """
    nombre = StringField(
        'Nombre / Razón social',
        validators=[DataRequired(message='El nombre del cliente es obligatorio.'),
                    Length(min=3, max=80, message='El nombre debe tener entre 3 y 80 caracteres.')]
    )

    tipo = StringField(
        'Tipo de negocio',
        validators=[DataRequired(message='El tipo de negocio es obligatorio.'),
                    Length(min=3, max=60, message='El tipo de negocio debe tener entre 3 y 60 caracteres.')]
    )

    contacto = StringField(
        'Teléfono de contacto',
        validators=[DataRequired(message='El teléfono de contacto es obligatorio.'),
                    Length(min=7, max=10, message='El teléfono debe tener entre 7 y 10 dígitos.'),
                    Regexp(r'^\d+$', message='El teléfono debe contener solo números.')]
    )

    ciudad = StringField(
        'Ciudad',
        validators=[DataRequired(message='La ciudad es obligatoria.'),
                    Length(min=3, max=50, message='La ciudad debe tener entre 3 y 50 caracteres.')]
    )

    submit = SubmitField('Guardar cliente')
