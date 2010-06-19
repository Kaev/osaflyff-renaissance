using System;
using System.Collections.Generic;
using System.Timers;
using System.Threading;

/*--------------------------------------------------------
 * Delayer.cs - file description
 * 
 * Version: 1.0
 * Author: Adidishen
 * Created: 7/14/2009 6:29:14 AM
 * 
 * Revisions: Exos (7/17/2009) ## New Delayer Class
 * -------------------------------------------------------*/

namespace FlyffWorld
{
    /// <summary>
    /// Contains information about a delayed action.
    /// </summary>
    public class DelayedAction
    {
        /// <summary>
        /// The method to execute.
        /// </summary>
        public Action method;
        /// <summary>
        /// The System.DateTime object which determines when to call the function.
        /// </summary>
        public DateTime dtCallTime;
        /// <summary>
        /// The ID of the delayed action.
        /// </summary>
        public ulong qwID;
        /// <summary>
        /// True if the delayed action should occur until the timer is manually canceled; otherwise, false.
        /// </summary>
        public bool bRecursion;
        /// <summary>
        /// If bRecursion is set to true, determines the delay between two method executions.
        /// </summary>
        public int dwRecursionDelay;
    }
    /// <summary>
    /// Provides methods to delay and schedule function calls.
    /// </summary>
    public delegate void Action();
    public static class Delayer
    {
        /// <summary>
        /// The object used for synchronization of Delayer.
        /// </summary>
        private static object srMethodList = new object();
        /// <summary>
        /// The thread used for the main Delayer loop.
        /// </summary>
        private static Thread thread = new Thread(new ThreadStart(MainLoop));
        /// <summary>
        /// The list of scheduled functions.
        /// </summary>
        private static List<DelayedAction> lstMethods = new List<DelayedAction>();
        /// <summary>
        /// A counter which gives an unique ID to every delayed action.
        /// </summary>
        private static ulong qwCounter = 0;
        /// <summary>
        /// Registers a delayed action.
        /// </summary>
        /// <param name="method">The target delegate to execute after delaying.</param>
        /// <param name="dwDelayMilliseconds">The amount of time, in milliseconds, to postpone the method execution for.</param>
        /// <returns>The ID of the scheduled action, which can be used to remove it from the list using CancelDelayedAction.</returns>
        public static ulong RegisterDelayedAction(Action method, int dwDelayMilliseconds)
        {
            return RegisterDelayedAction(method, DateTime.Now.AddMilliseconds(dwDelayMilliseconds));
        }
        /// <summary>
        /// Registers a scheduled action.
        /// </summary>
        /// <param name="method">The target delegate to execute.</param>
        /// <param name="dtCallTime">The System.DateTime object to determine when to execute the method.</param>
        /// <returns>The ID of the scheduled action, which can be used to remove it from the list using CancelDelayedAction.</returns>
        public static ulong RegisterDelayedAction(Action method, DateTime dtCallTime)
        {
            return RegisterDelayedAction(new DelayedAction() { dtCallTime = dtCallTime, method = method });
        }
        /// <summary>
        /// Registers a recurring delayed action.
        /// </summary>
        /// <param name="method">The target delegate to execute after delaying.</param>
        /// <param name="dwDelayMilliseconds">The amount of time, in milliseconds, to postpone the method execution for.</param>
        /// <param name="dwRecursionTime">The amount of time, in milliseconds, to determine the delay between two executions of the method.</param>
        /// <returns>The ID of the scheduled action, which can be used to remove it from the list using CancelDelayedAction.</returns>
        public static ulong RegisterDelayedAction(Action method, int dwDelayMilliseconds, int dwRecursionTime)
        {
            return RegisterDelayedAction(method, DateTime.Now.AddMilliseconds(dwDelayMilliseconds), dwRecursionTime);
        }
        /// <summary>
        /// Registers a recurring scheduled action.
        /// </summary>
        /// <param name="method">The target delegate to execute after delaying.</param>
        /// <param name="dtCallTime">The System.DateTime object to determine when to execute the method.</param>
        /// <param name="dwRecursionTime">The amount of time, in milliseconds, to determine the delay between two executions of the method.</param>
        /// <returns>The ID of the scheduled action, which can be used to remove it from the list using CancelDelayedAction.</returns>
        public static ulong RegisterDelayedAction(Action method, DateTime dtCallTime, int dwRecursionTime)
        {
            return RegisterDelayedAction(new DelayedAction() { dtCallTime = dtCallTime, method = method, bRecursion = true, dwRecursionDelay = dwRecursionTime });
        }
        /// <summary>
        /// Registers a DelayedAction object.
        /// </summary>
        /// <param name="action">The DelayedAction object to register.</param>
        /// <returns>The ID of the scheduled action, which can be used to remove it from the list using CancelDelayedAction.</returns>
        private static ulong RegisterDelayedAction(DelayedAction action)
        {
            action.qwID = qwCounter++;
            lock (srMethodList)
            {
                lstMethods.Add(action);
            }
            if (!thread.IsAlive)
            {
                thread.Start();
            }
            return action.qwID;
        }
        /// <summary>
        /// Cancels a delayed/scheduled action.
        /// </summary>
        /// <param name="qwActionID">The ID of the action to cancel.</param>
        public static void CancelDelayedAction(ulong qwActionID) // fixed by Maad for v11.01
        {
            lock (srMethodList)
            {
                for (int i = 0; i < lstMethods.Count; i++)
                {
                    try
                    {
                        if (lstMethods[i].qwID == qwActionID)
                        {
                            lstMethods.RemoveAt(i);
                            return;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        /// <summary>
        /// The main Delayer loop.
        /// </summary>
        private static void MainLoop()
        {
            while (thread.IsAlive)
            {
                for (int i = 0; i < lstMethods.Count; i++)
                {
                    if (lstMethods[i].dtCallTime < DateTime.Now)
                    {
                        lstMethods[i].method();
                        if (lstMethods[i].bRecursion)
                        {
                            lstMethods[i].dtCallTime = DateTime.Now.AddMilliseconds(lstMethods[i].dwRecursionDelay);
                        }
                        else
                        {
                            lstMethods.RemoveAt(i--);
                        }
                    }
                }
                System.Threading.Thread.Sleep(1);
            }
        }

    }
}
