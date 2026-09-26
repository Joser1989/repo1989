from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, SubmitField
from wtforms.validators import DataRequired, Length, EqualTo


class UsuarioForm(FlaskForm):
    usuario = StringField(
        'Usuario',
        validators=[DataRequired(), Length(min=3, max=50, message='Entre 3 y 50 caracteres')]
    )
    password = PasswordField(
        'Contraseña',
        validators=[DataRequired(), Length(min=6, message='Mínimo 6 caracteres')]
    )
    confirmar = PasswordField(
        'Confirmar contraseña',
        validators=[DataRequired(), EqualTo('password', message='Las contraseñas no coinciden')]
    )
    submit = SubmitField('Registrarse')
