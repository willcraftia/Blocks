#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// インスタンシングのための IBlockEffect の実装クラスです。
    /// </summary>
    public sealed class InstancingBlockEffect : IBlockEffect
    {
        /// <summary>
        /// インスタンシングに対応した Effect。
        /// </summary>
        Effect backingEffect;

        /// <summary>
        /// View パラメータ。
        /// </summary>
        EffectParameter view;
        
        /// <summary>
        /// Projection パラメータ。
        /// </summary>
        EffectParameter projection;
        
        /// <summary>
        /// AmbientLightColor パラメータ。
        /// </summary>
        EffectParameter ambientLightColor;

        /// <summary>
        /// DirectionalLight0。
        /// </summary>
        DirectionalLight directionalLight0;

        /// <summary>
        /// DirectionalLight1。
        /// </summary>
        DirectionalLight directionalLight1;

        /// <summary>
        /// DirectionalLight2。
        /// </summary>
        DirectionalLight directionalLight2;

        /// <summary>
        /// FogEnabled パラメータ。
        /// </summary>
        EffectParameter fogEnabled;

        /// <summary>
        /// FogStart パラメータ。
        /// </summary>
        EffectParameter fogStart;
        
        /// <summary>
        /// FogEnd パラメータ。
        /// </summary>
        EffectParameter fogEnd;
        
        /// <summary>
        /// FogColor パラメータ。
        /// </summary>
        EffectParameter fogColor;

        /// <summary>
        /// DiffuseColor パラメータ。
        /// </summary>
        EffectParameter diffuseColor;

        /// <summary>
        /// Alpha パラメータ。
        /// </summary>
        EffectParameter alpha;

        /// <summary>
        /// EmissiveColor パラメータ。
        /// </summary>
        EffectParameter emissiveColor;

        /// <summary>
        /// SpecularColor パラメータ。
        /// </summary>
        EffectParameter specularColor;

        /// <summary>
        /// SpecularPower パラメータ。
        /// </summary>
        EffectParameter specularPower;

        /// <summary>
        /// EyePosition パラメータ。
        /// </summary>
        EffectParameter eyePosition;

        // I/F
        public Matrix World
        {
            // NOTE
            // インスタンシングでは頂点ストリームから変換行列を得るため未使用となります。
            get { return Matrix.Identity; }
            set { }
        }

        // I/F
        public Matrix View
        {
            get { return view.GetValueMatrix(); }
            set
            {
                view.SetValue(value);
                eyePosition.SetValue(new Vector3(value.M41, value.M42, value.M43));
            }
        }

        // I/F
        public Matrix Projection
        {
            get { return projection.GetValueMatrix(); }
            set { projection.SetValue(value); }
        }

        // I/F
        public Vector3 AmbientLightColor
        {
            get { return ambientLightColor.GetValueVector3(); }
            set { ambientLightColor.SetValue(value); }
        }

        // I/F
        public DirectionalLight DirectionalLight0
        {
            get { return directionalLight0; }
        }

        // I/F
        public DirectionalLight DirectionalLight1
        {
            get { return directionalLight1; }
        }

        // I/F
        public DirectionalLight DirectionalLight2
        {
            get { return directionalLight2; }
        }

        // I/F
        public bool LightingEnabled
        {
            get
            {
                // MEMO
                // このインタフェースの役割がドキュメントからは不明です・・・。
                // ひとまずいずれかの Directional Light が ON ならば true とします。
                return directionalLight0.Enabled || directionalLight1.Enabled || directionalLight2.Enabled;
            }
            set
            {
                // MEMO
                // このインタフェースの役割がドキュメントからは不明です・・・。
                // ひとまず全 Directional Light を ON にします。
                directionalLight0.Enabled = true;
                directionalLight1.Enabled = true;
                directionalLight2.Enabled = true;
            }
        }

        // I/F
        public bool FogEnabled
        {
            get { return fogEnabled.GetValueBoolean(); }
            set { fogEnabled.SetValue(value); }
        }

        // I/F
        public float FogStart
        {
            get { return fogStart.GetValueSingle(); }
            set { fogStart.SetValue(value); }
        }

        // I/F
        public float FogEnd
        {
            get { return fogEnd.GetValueSingle(); }
            set { fogEnd.SetValue(value); }
        }

        // I/F
        public Vector3 FogColor
        {
            get { return fogColor.GetValueVector3(); }
            set { fogColor.SetValue(value); }
        }

        // I/F
        public Vector3 DiffuseColor
        {
            get { return diffuseColor.GetValueVector3(); }
            set { diffuseColor.SetValue(value); }
        }

        // I/F
        public float Alpha
        {
            get { return alpha.GetValueSingle(); }
            set { alpha.SetValue(value); }
        }

        // I/F
        public Vector3 EmissiveColor
        {
            get { return emissiveColor.GetValueVector3(); }
            set { emissiveColor.SetValue(value); }
        }

        // I/F
        public Vector3 SpecularColor
        {
            get { return specularColor.GetValueVector3(); }
            set { specularColor.SetValue(value); }
        }

        // I/F
        public float SpecularPower
        {
            get { return specularPower.GetValueSingle(); }
            set { specularPower.SetValue(value); }
        }

        // I/F
        public EffectPass Pass
        {
            get { return backingEffect.CurrentTechnique.Passes[0]; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="backingEffect">インスタンシングに対応した Effect。</param>
        public InstancingBlockEffect(Effect backingEffect)
        {
            if (backingEffect == null) throw new ArgumentNullException("backingEffect");
            // Effect の複製を生成して設定します。
            // 複製しないと InstancingBlockEffect 間で backingEffect が共有されてしまい、
            // プロパティの設定が互いに上書きされてしまいます。
            this.backingEffect = backingEffect.Clone();

            InitializeParameters();
        }

        // I/F
        public void EnableDefaultLighting()
        {
            ambientLightColor.SetValue(new Vector3(0.05333332f, 0.09882354f, 0.1819608f));
            directionalLight0.Enabled = true;
            directionalLight0.Direction = new Vector3(-0.5265408f, -0.5735765f, -0.6275069f);
            directionalLight0.DiffuseColor = new Vector3(1, 0.9607844f, 0.8078432f);
            directionalLight0.SpecularColor = new Vector3(1, 0.9607844f, 0.8078432f);
            directionalLight1.Enabled = true;
            directionalLight1.Direction = new Vector3(0.7198464f, 0.3420201f, 0.6040227f);
            directionalLight1.DiffuseColor = new Vector3(0.9647059f, 0.7607844f, 0.4078432f);
            directionalLight1.SpecularColor = new Vector3();
            directionalLight2.Enabled = true;
            directionalLight2.Direction = new Vector3(0.4545195f, -0.7660444f, 0.4545195f);
            directionalLight2.DiffuseColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f);
            directionalLight2.SpecularColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f);
        }

        /// <summary>
        /// プロパティからのアクセスに使用する EffectParameter の取得と初期化を行います。
        /// </summary>
        void InitializeParameters()
        {
            view = backingEffect.Parameters["View"];
            projection = backingEffect.Parameters["Projection"];

            ambientLightColor = backingEffect.Parameters["AmbientLightColor"];
            directionalLight0 = new DirectionalLight(
                backingEffect.Parameters["DirLight0Direction"],
                backingEffect.Parameters["DirLight0DiffuseColor"],
                backingEffect.Parameters["DirLight0SpecularColor"],
                null);
            directionalLight1 = new DirectionalLight(
                backingEffect.Parameters["DirLight1Direction"],
                backingEffect.Parameters["DirLight1DiffuseColor"],
                backingEffect.Parameters["DirLight1SpecularColor"],
                null);
            directionalLight2 = new DirectionalLight(
                backingEffect.Parameters["DirLight2Direction"],
                backingEffect.Parameters["DirLight2DiffuseColor"],
                backingEffect.Parameters["DirLight2SpecularColor"],
                null);
            
            fogEnabled = backingEffect.Parameters["FogEnabled"];
            fogStart = backingEffect.Parameters["FogStart"];
            fogEnd = backingEffect.Parameters["FogEnd"];
            fogColor = backingEffect.Parameters["FogColor"];

            diffuseColor = backingEffect.Parameters["DiffuseColor"];
            alpha = backingEffect.Parameters["Alpha"];
            emissiveColor = backingEffect.Parameters["EmissiveColor"];
            specularColor = backingEffect.Parameters["SpecularColor"];
            specularPower = backingEffect.Parameters["SpecularPower"];

            // View の設定時に [M41, M42, M43] を EyePosition へ設定します。
            // このため、専用のプロパティによるアクセスを提供しません。
            // BasicEffect でも EyePosition は定義済みで、Fog の制御で使用しますが、
            // 同様の方法で EyePosition を View から設定しているのではないか、と・・・。
            eyePosition = backingEffect.Parameters["EyePosition"];
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~InstancingBlockEffect()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) backingEffect.Dispose();

            disposed = true;
        }

        #endregion
    }
}
