#region Using

using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace Willcraftia.Xna.Framework.Audio
{
    /// <summary>
    /// Wave ファイルをストリーミングする DynamicSoundEffectInstance を管理するクラスです。
    /// </summary>
    public sealed class StreamingWave : IDisposable
    {
        /// <summary>
        /// 非同期に Wave データを読み込むための delegate。
        /// </summary>
        delegate void ReadDataAsyncCaller();

        /// <summary>
        /// Wave ファイルの Stream。
        /// </summary>
        Stream input;

        /// <summary>
        /// Wave ファイルの BinaryReader。
        /// </summary>
        BinaryReader reader;

        /// <summary>
        /// 'riff' chunk。
        /// </summary>
        RiffChunk riffChunk;

        /// <summary>
        /// 'format' chunk。
        /// </summary>
        WaveFormatChunk formatChunk;

        /// <summary>
        /// 'data' chunk のヘッダ部。
        /// </summary>
        /// <remarks>
        /// 'data' chunk のデータ部は非同期読み込みの対象。
        /// </remarks>
        ChunkHeader dataChunkHeader;

        /// <summary>
        /// input における Wave データの開始位置。
        /// </summary>
        long dataOffset;

        /// <summary>
        /// ストリーミング対応の DynamicSoundEffectInstance。
        /// </summary>
        DynamicSoundEffectInstance dynamicSound;

        /// <summary>
        /// バッファ サイズ。
        /// </summary>
        int bufferSize;

        /// <summary>
        /// バッファ。
        /// </summary>
        byte[] buffer;

        /// <summary>
        /// ReadData で読み込んだ Wave データのバイト数。
        /// </summary>
        int dataCount;

        /// <summary>
        /// ReadData で読み込んだ Wave データのバイト総数。
        /// </summary>
        int totalDataCount;

        /// <summary>
        /// 非同期に ReadData メソッドを呼び出すための delegate。
        /// </summary>
        ReadDataAsyncCaller readDataAsyncCaller;

        /// <summary>
        /// readDataAsyncCaller の IAsyncResult。
        /// </summary>
        IAsyncResult asyncResult;

        /// <summary>
        /// ストリーミング対応の DynamicSoundEffectInstance を取得します。
        /// </summary>
        public DynamicSoundEffectInstance DynamicSound
        {
            get { return dynamicSound; }
        }

        /// <summary>
        /// Wave データの読み込みをループさせるかどうかを示す値を取得または設定します。
        /// DynamicSoundEffectInstance の IsLooped プロパティは常に false であり、
        /// ループ再生するかどうかは DynamicSoundEffectInstance にデータを提供する側の責務です。
        /// </summary>
        public bool Looped { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// StreamingWave が存在する間、ストリーミングのために input はオープン状態が継続され、
        /// Dispose メソッドでその Dispose メソッドが呼び出されます。
        /// </summary>
        /// <param name="input">Wave ファイルの Stream。</param>
        /// <param name="bufferDuration">バッファリングする再生時間。</param>
        public StreamingWave(Stream input, TimeSpan bufferDuration)
        {
            if (input == null) throw new ArgumentNullException("input");
            this.input = input;

            reader = new BinaryReader(input);

            // 'data' chunk のデータ部の直前まで読み込みます。
            riffChunk = RiffChunk.ReadFrom(reader);
            formatChunk = WaveFormatChunk.ReadFrom(reader);
            dataChunkHeader = ChunkHeader.ReadFrom(reader);

            // 'data' chunk のデータ部の開始位置を記憶します。
            dataOffset = input.Position;

            int sampleRate = (int) formatChunk.SampleRate;
            AudioChannels channels = (AudioChannels) formatChunk.Channels;
            dynamicSound = new DynamicSoundEffectInstance(sampleRate, channels);
            dynamicSound.BufferNeeded += new EventHandler<EventArgs>(OnDynamicSoundBufferNeeded);

            bufferSize = dynamicSound.GetSampleSizeInBytes(bufferDuration);
            buffer = new byte[bufferSize];

            readDataAsyncCaller = new ReadDataAsyncCaller(ReadData);
        }

        /// <summary>
        /// 初期状態に戻します。
        /// </summary>
        public void Reset()
        {
            if (asyncResult != null) asyncResult.AsyncWaitHandle.WaitOne();

            asyncResult = null;
            dataCount = 0;
            totalDataCount = 0;
            input.Seek(dataOffset, SeekOrigin.Begin);
        }

        /// <summary>
        /// Wave データを読み込みます。
        /// </summary>
        void ReadData()
        {
            dataCount = input.Read(buffer, 0, bufferSize);
            totalDataCount += dataCount;

            // 'data' chunk より後にもデータがあるため、それらを破棄するように調整します。
            if (dataChunkHeader.Size < totalDataCount)
            {
                dataCount -= totalDataCount - dataChunkHeader.Size;
                totalDataCount = dataChunkHeader.Size;
            }

            // ループ ON で末尾に到達している場合は、先頭に戻して更に読み込みます。
            if (Looped && dataCount < bufferSize)
            {
                totalDataCount = 0;
                input.Seek(dataOffset, SeekOrigin.Begin);

                dataCount += input.Read(buffer, dataCount, bufferSize - dataCount);
                totalDataCount += dataCount;
            }
        }

        /// <summary>
        /// DynamicSoundEffectInstance.BufferNeeded イベントの発生で呼び出されます。
        /// 必要に応じて Wave データを読み込み、DynamicSoundEffectInstance に設定します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDynamicSoundBufferNeeded(object sender, EventArgs e)
        {
            if (!Looped && totalDataCount == dataChunkHeader.Size)
            {
                // 再生を完全に終わらせるように処理します。
                if (dynamicSound.PendingBufferCount == 0)
                {
                    // 停止寸前であるため状態をリセットします。
                    // これにより、停止後に再生を開始した時に Wave データの先頭から再生できます。
                    Reset();

                    // 停止状態に設定します。
                    dynamicSound.Stop();
                }
                return;
            }

            if (asyncResult == null)
            {
                // 再生直後はここに入ります。
                // ここでは同期的に Wave データを読み込みます。
                ReadData();
            }
            else
            {
                // 非同期に Wave データを読み込んでいるので、その完了を待機します。
                asyncResult.AsyncWaitHandle.WaitOne();
            }

            if (dataCount == bufferSize)
            {
                // 最大サイズをフルに使用している場合は分割して設定します。
                var halfSize = bufferSize / 2;
                dynamicSound.SubmitBuffer(buffer, 0, halfSize);
                dynamicSound.SubmitBuffer(buffer, halfSize, halfSize);
            }
            else if (dataCount != 0)
            {
                // 最大サイズに足りていない場合はそのまま設定します。
                dynamicSound.SubmitBuffer(buffer, 0, dataCount);
            }

            dataCount = 0;

            if (!Looped && totalDataCount == dataChunkHeader.Size)
            {
                // ループ OFF で末尾に到達しているならば、ここで設定したバッファで再生を終えます。
                // なお、DynamicSoundEffectInstance は、Stop() および Stop(bool) メソッドを呼び出しても、
                // PendingBufferCount = 0 になるまで停止しない点に注意が必要です。
                return;
            }

            // 非同期に次の Wave データを読み込み、次の BufferNeeded イベント受信に備えます。
            asyncResult = readDataAsyncCaller.BeginInvoke(null, null);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~StreamingWave()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) input.Dispose();

            disposed = true;
        }

        #endregion
    }
}
