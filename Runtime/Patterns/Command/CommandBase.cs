using UnityEngine;
using System.Collections;

namespace XRC.Core
{
	
	/// <summary>
	/// Provides an abstract base class for commands that implement the ICommand interface.
	/// </summary>
	public abstract class CommandBase : ICommand
	{
		/// <summary>
		/// Flag indicating whether the command has been executed or not
		/// </summary>
		protected bool m_isExecuted = false;

		/// <summary>
		/// Abstract method to get the size in bytes of the command's stored data
		/// </summary>
		/// <returns></returns>
		public abstract long GetStoredBytes();
		
		/// <summary>
		/// Abstract method to combine the current command with the next command
		/// </summary>
		/// <param name="p_nextCmd"></param>
		/// <returns></returns>
		public abstract bool CombineWithNext(ICommand p_nextCmd);

		/// <summary>
		/// Method to execute the command
		/// </summary>
		/// <returns></returns>
		public virtual bool Execute()
		{
			if (m_isExecuted)
			{
				// Error message indicating that the command was already executed
				Debug.LogError("UR_CommandBase: called 'Execute', but this command was already executed!");
				return false;
			}

			// Mark the command as executed
			m_isExecuted = true;
			return true;
		}

		/// <summary>
		/// Method to rollback the changes made by the command
		/// </summary>
		/// <returns></returns>
		public virtual bool Rollback()
		{
			if (!m_isExecuted)
			{
				// Error message indicating that the command was not yet rolled back
				Debug.LogError("UR_CommandBase: called 'Rollback', but this command was not yet rolled back!");
				return false;
			}

			// Mark the command as not executed (rolled back)
			m_isExecuted = false;
			return true;
		}
	}
}
