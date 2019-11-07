using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace BrawlLib.Internal
{
    /// <summary>
    /// A timer created from OpenTK's gamewindow.
    /// </summary>
    public class CoolTimer
    {
        private double
            _updatePeriod,
            _renderPeriod,
            _targetUpdatePeriod,
            _targetRenderPeriod,
            _updateTime,
            _renderTime,
            _nextRender,
            _nextUpdate;

        private readonly Stopwatch _updateWatch = new Stopwatch(), _renderWatch = new Stopwatch();

        public event EventHandler<FrameEventArgs> RenderFrame;
        public event EventHandler<FrameEventArgs> UpdateFrame;

        #region Render

        /// <summary>
        /// Gets or sets a double representing the target render frequency, in hertz.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that RenderFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 1.0Hz are clamped to 1.0Hz. Values higher than 200.0Hz are clamped to 200.0Hz.</para>
        /// </remarks>
        public double TargetRenderFrequency
        {
            get
            {
                if (TargetRenderPeriod == 0.0)
                {
                    return 0.0;
                }

                return 1.0 / TargetRenderPeriod;
            }
            set
            {
                double v = value.Clamp(0.0, 200.0);

                if (v < 1.0)
                {
                    TargetRenderPeriod = 0.0;
                }
                else
                {
                    TargetRenderPeriod = 1.0 / v;
                }
            }
        }

        /// <summary>
        /// Gets or sets a double representing the target render period, in seconds.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that RenderFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 0.005 seconds (200Hz) are clamped to 0.0. Values higher than 1.0 seconds (1Hz) are clamped to 1.0.</para>
        /// </remarks>
        public double TargetRenderPeriod
        {
            get => _targetRenderPeriod;
            set
            {
                double v = value.Clamp(0.0, 1.0);

                if (v < 0.001)
                {
                    _targetRenderPeriod = 0.0;
                }
                else
                {
                    _targetRenderPeriod = v;
                }
            }
        }

        /// <summary>
        /// Gets a double representing the actual frequency of RenderFrame events, in hertz (i.e. fps or frames per second).
        /// </summary>
        public double RenderFrequency
        {
            get
            {
                if (_renderPeriod == 0.0)
                {
                    return 1.0;
                }

                return 1.0 / _renderPeriod;
            }
        }

        /// <summary>
        /// Gets a double representing the period of RenderFrame events, in seconds.
        /// </summary>
        public double RenderPeriod => _renderPeriod;

        /// <summary>
        /// Gets a double representing the time spent in the RenderFrame function, in seconds.
        /// </summary>
        public double RenderTime
        {
            get => _renderTime;
            protected set => _renderTime = value;
        }

        #endregion

        #region Update

        /// <summary>
        /// Gets or sets a double representing the target update frequency, in hertz.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that UpdateFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 1.0Hz are clamped to 1.0Hz. Values higher than 200.0Hz are clamped to 200.0Hz.</para>
        /// </remarks>
        public double TargetUpdateFrequency
        {
            get
            {
                if (TargetUpdatePeriod == 0.0)
                {
                    return 0.0;
                }

                return 1.0 / TargetUpdatePeriod;
            }
            set
            {
                double v = value.Clamp(0.0, 200.0);

                if (v < 1.0)
                {
                    TargetUpdatePeriod = 0.0;
                }
                else
                {
                    TargetUpdatePeriod = 1.0 / v;
                }
            }
        }

        /// <summary>
        /// Gets or sets a double representing the target update period, in seconds.
        /// </summary>
        /// <remarks>
        /// <para>A value of 0.0 indicates that UpdateFrame events are generated at the maximum possible frequency (i.e. only limited by the hardware's capabilities).</para>
        /// <para>Values lower than 0.005 seconds (200Hz) are clamped to 0.0. Values higher than 1.0 seconds (1Hz) are clamped to 1.0.</para>
        /// </remarks>
        public double TargetUpdatePeriod
        {
            get => _targetUpdatePeriod;
            set
            {
                double v = value.Clamp(0.0, 1.0);

                if (v <= 0.005)
                {
                    _targetUpdatePeriod = 0.0;
                }
                else
                {
                    _targetUpdatePeriod = v;
                }
            }
        }

        /// <summary>
        /// Gets a double representing the frequency of UpdateFrame events, in hertz.
        /// </summary>
        public double UpdateFrequency
        {
            get
            {
                if (_updatePeriod == 0.0)
                {
                    return 1.0;
                }

                return 1.0 / _updatePeriod;
            }
        }

        /// <summary>
        /// Gets a double representing the period of UpdateFrame events, in seconds.
        /// </summary>
        public double UpdatePeriod => _updatePeriod;

        /// <summary>
        /// Gets a double representing the time spent in the UpdateFrame function, in seconds.
        /// </summary>
        public double UpdateTime => _updateTime;

        #endregion

        /// <summary>
        /// Runs the timer until Stop() is called.
        /// Do note that the function that calls this will be suspended until the timer is stopped.
        /// Code located after where you call this will then be executed after.
        /// </summary>
        /// <param name="updatesPerSec">FPS of update events.</param>
        /// <param name="framesPerSec">FPS of render events.</param>
        public void Run(double updatesPerSec, double framesPerSec)
        {
            _running = true;
            //try
            //{
            //Action<object, DoWorkEventArgs> work = (object sender, DoWorkEventArgs e) =>
            //{
            TargetUpdateFrequency = updatesPerSec;
            TargetRenderFrequency = framesPerSec;

            _updateWatch.Reset();
            _renderWatch.Reset();

            if (TargetUpdateFrequency != 0)
            {
                _updateWatch.Start();
            }

            if (TargetRenderFrequency != 0)
            {
                _renderWatch.Start();
            }

            while (true)
            {
                ProcessEvents();
                if (!_running)
                {
                    return;
                }

                UpdateAndRenderFrame();
            }

            //};
            //using (BackgroundWorker b = new BackgroundWorker())
            //{
            //    b.DoWork += new DoWorkEventHandler(work);
            //    //b.RunWorkerCompleted += new RunWorkerCompletedEventHandler(completed);
            //    b.RunWorkerAsync();
            //}
            //}
            //catch
            //{
            //    _running = false; 
            //    return; 
            //}
        }

        private void UpdateAndRenderFrame()
        {
            RaiseUpdateFrame();
            RaiseRenderFrame();
        }

        private void RaiseUpdateFrame()
        {
            int numUpdates = 0;
            double totalUpdateTime = 0;

            // Cap the maximum time drift to 1 second (e.g. when the process is suspended).
            double time = _updateWatch.Elapsed.TotalSeconds;
            if (time <= 0)
            {
                return;
            }

            if (time > 1.0)
            {
                time = 1.0;
            }

            // Raise UpdateFrame events until we catch up with our target update rate.
            while (_nextUpdate - time <= 0 && time > 0)
            {
                _nextUpdate -= time;
                OnUpdateFrameInternal(new FrameEventArgs(time));
                time = _updateTime = _updateWatch.Elapsed.TotalSeconds - time;
                // Stopwatches are not accurate over long time periods.
                // We accumulate the total elapsed time into the time variable
                // while reseting the Stopwatch frequently.
                _updateWatch.Reset();
                _updateWatch.Start();

                // Don't schedule a new update more than 1 second in the future.
                // Sometimes the hardware cannot keep up with updates
                // (e.g. when the update rate is too high, or the UpdateFrame processing
                // is too costly). This cap ensures we can catch up in a reasonable time
                // once the load becomes lighter.
                _nextUpdate += TargetUpdatePeriod;
                _nextUpdate = Math.Max(_nextUpdate, -1.0);

                totalUpdateTime += _updateTime;

                // Allow up to 10 consecutive UpdateFrame events to prevent the
                // application from "hanging" when the hardware cannot keep up
                // with the requested update rate.
                if (++numUpdates >= 10 || TargetUpdateFrequency == 0.0)
                {
                    break;
                }
            }

            // Calculate statistics 
            if (numUpdates > 0)
            {
                _updatePeriod = totalUpdateTime / numUpdates;
            }
        }

        private void RaiseRenderFrame()
        {
            // Cap the maximum time drift to 1 second (e.g. when the process is suspended).
            double time = _renderWatch.Elapsed.TotalSeconds;
            if (time <= 0)
            {
                return;
            }

            if (time > 1.0)
            {
                time = 1.0;
            }

            double timeLeft = _nextRender - time;

            if (timeLeft <= 0.0)
            {
                // Schedule next render event. The 1 second cap ensures
                // the process does not appear to hang.
                _nextRender = timeLeft + TargetRenderPeriod;
                if (_nextRender < -1.0)
                {
                    _nextRender = -1.0;
                }

                _renderWatch.Reset();
                _renderWatch.Start();

                if (time > 0)
                {
                    _renderPeriod = time;
                    OnRenderFrameInternal(new FrameEventArgs(time));
                    _renderTime = _renderWatch.Elapsed.TotalSeconds;
                }
            }
        }

        private void OnRenderFrameInternal(FrameEventArgs e)
        {
            if (_running)
            {
                OnRenderFrame(e);
            }
        }

        private void OnUpdateFrameInternal(FrameEventArgs e)
        {
            if (_running)
            {
                OnUpdateFrame(e);
            }
        }

        private void OnRenderFrame(FrameEventArgs e)
        {
            RenderFrame?.Invoke(this, e);
        }

        private void OnUpdateFrame(FrameEventArgs e)
        {
            UpdateFrame?.Invoke(this, e);
        }

        private bool _running;
        public bool IsRunning => _running;

        public void Stop()
        {
            _running = false;
        }

        private void ProcessEvents()
        {
            Application.DoEvents();
            Thread.Sleep(0);
        }
    }

    public class FrameEventArgs : EventArgs
    {
        private double elapsed;

        public FrameEventArgs()
        {
        }

        /// <param name="elapsed">The amount of time that has elapsed since the previous event, in seconds.</param>
        public FrameEventArgs(double elapsed)
        {
            Time = elapsed;
        }

        /// <summary>
        /// Gets a <see cref="double"/> that indicates how many seconds of time elapsed since the previous event.
        /// </summary>
        public double Time
        {
            get => elapsed;
            set => elapsed = value;
        }
    }
}