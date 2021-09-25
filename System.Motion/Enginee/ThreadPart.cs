using System;
using System.Threading;
using System.Threading.Tasks;
using System.Interfaces;
using System.ToolKit;

namespace System.Enginee
{
    /// <summary>
    ///     支持测试线程的设备部件
    /// </summary>
    public abstract class ThreadPart : IThreadPart
    {
        private CancellationTokenSource _cancelTokenSource;

        protected ThreadPart()
        {
            ContinueEvent = new ManualResetEvent(true);
            StopEvent = new ManualResetEvent(false);
        }

        /// <summary>
        ///     将指派给测试线程任务的 <see cref="P:System.Threading.Tasks.TaskFactory.CancellationToken" />。
        /// </summary>
        protected CancellationToken CancelToken
        {
            get { return _cancelTokenSource.Token; }
        }

        /// <summary>
        ///     继续信号量。
        /// </summary>
        protected ManualResetEvent ContinueEvent { get; set; }

        /// <summary>
        ///     停止信号量。
        /// </summary>
        protected ManualResetEvent StopEvent { get; set; }

        private void Clean()
        {
            var ini = this as INeedClean;
            if (ini != null)
                ini.Clean();
            _cancelTokenSource = null;
        }

        #region Implementation of IThreadPart

        public RunStates RunState
        {
            get
            {
                if(_cancelTokenSource == null) return RunStates.Stop;
                return ContinueEvent.WaitOne(0) ? RunStates.Running : RunStates.Pause;
            }
        }

        /// <summary>
        ///     运行部件驱动线程。
        /// </summary>
        /// <param name="runningMode">运行模式。</param>
        public virtual void Run(RunningModes runningMode)
        {
                StopEvent.Reset();
            if (_cancelTokenSource == null)//多线程操作
            {
                _cancelTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Running(runningMode);                    
                    }
                    catch (OperationCanceledException)
                    {
                        LogHelper.Debug("设备驱动程序异常");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Debug("设备驱动程序异常", ex);
                    }
                },TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning)
                .ContinueWith(task => Clean());
            }
        }

        /// <summary>
        ///     暂停任务线程
        /// </summary>
        public virtual void Pause()
        {
            if (_cancelTokenSource == null) return;
            ContinueEvent.Reset();
            //Log.Debug("{0}部件暂停。", Name);
        }

        /// <summary>
        ///     唤醒任务线程
        /// </summary>
        public virtual void Resume()
        {
            if (_cancelTokenSource == null) return;
            ContinueEvent.Set();
            //Log.Debug("{0}部件唤醒。", Name);
        }

        /// <summary>
        ///     停止任务线程
        /// </summary>
        public virtual void Stop()
        {
            if (_cancelTokenSource == null) return;
            if (_cancelTokenSource.Token.CanBeCanceled)
            {
                StopEvent.Set();
                _cancelTokenSource.Cancel();
                //Log.Debug("{0}部件停止。", Name);
            }
        }

        /// <summary>
        ///     驱动部件运行。
        /// </summary>
        /// <param name="runningMode">运行模式。</param>
        public abstract void Running(RunningModes runningMode);

        #endregion
    }
}