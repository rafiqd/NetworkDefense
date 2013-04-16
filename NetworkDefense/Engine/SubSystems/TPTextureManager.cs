using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine.SubSystems
{
    public class TPTextureManager
    {
        /// <summary>
        /// Game reference.
        /// </summary>
        private TPGame m_GameRef;
        /// <summary>
        /// Dictionary used to store our game textures. 
        /// </summary>
        Dictionary<string, Texture2D> m_TextureLibrary = new Dictionary<string, Texture2D>();
        /// <summary>
        /// Set used to store references to our procedurally generated textures. 
        /// </summary>
        HashSet<Texture2D> m_ProceduralLibrary = new HashSet<Texture2D>();
        public TPTextureManager(TPGame gameref)
        {
            m_GameRef = gameref;
        }
        /// <summary>
        /// Adds a texture to an existing texture dictionary. If the texture already exists in memory it returns
        /// a reference to that texture. 
        /// </summary> 
        /// <param name="texPath">The path to the texture. Also used as a unique key.</param>
        public Texture2D LoadTexture( string texPath)
        {
            if (m_TextureLibrary.ContainsKey(texPath))
            {
                return m_TextureLibrary[texPath];
            }
            else
            { 
                m_TextureLibrary.Add(texPath, m_GameRef.Content.Load<Texture2D>(texPath));
            }
            return m_TextureLibrary[texPath]; 
        } 
        /// <summary>
        /// Creates and returns a procedurally generated rectangle. 
        /// </summary>
        /// <param name="width">The rectangle width.</param>
        /// <param name="height">The rectangle height.</param>
        /// <param name="color">The rectangle color.</param>
        /// <returns></returns>
        public Texture2D CreateFilledRectangle(int width, int height, Color color)
        {
            Color[] myColor = new Color[width * height];
            Texture2D tex = new Texture2D(m_GameRef.GraphicsDevice, width, height, false, SurfaceFormat.Color);

            for (int i = 0; i < myColor.Length; i++)
            {
                myColor[i] = color;
            }

            tex.SetData<Color>(myColor);
            m_ProceduralLibrary.Add(tex);
            return tex;
        } 
        /// <summary>
        /// Clears procedural texture memory.
        /// </summary>
        public void ClearProceduralMemory()
        {
            foreach (Texture2D t in m_ProceduralLibrary)
            {
                t.Dispose();
            }
            m_ProceduralLibrary.Clear();
        } 
    }
}
