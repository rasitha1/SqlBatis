using System.Collections.Generic;
using System.Threading;

namespace IBatisNet.DataMapper.SessionStore
{
    /// <summary>
    ///     This implementation of <see cref="ISessionStore" /> using <see cref="AsyncLocal{T}"/>.
    /// </summary>
    /// <remarks>
    ///     This replaces all previous sessions stores and the only option for .net standard.
    /// </remarks>
    public class AsyncLocalSessionStore : AbstractSessionStore
    {
        private static readonly AsyncLocal<Dictionary<string, ISqlMapSession>> LocalStore = new AsyncLocal<Dictionary<string, ISqlMapSession>>();
        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncLocalSessionStore" /> class.
        /// </summary>
        /// <param name="sqlMapperId">The SQL mapper id.</param>
        public AsyncLocalSessionStore(string sqlMapperId) : base(sqlMapperId)
        {
        }

        /// <summary>
        ///     Get the local session
        /// </summary>
        public override ISqlMapSession LocalSession => LocalStore.Value != null && LocalStore.Value.TryGetValue(sessionName, out var session) ? session : null;

        /// <summary>
        ///     Store the specified session.
        /// </summary>
        /// <param name="session">The session to store</param>
        public override void Store(ISqlMapSession session)
        {
            if (LocalStore.Value == null)
            {
                LocalStore.Value = new Dictionary<string, ISqlMapSession>();
            }
            LocalStore.Value[sessionName] = session;
        }

        /// <summary>
        ///     Remove the local session.
        /// </summary>
        public override void Dispose()
        {
            LocalStore.Value = null;
        }
    }
}