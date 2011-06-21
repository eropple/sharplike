///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 2010 Ed Ropple.          ///
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
using Mono.Addins;
using Mono.Addins.CecilReflector;
using System.IO;
using System.Reflection;
using Sharplike;
using Sharplike.Core.Rendering;
using Sharplike.Core.Audio;
using Sharplike.Core.Input;
using Sharplike.Core.Scripting;
using Sharplike.Core.Runtime;
using Sharplike.Core.Messaging;
using Sharplike.Core.Scheduling;


namespace Sharplike.Core
{
    /// <summary>
    /// Game is the core entry point of the Sharplike engine.
    /// </summary>
    /// <example>
    /// Game.Initialize();
    /// 
    /// Game.SetRenderSystem("OpenTK");
    /// Game.CreateRenderWindow(...);
    /// 
    /// Game.SetInputSystem("OpenTK");
    /// Game.SetAudioSystem("OpenTK");
    /// 
    /// g.Run(new StepwiseGameLoop(new StepwiseGameLoop.Execute(MyGame)));
    /// 
    /// g.Terminate();
    /// </example>
    public static class Game
    {
		private static readonly EventArgs internedEventArg = new EventArgs();

		private static InputSystem inputSystem = null;
        private static AbstractRenderSystem renderSystem = null;
        private static AbstractAudioEngine audioSystem = null;

        private static GameMessenger messenger = new GameMessenger();

		private static PostOffice post = new PostOffice();
		private static Dictionary<String, List<IMessageReceiver>> subscriptions = new Dictionary<string, List<IMessageReceiver>>();

		/// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="basedir">The base application directory.</param>
        public static void Initialize(String basedir)
        {
            SubscribeToChannels(messenger);

            BaseDirectory = Path.GetFullPath(basedir);
            if (!Directory.Exists(BaseDirectory))
                throw new DirectoryNotFoundException("Specified base directory does not exist.");

            if (AddinManager.IsInitialized)
                return;

            AddinManager.Initialize(PathTo("addins"));
#if DEBUG
            AddinManager.Registry.Rebuild(new ConsoleProgressStatus(true));
#else
            AddinManager.Registry.Update();
#endif

            Game.InputSystem = new InputSystem();

			Game.Time = 0;
        }

        /// <summary>
        /// Creates a new game. The base directory will be the location of the .exe file that was run.
        /// </summary>
        public static void Initialize()
        {
            Initialize(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
        }

		#region Messaging
		/// <summary>
		/// Thread safe. Broadcasts a message to a channel.
		/// </summary>
		/// <param name="channel">The channel to send to</param>
		/// <param name="messagename">The name of the message to send</param>
		/// <param name="args">The arguments for the message</param>
        public static void SendMessage(String channel, String messagename, params Object[] args)
        {
			List<IMessageReceiver> subscribers;
			lock (subscriptions)
			{
				if (subscriptions.TryGetValue(channel, out subscribers))
				{
					foreach (IMessageReceiver r in subscribers)
					{
						SendMessage(r, messagename, args);
					}
				}
			}
        }

		/// <summary>
		/// Thread safe. Broadcasts a message to a channel.
		/// </summary>
		/// <param name="channel">The channel to send to</param>
		/// <param name="messagename">The name of the message to send</param>
		public static void SendMessage(String channel, String messagename)
		{
			SendMessage(channel, messagename, new Object[0]);
		}

		/// <summary>
		/// Thread safe. Send a message to a particular target.
		/// </summary>
		/// <param name="target">The target to send the message to.</param>
		/// <param name="messagename">The name of the message to send</param>
		/// <param name="args">The arguments for the message</param>
        public static void SendMessage(IMessageReceiver target, String messagename, params Object[] args)
        {
			Message m = new Message(messagename, null, args);
			target.AssertArgumentTypes(m);
			post.EnqueueMessage(target, m);
        }

		/// <summary>
		/// Thread safe. Send a message to a particular target.
		/// </summary>
		/// <param name="target">The target to send the message to.</param>
		/// <param name="messagename">The name of the message to send</param>
		public static void SendMessage(IMessageReceiver target, String messagename)
		{
			SendMessage(target, messagename, new Object[0]);
		}

		/// <summary>
		/// Thread safe. Subscribes a message receiver to a particular channel.
		/// </summary>
		/// <remarks>If an object is subscribed to a channel, it MUST be unsubscribed on destruction.</remarks>
		/// <see cref="Sharplike.Core.Game.Unsubscribe"/>
		/// <param name="chan">The channel to subscribe to.</param>
		/// <param name="subscriber">The object that will receive messages.</param>
        public static void SubscribeToChannel(String chan, IMessageReceiver subscriber)
        {
			List<IMessageReceiver> channel;
			lock (subscriptions)
			{
				if (!subscriptions.TryGetValue(chan, out channel))
				{
					channel = new List<IMessageReceiver>();
					subscriptions.Add(chan, channel);
				}

				channel.Add(subscriber);
			}
        }

		/// <summary>
		/// Thread safe. Automatically subscribe to all channels specified in class attributes.
		/// </summary>
		/// <remarks>If an object is subscribed to a channel, it MUST be unsubscribed on destruction.</remarks>
		/// <see cref="Sharplike.Core.Game.Unsubscribe"/>
		/// <seealso cref="Sharplike.Core.Messaging.ChannelSubscriberAttribute"/>
		/// <param name="subscriber">The object to create subscriptions for.</param>
        public static void SubscribeToChannels(IMessageReceiver subscriber)
        {
			foreach (ChannelSubscriberAttribute attr in
				Attribute.GetCustomAttributes(subscriber.GetType()))
			{
				SubscribeToChannel(attr.Channel, subscriber);
			}
        }

		/// <summary>
		/// Thread safe. Unsubscribes an object from the whole post system. This object will no longer receive messages.
		/// </summary>
		/// <param name="subscriber">The object to unsubscribe.</param>
		public static void Unsubscribe(IMessageReceiver subscriber)
		{
			post.RemoveReceiver(subscriber);
			lock (subscriptions)
			{
				foreach (KeyValuePair<String, List<IMessageReceiver>> kvp in subscriptions)
				{
					kvp.Value.Remove(subscriber);
				}
			}
		}
		#endregion

		#region Scheduling
		private static List<IScheduledTask> tasks = new List<IScheduledTask>();

		/// <summary>
		/// Thread Safe. Registers a new scheduled task to be run every step.
		/// </summary>
		/// <param name="task">The task to schedule.</param>
		public static void RegisterTask(IScheduledTask task)
		{
			lock (tasks)
			{
				if (!tasks.Contains(task))
					tasks.Add(task);
			}
		}

		/// <summary>
		/// Thread Safe. Unregisters a task from the task queue.
		/// </summary>
		/// <param name="task">The task to unregister.</param>
		public static void UnregisterTask(IScheduledTask task)
		{
			lock (tasks)
			{
				tasks.Remove(task);
			}
		}
		#endregion

		/// <summary>
        /// Sets the input system to use. WARNING: Many input systems (such as OpenTK) reference the rendering system
        /// for a window handle. Consult the documentation of your input system for details, but in general
        /// be sure to initialize your rendering system first.
        /// </summary>
        /// <param name="sys">The name of the input system to use.</param>
        public static void SetInputSystem(String sys)
        {
            if (Game.inputSystem.Provider != null)
                throw new InvalidOperationException("Game.SetInputSystem() may only be called once.");
            TypeExtensionNode node = (TypeExtensionNode)AddinManager.GetExtensionNode(String.Format("/Sharplike/Input/{0}", sys));
			if (node == null)
				throw new ArgumentException("Specified input system could not be found.", "sys");
            InputSystem.Provider = (AbstractInputProvider)node.CreateInstance();
        }

        /// <summary>
        /// Sets the rendering engine to use.
        /// </summary>
        /// <param name="sys">The name of the rendering engine. Default is "OpenTK".</param>
        public static void SetRenderSystem(String sys)
        {
            if (Game.renderSystem != null)
                throw new InvalidOperationException("Game.SetRenderSystem() may only be called once.");
			TypeExtensionNode node = (TypeExtensionNode)AddinManager.GetExtensionNode(String.Format("/Sharplike/Rendering/{0}", sys));
			if (node == null)
				throw new ArgumentException("Specified render system could not be found.", "sys");
            RenderSystem = (AbstractRenderSystem)node.CreateInstance();
        }

        /// <summary>
        /// Sets the audio engine to use.
        /// </summary>
        /// <param name="sys">The name of the audio engine. Default is "OpenTK".</param>
        public static void SetAudioSystem(String sys)
        {
            if (Game.audioSystem != null)
                throw new InvalidOperationException("Game.SetAudioSystem() may only be called once.");
            ExtensionNodeList en = AddinManager.GetExtensionNodes("/Sharplike/Audio");
			TypeExtensionNode node = (TypeExtensionNode)AddinManager.GetExtensionNode(String.Format("/Sharplike/Audio/{0}", sys));
			if (node == null)
				throw new ArgumentException("Specified audio system could not be found.", "sys");
            AudioSystem = (AbstractAudioEngine)node.CreateInstance();
        }

        /// <summary>
        /// Starts the game's processing. Make sure that the game has been initialized through a call to
        /// Initialize() first. Game call will not return until the user has quit the game.
        /// </summary>
        /// <param name="loop">The AbstractGameLoop that's responsible for delegating to game logic.</param>
        public static void Run(AbstractGameLoop loop)
        {
			if (Scheduler == null)
				Scheduler = new SingleThreadedScheduler();

			if (Game.OnGameInitialization != null)
                Game.OnGameInitialization(null, internedEventArg);
            loop.Begin();
			if (Game.OnGameTermination != null)
                Game.OnGameTermination(null, internedEventArg);
        }

		/// <summary>
		/// Starts the game's processing and immediately returns. For expert users only!
		/// </summary>
		public static void Run()
		{
			if (Scheduler == null)
				Scheduler = new SingleThreadedScheduler();

			if (Game.OnGameInitialization != null)
				Game.OnGameInitialization(null, internedEventArg);
		}

		/// <summary>
		/// Stops the game from running. For expert users only!
		/// </summary>
		public static void Stop()
		{
			if (Game.OnGameTermination != null)
				Game.OnGameTermination(null, internedEventArg);
		}

        /// <summary>
        /// Processes the game's core subsystems. Game function should not be called except by expert users.
        /// </summary>
        public static void Process()
        {
			

            if (GameProcessing != null)
                GameProcessing(null, internedEventArg);

            if (AudioSystem != null)
                AudioSystem.Process();

            if (RenderSystem != null)
                RenderSystem.Process();

			++Game.Time;
			PumpMessages();
        }

		/// <summary>
		/// Pumps game messages. For expert users only!
		/// </summary>
		public static void PumpMessages()
		{
			post.PumpMessages();
		}

		/// <summary>
		/// Processes a single step of game code. This will be called every frame for realtime games. 
		/// Should not be called except by expert users. Even they should reconsider.
		/// </summary>
		public static void Step()
		{
			Process();

			//List<IScheduledTask> alltasks;
			//lock (tasks)
			//{
			//    alltasks = new List<IScheduledTask>(tasks);
			//}
			//Scheduler.Process(alltasks.AsReadOnly());
			Scheduler.Process();
		}

        /// <summary>
        /// The system in charge of graphical output.
        /// </summary>
        public static AbstractRenderSystem RenderSystem
        {
            get { return Game.renderSystem; }
			private set { Game.renderSystem = value; }
        }

        /// <summary>
        /// The system in charge of audio playback.
        /// </summary>
        public static AbstractAudioEngine AudioSystem
        {
			get { return Game.audioSystem; }
			private set { Game.audioSystem = value; }
        }

        /// <summary>
        /// The system in charge of user input.
        /// </summary>
        public static InputSystem InputSystem
        {
			get { return Game.inputSystem; }
			private set { Game.inputSystem = value; }
        }

		public static IScheduler Scheduler
		{
			get;
			set;
		}

        /// <summary>
        /// The system in charge of dynamic scripting.
        /// </summary>
        public static ScriptingSystem Scripting
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns a full system path to the given location. Properly accounts for
		/// system differences. Regardless of your development platform, use the foreslash
		/// ("/") to separate path elements.
		/// </summary>
		/// <param name="location">The path to generate off of the game's BaseDirectory.</param>
		/// <returns>The full system path to the specified location.</returns>
        public static String PathTo(String location)
        {
			return Game.PathTo(location.Split('/'));
        }
		/// <summary>
		/// Builds a full system path to the specified location from an IEnumerable of
		/// path components.
		/// </summary>
		/// <param name="location">An enumerable collection of path components.</param>
		/// <returns>The full system path to the specified location.</returns>
        public static String PathTo(IEnumerable<String> location)
		{
			String foo = Game.BaseDirectory;
			foreach (String s in location)
				foo = Path.Combine(foo, s);
			return foo;
		}

        /// <summary>
        /// The base application directory of the current game.
        /// </summary>
        public static String BaseDirectory
        {
            get;
            private set;
        }

		public static Int64 Time
		{
			get;
			internal set;
		}



		/// <summary>
		/// Used to set the value of the game loop's current tick. This is NOT
		/// being used as the property's setter because it must be stressed that
		/// this is a BAD THING TO MESS WITH. Only do so in established, defined
		/// situations (for example, setting the tick value of a restored saved
		/// game).
		/// </summary>
		/// <param name="newTickValue">The new value of the current tick.</param>
		/// <returns>The old value of the current tick.</returns>
		public static Int64 SetTick(Int64 newTickValue)
		{
			Int64 a = Game.Time;
			Game.Time = newTickValue;
			return a;
		}

		/// <summary>
		/// Call Game when your application's done. Disposes of Game and fires any
		/// OnGameTermination events.
		/// </summary>
        public static void Terminate()
		{
			if (Game.Terminated) return;

            if (AudioSystem != null)
                AudioSystem.Dispose();
			
			if (InputSystem.Provider != null)
				InputSystem.Provider.Dispose();

			if (RenderSystem != null)
				RenderSystem.Dispose();

			if (Game.OnGameTermination != null)
				Game.OnGameTermination(null, Game.internedEventArg);
			Game.Terminated = true;
			Unsubscribe(messenger);
		}

        public static Boolean Terminated
		{
			get;
			private set;
		}

        public delegate void GameProcessingEventHandler(object sender, EventArgs e);

        /// <summary>
        /// GameProcessing is called just before a frame is rendered to the screen.
        /// It may be used for graphical map effects, but should not contain game logic.
        /// </summary>
        public static event EventHandler<EventArgs> GameProcessing;

		/// <summary>
		/// OnGameInitialization is an event that will fire after the game loop
		/// is initialized, but before it is run for the first time.
		/// </summary>
        public static event EventHandler<EventArgs> OnGameInitialization;

		/// <summary>
		/// Called after the game loop finally terminates.
		/// </summary>
        public static event EventHandler<EventArgs> OnGameTermination;
    }

    [ChannelSubscriber("Game")]
    public class GameMessenger : IMessageReceiver
    {
        internal GameMessenger()
        {
			mhandler.SetHandler("Terminate", Message_Terminate);
        }

        public void OnMessage(Message msg)
        {
            mhandler.HandleMessage(msg);
        }

		void Message_Terminate(Message msg)
		{
			Game.Terminate();
		}

		public void AssertArgumentTypes(Message msg)
		{
			mhandler.AssertArgumentTypes(msg);
		}

		private MessageHandler mhandler = new MessageHandler();
	}
}
