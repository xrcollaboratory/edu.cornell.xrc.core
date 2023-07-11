using UnityEngine;
using System.Collections;

namespace XRC.Core
{
	public abstract class CommandBase : ICommand
	{
		protected bool m_isExecuted = false;

		public abstract long GetStoredBytes();
		public abstract bool CombineWithNext(ICommand p_nextCmd);

		public virtual bool Execute()
		{
			if (m_isExecuted)
			{
				Debug.LogError("UR_CommandBase: called 'Execute', but this command was already executed!");
				return false;
			}

			m_isExecuted = true;
			return true;
		}

		public virtual bool Rollback()
		{
			if (!m_isExecuted)
			{
				Debug.LogError("UR_CommandBase: called 'Rollback', but this command was not yet rolled back!");
				return false;
			}

			m_isExecuted = false;
			return true;
		}
	}
}
