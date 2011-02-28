using System;
using System.Threading;
using System.Web;

namespace LongRunningHttpRequest.Handlers
{
    public class LongRunningRequestHandler : IHttpAsyncHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var asynch = new LongRunningOperation(cb, context, extraData);
            asynch.StartAsyncWork();
            return asynch;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
        }

        private class LongRunningOperation : IAsyncResult
        {
            private readonly AsyncCallback _callback;
            private readonly HttpContext _context;
            private readonly object _state;
            public bool IsCompleted { get; private set; }
            public object AsyncState { get; private set; }
            public WaitHandle AsyncWaitHandle { get { return null; } }
            public bool CompletedSynchronously { get { return false; } }

            public LongRunningOperation(AsyncCallback callback, HttpContext context, object state)
            {
                _callback = callback;
                _context = context;
                _state = state;
            }

            public void StartAsyncWork()
            {
                ThreadPool.QueueUserWorkItem(StartAsyncTask, null);
            }

            private void StartAsyncTask(object state)
            {
                while(true)
                {
                    _context.Response.Write("{message:\"hello\",time:\""+ DateTime.Now.ToShortTimeString() + "\"}\n");
                    _context.Response.Flush();

                    Thread.Sleep(new TimeSpan(0, 0, 3));
                }
            }
        }
    }
}