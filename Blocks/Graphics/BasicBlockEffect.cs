#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// BasicEffect を実体とする IBlockEffect の実装クラスです。
    /// </summary>
    public sealed class BasicBlockEffect : IBlockEffect
    {
        /// <summary>
        /// BasicEffect。
        /// </summary>
        BasicEffect backingEffect;

        // I/F
        public Matrix World
        {
            get { return backingEffect.World; }
            set { backingEffect.World = value; }
        }

        // I/F
        public Matrix View
        {
            get { return backingEffect.View; }
            set { backingEffect.View = value; }
        }

        // I/F
        public Matrix Projection
        {
            get { return backingEffect.Projection; }
            set { backingEffect.Projection = value; }
        }

        // I/F
        public Vector3 AmbientLightColor
        {
            get { return backingEffect.AmbientLightColor; }
            set { backingEffect.AmbientLightColor = value; }
        }

        // I/F
        public DirectionalLight DirectionalLight0
        {
            get { return backingEffect.DirectionalLight0; }
        }

        // I/F
        public DirectionalLight DirectionalLight1
        {
            get { return backingEffect.DirectionalLight1; }
        }

        // I/F
        public DirectionalLight DirectionalLight2
        {
            get { return backingEffect.DirectionalLight2; }
        }

        // I/F
        public bool LightingEnabled
        {
            get { return backingEffect.LightingEnabled; }
            set { backingEffect.LightingEnabled = value; }
        }

        // I/F
        public bool FogEnabled
        {
            get { return backingEffect.FogEnabled; }
            set { backingEffect.FogEnabled = value; }
        }

        // I/F
        public float FogStart
        {
            get { return backingEffect.FogStart; }
            set { backingEffect.FogStart = value; }
        }

        // I/F
        public float FogEnd
        {
            get { return backingEffect.FogEnd; }
            set { backingEffect.FogEnd = value; }
        }

        // I/F
        public Vector3 FogColor
        {
            get { return backingEffect.FogColor; }
            set { backingEffect.FogColor = value; }
        }

        // I/F
        public Vector3 DiffuseColor
        {
            get { return backingEffect.DiffuseColor; }
            set { backingEffect.DiffuseColor = value; }
        }

        // I/F
        public float Alpha
        {
            get { return backingEffect.Alpha; }
            set { backingEffect.Alpha = value; }
        }

        // I/F
        public Vector3 EmissiveColor
        {
            get { return backingEffect.EmissiveColor; }
            set { backingEffect.EmissiveColor = value; }
        }

        // I/F
        public Vector3 SpecularColor
        {
            get { return backingEffect.SpecularColor; }
            set { backingEffect.SpecularColor = value; }
        }

        // I/F
        public float SpecularPower
        {
            get { return backingEffect.SpecularPower; }
            set { backingEffect.SpecularPower = value; }
        }

        // I/F
        public EffectPass Pass
        {
            get { return backingEffect.CurrentTechnique.Passes[0]; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="backingEffect">BasicEffect。</param>
        public BasicBlockEffect(BasicEffect backingEffect)
        {
            if (backingEffect == null) throw new ArgumentNullException("backingEffect");
            this.backingEffect = backingEffect;
        }

        // I/F
        public void EnableDefaultLighting()
        {
            backingEffect.EnableDefaultLighting();
        }
    }
}
