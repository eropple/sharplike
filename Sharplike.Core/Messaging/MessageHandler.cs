///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 Ed Ropple.               ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Sharplike.Core.Messaging
{
	[Serializable]
	public class MessageHandler
	{
		public delegate void HandlerFunction(Message msg);
		private Dictionary<String, HandlerFunction> handlers = new Dictionary<string, HandlerFunction>();

		public bool HandleMessage(Message msg)
		{
			HandlerFunction func;
			if (handlers.TryGetValue(msg.Name, out func))
			{
				func(msg);
				return true;
			}
			return false;
		}

		public void AssertArgumentTypes(Message msg)
		{
			HandlerFunction func;
			if (handlers.TryGetValue(msg.Name, out func))
			{
				foreach (MessageArgumentAttribute attr in Attribute.GetCustomAttributes(func.GetType()))
				{
					if (msg.Args.Length <= attr.ArgumentIndex)
					{
						throw new ArgumentException(
							String.Format("Argument {0}: Expected argument of type {1}.",
							attr.ArgumentIndex, attr.ArgumentType.FullName));
					}

					if (attr.ArgumentType.IsAssignableFrom(msg.Args[attr.ArgumentIndex].GetType()))
					{
						throw new ArgumentException(
							String.Format("Argument {0}: Expected argument of type {1}.",
							attr.ArgumentIndex, attr.ArgumentType.FullName));
					}
				}
			}
		}

		public void SetHandler(String message, HandlerFunction del)
		{
			handlers[message] = del;
		}

		public void RemoveHandler(String message)
		{
			handlers.Remove(message);
		}
	}
}
