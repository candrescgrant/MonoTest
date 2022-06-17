using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;
using MonoGameDesktopGL;

namespace MonoGameDesktopDX
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int width = 1280;
        int height = 720;
        Texture2D tex;
        Rectangle rect;
                
        bool colision = false;

        //ENEMIGOS
        List<Enemigo> listaEnemigos = new List<Enemigo>();
        Random random = new Random();
        int contador = 0;
        int limite = 10;

        //MOUSE
        MouseState raton = new MouseState();

        //FONDO BACKGROUND
        Texture2D fondo;
        Rectangle rectFondo;

        //FUENTE
        SpriteFont fuente;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            VariablesVentana();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CargarFondo();
            CargarFuente();
            CrearHeroe();
            CrearEnemigosIniciales();

            // TODO: load content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (!colision)
            {
                MovimientosTeclado();
            }
            ActualizarEnemigos();
            EliminarEnemigos();
            ComprobarVida();
            DetectarColision();
            Conteo();
            DetectarRaton();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //TODO: Draw your game
            spriteBatch.Begin();
            spriteBatch.Draw(fondo, rectFondo, Color.White);
            DibujarEnemigos();
            spriteBatch.Draw(tex, rect, Color.White);
            spriteBatch.DrawString(fuente, "ESTE ES UN TEXTO DE PRUEBA", Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(time);
        }


        private void CargarFuente()
        {
            fuente = Content.Load<SpriteFont>("FONT");
        }
        private void CargarFondo()
        {
            FileStream imagenFondo = new FileStream("Content/bg1.jpg", FileMode.Open);
            fondo = Texture2D.FromStream(GraphicsDevice, imagenFondo);
            imagenFondo.Dispose();
            rectFondo = new Rectangle(0,0, 2000, height);
        }
        private void DetectarRaton()
        {
            raton = Mouse.GetState();
            rect.X = raton.Position.X;
            rect.Y = raton.Position.Y;
        }
        private void EliminarEnemigos()
        {
            for(int i = 0; i < listaEnemigos.Count; i++)
            {
                if(listaEnemigos[i].rect.Y > 500)
                {
                    listaEnemigos[i].Eliminar();
                }
            }
        }
        private void ComprobarVida()
        {
            for(int i = 0; i < listaEnemigos.Count; i++)
            {
                if (!listaEnemigos[i].estaVivo)
                {
                    listaEnemigos.Remove(listaEnemigos[i]);
                }
            }
        }
        private void DibujarEnemigos()
        {
            for (int i = 0; i < listaEnemigos.Count; i++)
            {
                listaEnemigos[i].Draw();
            }
        }
        private void ActualizarEnemigos()
        {
            for (int i = 0; i < listaEnemigos.Count; i++)
            {
                listaEnemigos[i].Update();
            }
        }
        private void VariablesVentana()
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }
        private void CrearHeroe()
        {
            FileStream imagenChoco = new FileStream("Content/chapulin.png", FileMode.Open);
            tex = Texture2D.FromStream(GraphicsDevice, imagenChoco);
            imagenChoco.Dispose();
            rect = new Rectangle(100, 300, 150, 150);
        }
        private void CrearEnemigosIniciales()
        {
            FileStream imagenEnemigo = new FileStream("Content/1477232.png", FileMode.Open);
            for (int i = 0; i < 20; i++)
            {
                Texture2D texEnemigo = Texture2D.FromStream(GraphicsDevice, imagenEnemigo);
                Vector2 posicionEnemigo = new Vector2(random.Next(0, width), -200);
                Vector2 velocidadEnemigo = new Vector2(0, random.Next(1, 5));
                Enemigo nuevoEnemigo = new Enemigo(
                    texEnemigo,
                    posicionEnemigo,
                    velocidadEnemigo,
                    spriteBatch);

                listaEnemigos.Add(nuevoEnemigo);
                GC.Collect();
            }
            imagenEnemigo.Dispose();
        }
        private void Conteo()
        {
            contador++;
            if(contador >= limite)
            {
                CrearEnemigo();
                contador = 0;
            }
        }
        private void CrearEnemigo()
        {
            FileStream imagenEnemigo = new FileStream("Content/1477232.png", FileMode.Open);
            Texture2D texEnemigo = Texture2D.FromStream(GraphicsDevice, imagenEnemigo);
            Vector2 posicionEnemigo = new Vector2(random.Next(0, width), -200);
            Vector2 velocidadEnemigo = new Vector2(0, random.Next(1, 5));
            Enemigo nuevoEnemigo = new Enemigo(
                 texEnemigo,
                 posicionEnemigo,
                 velocidadEnemigo,
                 spriteBatch);
            listaEnemigos.Add(nuevoEnemigo);            
            imagenEnemigo.Dispose();
            GC.Collect();
        }
        private void DetectarColision()
        {
            for(int i = 0; i < listaEnemigos.Count; i++)
            {
                if (rect.Intersects(listaEnemigos[i].rect))
                {
                    listaEnemigos[i].Eliminar();
                }
            }
        }
        private void MovimientosTeclado()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rect.X += 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rect.X -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                rect.Y -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                rect.Y += 5;
            }
        }
    }
}