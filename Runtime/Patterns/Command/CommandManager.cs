using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XRC.Core
{
	/// <summary>
	/// Manages the execution, undo, and redo of commands in the application.
	/// </summary>
	public class CommandManager
	{
		private static CommandManager s_instance = null;
		public static CommandManager Instance { get{ return s_instance == null ? s_instance = new CommandManager() : s_instance; } }

		private List<ICommand> m_undoCommands = new List<ICommand>();
		private List<ICommand> m_redoCommands = new List<ICommand>();

		// Provided in Utils
		private UtilityOnDestroyHandler m_sceneDestroyHandler = null;
		
		
		/// <summary>
		/// Gets or sets a value indicating whether the CommandManager is destroyed when a new scene is loaded.
		/// </summary>
		public bool IsDestroyedOnSceneLoad
		{
			get{ return m_sceneDestroyHandler != null; }
			set
			{
				if (value)
				{
					
					if (m_sceneDestroyHandler == null)
					{
						GameObject sceneDestroyGO = new GameObject("UR_CommandMgrSceneDestroyHandler");
						m_sceneDestroyHandler = sceneDestroyGO.AddComponent<UtilityOnDestroyHandler>();
						m_sceneDestroyHandler.m_onDestroy += Reset;
					}
				}
				else
				{
					if (m_sceneDestroyHandler != null)
					{
						m_sceneDestroyHandler.DestroyWithoutHandling();
						m_sceneDestroyHandler = null;
					}
				}
			}
		}

		private long m_commandsStoredBytesLimit = 128*1024*1024; // default 128MB (mega byte)
		
		/// <summary>
		/// Gets or sets the maximum amount of memory that can be used to store command data.
		/// </summary>
		public long StoredBytesLimit
		{
			get{ return m_commandsStoredBytesLimit; }
			set
			{
				m_commandsStoredBytesLimit = value;
				LimitMemoryUsage();
			}
		}

		private long m_commandsStoredBytes = 0;
		
		/// <summary>
		/// Gets the total amount of memory used to store command data.
		/// </summary>
		public long StoredBytes { get{ return m_commandsStoredBytes; } }

		/// <summary>
		/// Gets a value indicating whether there are redoable commands.
		/// </summary>
		public bool IsRedoable { get{ return m_redoCommands.Count > 0;} }
		
		/// <summary>
		/// Gets a value indicating whether there are undoable commands.
		/// </summary>
		public bool IsUndoable { get{ return m_undoCommands.Count > 0;} }


		/// <summary>
		/// Adds a command to the command manager.
		/// </summary>
		/// <param name="p_cmd">The command to add.</param>
		/// <param name="p_isAlreadyExecuted">Indicates whether the command has already been executed.</param>
		public void Add(ICommand p_cmd, bool p_isAlreadyExecuted)
		{
			if (p_cmd != null)
			{
				if (!p_isAlreadyExecuted) { p_cmd.Execute(); }
				
				if (m_undoCommands.Count > 0)
				{
					ICommand lastCmd = m_undoCommands[m_undoCommands.Count-1];
					long lastCmdBytes = lastCmd.GetStoredBytes();
					if (lastCmd.CombineWithNext(p_cmd))
					{
						ClearRedoCommands();
						m_commandsStoredBytes += lastCmd.GetStoredBytes() - lastCmdBytes;
						LimitMemoryUsage();
						return; // commands are combined now
					}
				}
				m_undoCommands.Add(p_cmd);
				ClearRedoCommands();
				m_commandsStoredBytes += p_cmd.GetStoredBytes();
				LimitMemoryUsage();
			}
			else
			{
				Debug.LogError("UR_CommandMgr: Add: p_cmd is null!");
			}
		}
		/// <summary>
		/// Executes a command and adds it to the command manager.
		/// </summary>
		/// <param name="p_cmd">The command to execute.</param>
		public void Execute(ICommand p_cmd)
		{
			Add(p_cmd, false);
		}
		/// <summary>
		/// Redoes the last undone command.
		/// </summary>
		/// <returns>True if the redo was successful, false otherwise.</returns>
		public bool Redo()
		{
			if (IsRedoable)
			{
				ICommand cmd = m_redoCommands[m_redoCommands.Count-1];
				m_redoCommands.RemoveAt(m_redoCommands.Count-1);
				m_commandsStoredBytes -= cmd.GetStoredBytes();
				cmd.Execute();
				m_commandsStoredBytes += cmd.GetStoredBytes();
				m_undoCommands.Add(cmd);
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// Undoes the last executed command.
		/// </summary>
		/// <returns>True if the undo was successful, false otherwise.</returns>
		public bool Undo()
		{
			if (IsUndoable)
			{
				ICommand cmd = m_undoCommands[m_undoCommands.Count-1];
				m_undoCommands.RemoveAt(m_undoCommands.Count-1);
				m_commandsStoredBytes -= cmd.GetStoredBytes();
				cmd.Rollback();
				m_commandsStoredBytes += cmd.GetStoredBytes();
				m_redoCommands.Add(cmd);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Resets the command manager, clearing all commands.
		/// </summary>
		public void Reset()
		{
			m_undoCommands.Clear();
			m_redoCommands.Clear();
			m_commandsStoredBytes = 0;
		}

		private void ClearRedoCommands()
		{
			for (int i = 0; i < m_redoCommands.Count; i++)
			{
				m_commandsStoredBytes -= m_redoCommands[i].GetStoredBytes();
			}
			m_redoCommands.Clear();
		}

		private void LimitMemoryUsage()
		{
			while (m_commandsStoredBytes > m_commandsStoredBytesLimit && m_undoCommands.Count > 0)
			{
				// remove oldest command
				ICommand oldestCmd = m_undoCommands[0];
				m_undoCommands.RemoveAt(0);
				m_commandsStoredBytes -= oldestCmd.GetStoredBytes();
			}
			// in theory the redo list could use a lot storage space, but since the redo list is cleared after every new command, we do not check it here
			// => this means, that reducing the StoredBytesLimit on runtime will not delete the redo stack, it will be deleted when a new command is executed
		}
	}



}
