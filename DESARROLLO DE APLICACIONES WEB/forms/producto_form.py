from flask_wtf import FlaskForm
from wtforms import StringField, DecimalField, TextAreaField, BooleanField, SubmitField
from wtforms.validators import DataRequired, Length, NumberRange


class ProductoForm(FlaskForm):
    """
    Formulario para registrar y editar productos del catálogo AL WORK.
    Se reutiliza tanto para registro como para edición (ver ruta en app.py).
    """
    nombre = StringField(
        'Nombre del producto',
        validators=[DataRequired(message='El nombre del producto es obligatorio.'),
                    Length(min=3, max=80, message='El nombre debe tener entre 3 y 80 caracteres.')]
    )

    precio = DecimalField(
        'Precio ($)',
        places=2,
        validators=[DataRequired(message='El precio es obligatorio.'),
                    NumberRange(min=0.01, max=1000, message='El precio debe estar entre 0.01 y 1000.')]
    )

    imagen = StringField(
        'Archivo de imagen (ej: polo1.png)',
        validators=[DataRequired(message='Debe indicar el archivo de imagen.'),
                    Length(max=100, message='El nombre del archivo es demasiado largo.')]
    )

    descripcion = TextAreaField(
        'Descripción',
        validators=[DataRequired(message='La descripción es obligatoria.'),
                    Length(min=10, max=300, message='La descripción debe tener entre 10 y 300 caracteres.')]
    )

    disponible = BooleanField('Disponible')

    submit = SubmitField('Guardar producto')
