#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// RenderTarget を管理するクラスです。
    /// </summary>
    public sealed class BackBuffer : IDisposable
    {
        /// <summary>
        /// GraphicsDevice。
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// 幅。
        /// </summary>
        int width;

        /// <summary>
        /// 高さ。
        /// </summary>
        int height;

        /// <summary>
        /// true (Mipmap を使用する場合)、false (それ以外の場合)。
        /// </summary>
        bool mipMap;

        /// <summary>
        /// SurfaceFormat。
        /// </summary>
        SurfaceFormat surfaceFormat;

        /// <summary>
        /// DepthFormat。
        /// </summary>
        DepthFormat depthFormat;

        /// <summary>
        /// MultiSampleCount。
        /// </summary>
        int multiSampleCount;

        /// <summary>
        /// RenderTargetUsage。
        /// </summary>
        RenderTargetUsage renderTargetUsage;

        /// <summary>
        /// 管理する RenderTarget の数。
        /// </summary>
        int renderTargetCount;
        
        /// <summary>
        /// true (このインスタンスの機能が有効な場合)、false (それ以外の場合)。
        /// </summary>
        bool enabled;
        
        /// <summary>
        /// 現在使用している RenderTarget のインデックス。
        /// </summary>
        int currentIndex;
        
        /// <summary>
        /// 管理している RenderTarget の配列。
        /// </summary>
        RenderTarget2D[] renderTargets;

        /// <summary>
        /// true (RenderTarget が利用可能な状態である場合)、false (それ以外の場合)。
        /// </summary>
        bool activated;

        /// <summary>
        /// true (Begin メソッドが呼び出され、End メソッドが呼び出される前である場合)、false (それ以外の場合)。
        /// </summary>
        bool begun;

        /// <summary>
        /// BackBuffer を識別するための名前を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 幅を取得または設定します。
        /// </summary>
        public int Width
        {
            get { return width; }
            set
            {
                if (width == value) return;
                width = value;
                Deactivate();
            }
        }

        /// <summary>
        /// 高さを取得または設定します。
        /// </summary>
        public int Height
        {
            get { return height; }
            set
            {
                if (height == value) return;
                height = value;
                Deactivate();
            }
        }

        /// <summary>
        /// MipMap を利用するかどうかを取得または設定します。
        /// </summary>
        /// <value>
        /// true (Mipmap を利用する場合)、false (それ以外の場合)。
        /// </value>
        public bool MipMap
        {
            get { return mipMap; }
            set
            {
                if (mipMap == value) return;
                mipMap = value;
                Deactivate();
            }
        }

        /// <summary>
        /// SurfaceFormat を取得または設定します。
        /// </summary>
        public SurfaceFormat SurfaceFormat
        {
            get { return surfaceFormat; }
            set
            {
                if (surfaceFormat == value) return;
                surfaceFormat = value;
                Deactivate();
            }
        }

        /// <summary>
        /// DepthFormat を取得または設定します。
        /// </summary>
        public DepthFormat DepthFormat
        {
            get { return depthFormat; }
            set
            {
                if (depthFormat == value) return;
                depthFormat = value;
                Deactivate();
            }
        }

        /// <summary>
        /// MultiSampleCount を取得または設定します。
        /// </summary>
        public int MultiSampleCount
        {
            get { return multiSampleCount; }
            set
            {
                if (multiSampleCount == value) return;
                multiSampleCount = value;
                Deactivate();
            }
        }

        /// <summary>
        /// RenderTargetUsage を取得または設定します。
        /// </summary>
        public RenderTargetUsage RenderTargetUsage
        {
            get { return renderTargetUsage; }
            set
            {
                if (renderTargetUsage == value) return;
                renderTargetUsage = value;
                Deactivate();
            }
        }

        /// <summary>
        /// 使用する RenderTarget の数を取得または設定します。
        /// </summary>
        public int RenderTargetCount
        {
            get { return renderTargetCount; }
            set
            {
                value = value < 1 ? 1 : value;
                if (renderTargetCount == value) return;
                renderTargetCount = value;
                Deactivate();
            }
        }

        /// <summary>
        /// このインスタンスの機能が有効であるかどうかを判定します。
        /// </summary>
        /// <value>
        /// true (このインスタンスの機能が有効である場合)、false (それ以外の場合)。
        /// </value>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;
                enabled = value;
                if (!enabled) Deactivate();
            }
        }

        /// <summary>
        /// 現在有効にしている RenderTarget のインデックスを取得または設定します。
        /// </summary>
        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                if (value < 0 || renderTargetCount <= value) throw new ArgumentOutOfRangeException("CurrentIndex");

                if (currentIndex != value) currentIndex = value;
            }
        }

        /// <summary>
        /// 矩形サイズを取得します。
        /// </summary>
        /// <remarks>
        /// このプロパティは new Rectangle(0, 0, Width, Height) を返します。
        /// </remarks>
        public Rectangle Bounds
        {
            get { return new Rectangle(0, 0, width, height); }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <remarks>
        /// name は、インスタンスで管理する RenderTarget の名前にも設定されます。
        /// 1 つの RenderTarget のみを管理する場合には、name の値がそのまま RenderTarget の名前として設定されます。
        /// 2 つ以上の RenderTarget を管理する場合には、
        /// name の値に各 RenderTarget のインデックスを接尾語とした名前が RenderTarget の名前として設定されます。
        /// </remarks>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="name">インスタンスを識別するための名前。</param>
        public BackBuffer(GraphicsDevice graphicsDevice, string name)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");

            this.graphicsDevice = graphicsDevice;

            Name = name;

            var pp = graphicsDevice.PresentationParameters;
            width = pp.BackBufferWidth;
            height = pp.BackBufferHeight;
            mipMap = true;
            surfaceFormat = pp.BackBufferFormat;
            depthFormat = pp.DepthStencilFormat;
            multiSampleCount = pp.MultiSampleCount;
            renderTargetUsage = pp.RenderTargetUsage;

            renderTargetCount = 1;
            currentIndex = 0;

            activated = false;
            disposed = false;
        }

        /// <summary>
        /// 指定のインデックスの RenderTarget を取得します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RenderTarget2D GetRenderTarget(int index)
        {
            AssertEnabled();
            Activate();
            return renderTargets[index];
        }

        /// <summary>
        /// CurrentIndex プロパティが示す RenderTarget を取得します。
        /// </summary>
        /// <returns></returns>
        public RenderTarget2D GetRenderTarget()
        {
            AssertEnabled();
            return GetRenderTarget(CurrentIndex);
        }

        /// <summary>
        /// CurrentIndex プロパティが示す RenderTarget を GraphicsDevice に設定します。
        /// </summary>
        public void Begin()
        {
            AssertEnabled();
            Begin(CurrentIndex);
        }

        /// <summary>
        /// Being メソッドで GraphicsDevice に設定した RenderTarget を GraphicsDevice から切り離します。
        /// </summary>
        public void End()
        {
            AssertEnabled();
            if (!begun)throw new InvalidOperationException("End() must be invoked after Begin()");

            graphicsDevice.SetRenderTarget(null);

            begun = false;
        }

        /// <summary>
        /// 指定のインデックスが示す RenderTarget を GraphicsDevice に設定します。
        /// </summary>
        /// <param name="index"></param>
        void Begin(int index)
        {
            AssertEnabled();
            if (begun) throw new InvalidOperationException("Begin() must not be invoked after the other Begin()");

            Activate();

            graphicsDevice.SetRenderTarget(renderTargets[index]);

            begun = true;
        }

        /// <summary>
        /// RenderTarget を利用可能な状態にします。
        /// </summary>
        void Activate()
        {
            if (activated) return;

            InitializeRenderTargets();
            activated = true;
        }

        /// <summary>
        /// RenderTarget を構築します。
        /// </summary>
        void InitializeRenderTargets()
        {
            renderTargets = new RenderTarget2D[renderTargetCount];

            for (int i = 0; i < renderTargetCount; i++)
            {
                renderTargets[i] = new RenderTarget2D(
                    graphicsDevice,
                    width,
                    height,
                    mipMap,
                    surfaceFormat,
                    depthFormat,
                    multiSampleCount,
                    renderTargetUsage);

                if (!string.IsNullOrEmpty(Name))
                {
                    if (1 < renderTargetCount)
                    {
                        renderTargets[i].Name = Name + "." + i;
                    }
                    else
                    {
                        renderTargets[i].Name = Name;
                    }
                }
            }
        }

        /// <summary>
        /// RenderTarget を破棄して利用不能な状態にします。
        /// </summary>
        void Deactivate()
        {
            if (!activated) return;

            DisposeRenderTargets();
            activated = false;
        }

        /// <summary>
        /// RenderTarget を破棄します。
        /// </summary>
        void DisposeRenderTargets()
        {
            if (renderTargets == null) return;

            for (int i = 0; i < renderTargets.Length; i++)
            {
                if (renderTargets[i] != null) renderTargets[i].Dispose();
            }
        }

        /// <summary>
        /// このインスタンスの機能が有効であるかどうかを検査します。
        /// </summary>
        void AssertEnabled()
        {
            if (!enabled) throw new InvalidOperationException(string.Format("Back buffer '{0}' disabled", Name));
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~BackBuffer()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) Deactivate();

            disposed = true;
        }

        #endregion
    }
}
