using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGameDesktopGL
{
    public class Enemigo
    {
        Texture2D tex;
        public Rectangle rect;
        Vector2 posicion;
        Vector2 velocidad;
        SpriteBatch spriteBatch;
        public bool estaVivo = true;

        public Enemigo(
            Texture2D _tex,
            Vector2 _posicion,
            Vector2 _velocidad,
            SpriteBatch _spriteBatch)
        {
            tex = _tex;
            posicion = _posicion;
            velocidad = _velocidad;
            spriteBatch = _spriteBatch;

            rect = new Rectangle((int)posicion.X, (int)posicion.Y, 50, 50);
        }

        public void Eliminar()
        {
            estaVivo = false;
        }

        public void Update()
        {
            rect.X += (int)velocidad.X;
            rect.Y += (int)velocidad.Y;
        }

        public void Draw()
        {
            spriteBatch.Draw(tex, rect, Color.White);
        }
    }
}
