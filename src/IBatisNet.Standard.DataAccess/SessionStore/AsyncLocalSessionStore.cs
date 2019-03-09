using System.Collections.Generic;
using System.Threading;
using IBatisNet.Common;

namespace IBatisNet.DataAccess.SessionStore
{
    /// <summary>
    ///     This implementation of <see cref="ISessionStore" /> using <see cref="AsyncLocal{T}"/>.
    /// </summary>
    /// <remarks>
    ///     This replaces all previous sessions stores and the only option for .net standard.
    /// </remarks>
    public class AsyncLocalSessionStore : AbstractSessionStore
    {
        private static readonly AsyncLocal<Dictionary<string, IDalSession>> LocalStore = new AsyncLocal<Dictionary<string, IDalSession>>();
        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncLocalSessionStore" /> class.
        /// </summary>
        /// <param name="daoManagerId">The DaoManager name.</param>
        public AsyncLocalSessionStore(string daoManagerId) : base(daoManagerId)
        {
        }

        /// <summary>
        ///     Get the local session
        /// </summary>
        public override IDalSession LocalSession => LocalStore.Value != null && LocalStore.Value.TryGetValue(sessionName, out var session) ? session : null;

        /// <summary>
        ///     Store the specified session.
        /// </summary>
        /// <param name="session">The session to store</param>
        public override void Store(IDalSession session)
        {
            if (LocalStore.Value == null)
            {
                LocalStore.Value = new Dictionary<string, IDalSession>();
            }

            LocalStore.Value[sessionName] = session;
        }

        /// <summary>
        ///     Remove the local session.
        /// </summary>
        public override void Dispose()
        {
            if (LocalStore.Value != null)
            {
                LocalStore.Value.Remove(sessionName);
                if (LocalStore.Value.Count == 0)
                {
                    LocalStore.Value = null;
                }
            }            
        }
    }
}