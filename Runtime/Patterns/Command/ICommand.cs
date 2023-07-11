using UnityEngine;
using System.Collections;

namespace XRC.Core
{
	public interface ICommand
	{
		long GetStoredBytes();
		bool Execute();
		bool Rollback();
		bool CombineWithNext(ICommand p_nextCmd);
	}
}
