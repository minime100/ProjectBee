using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjectBee.Domain;

namespace ProjectBee.Main
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainEntry : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameObject cube;

        // The object that will contain our shader
        Effect shader;

        // Parameters for our shader object
        EffectParameter projectionParameter;
        EffectParameter viewParameter;
        EffectParameter worldParameter;
        EffectParameter ambientIntensityParameter;
        EffectParameter ambientColorParameter;
        EffectParameter lightDir;

        Matrix world, view, projection;
        float ambientLightIntensity;
        Vector4 ambientLightColor;

        double rotateCamera = 0.0f;

        public MainEntry()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        public void SetupShaderParameters()
        {
            // Bind the parameters with the shader.
            worldParameter = shader.Parameters["World"];
            viewParameter = shader.Parameters["View"];
            projectionParameter = shader.Parameters["Projection"];

            ambientColorParameter = shader.Parameters["AmbientColor"];
            ambientIntensityParameter = shader.Parameters["AmbientIntensity"];
            lightDir = shader.Parameters["LightDir"];
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            cube = CreateCube();

            // Load the shader
            shader = Content.Load<Effect>("Shader");

            // Set up the parameters
            SetupShaderParameters();

            // calculate matrixes
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 1000.0f);

            //create a default world matrix
            world = Matrix.Identity;

            // TODO: use this.Content to load your game content here
        }

        private GameObject CreateCube()
        {
            GameObject cube = new GameObject();
            cube.Mesh = Content.Load<Model>("cube");
            return cube;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ambientLightIntensity = 1.0f;
            ambientLightColor = Color.DarkGreen.ToVector4();

            rotateCamera += gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            view = Matrix.CreateLookAt(new Vector3(20.0f * (float)Math.Cos(rotateCamera), 2, 20.0f * (float)Math.Sin(rotateCamera)), new Vector3(0, 0, 0), Vector3.Up);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ModelMesh mesh = cube.Mesh.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];

            // Set parameters
            projectionParameter.SetValue(projection);
            viewParameter.SetValue(view);
            worldParameter.SetValue(world);
            ambientIntensityParameter.SetValue(ambientLightIntensity);
            ambientColorParameter.SetValue(ambientLightColor);
            lightDir.SetValue(new Vector3(1,0,0));


            //set the vertex source to the mesh's vertex buffer
            graphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);

            //set the current index buffer to the sample mesh's index buffer
            graphics.GraphicsDevice.Indices = meshPart.IndexBuffer;

            shader.CurrentTechnique = shader.Techniques["Technique1"];

            for (int i = 0; i < shader.CurrentTechnique.Passes.Count; i++)
            {
                //EffectPass.Apply will update the device to
                //begin using the state information defined in the current pass
                shader.CurrentTechnique.Passes[i].Apply();

                //theMesh contains all of the information required to draw
                //the current mesh
                graphics.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
            }

            base.Draw(gameTime);
        }
    }
}
