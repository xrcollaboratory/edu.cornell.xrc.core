using UnityEngine;
using System.Collections;

namespace XRC.Core
{
	/// <summary>
	/// An interface for implementing cammands using the command pattern. 
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Method to get the size in bytes of the command's stored data
		/// </summary>
		/// <returns>Returns the number of stored bytes.</returns>
		long GetStoredBytes();
		/// <summary>
		/// Method to execute the command
		/// </summary>
		/// <returns></returns>
		bool Execute();
		/// <summary>
		///  Method to rollback the changes made by the command
		/// </summary>
		/// <returns></returns>
		bool Rollback();
		/// <summary>
		/// Method to combine the current command with the next command
		/// </summary>
		/// <param name="p_nextCmd"></param>
		/// <returns></returns>
		bool CombineWithNext(ICommand p_nextCmd);
	}
}
