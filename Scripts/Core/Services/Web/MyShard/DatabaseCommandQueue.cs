using System;
using System.Collections;
using System.Data.Odbc;
using System.Threading;
using Server.WebConfiguration;

namespace Server.Engines.MyShard
{
    public class DatabaseCommandQueue
    {
        private readonly Queue m_Queue;
        private readonly ManualResetEvent m_Sync;
        private readonly Thread m_Thread;
        private readonly string m_CompletionString;
        private readonly string m_ConnectionString;
        private bool m_HasCompleted;
        public DatabaseCommandQueue(string completionString, string threadName)
            : this(WebConfig.WebCompileConnectionString(), completionString, threadName)
        {
        }

        public DatabaseCommandQueue(string connectionString, string completionString, string threadName)
        {
            this.m_CompletionString = completionString;
            this.m_ConnectionString = connectionString;

            this.m_Queue = Queue.Synchronized(new Queue());

            this.m_Queue.Enqueue(null); // signal connect

            /*m_Queue.Enqueue( "DELETE FROM MyShard_characters" );
            m_Queue.Enqueue( "DELETE FROM MyShard_characters_layers" );
            m_Queue.Enqueue( "DELETE FROM MyShard_characters_skills" );
            m_Queue.Enqueue( "DELETE FROM MyShard_guilds" );
            m_Queue.Enqueue( "DELETE FROM MyShard_guilds_wars" );*/

            this.m_Sync = new ManualResetEvent(true);

            this.m_Thread = new Thread(new ThreadStart(Thread_Start));
            this.m_Thread.Name = threadName;//"MyShard Database Command Queue";
            this.m_Thread.Priority = WebConfig.WebDatabaseThreadPriority;
            this.m_Thread.Start();
        }

        public bool HasCompleted
        {
            get
            {
                return this.m_HasCompleted;
            }
        }
        public void Enqueue(object obj)
        {
            lock (this.m_Queue.SyncRoot)
            {
                this.m_Queue.Enqueue(obj);
                try
                {
                    this.m_Sync.Set();
                }
                catch
                {
                }
            }
        }

        private void Thread_Start()
        {
            bool connected = false;

            OdbcConnection connection = null;
            OdbcCommand command = null;
            OdbcTransaction transact = null;

            DateTime start = DateTime.UtcNow;

            bool shouldWriteException = true;

            while (true)
            {
                this.m_Sync.WaitOne();

                while (this.m_Queue.Count > 0)
                {
                    try
                    {
                        object obj = this.m_Queue.Dequeue();

                        if (obj == null)
                        {
                            if (connected)
                            {
                                if (transact != null)
                                {
                                    try
                                    {
                                        transact.Commit();
                                    }
                                    catch (Exception commitException)
                                    {
                                        Console.WriteLine("MyShard: Exception caught when committing transaction");
                                        Console.WriteLine(commitException);

                                        try
                                        {
                                            transact.Rollback();
                                            Console.WriteLine("MyShard: Transaction has been rolled back");
                                        }
                                        catch (Exception rollbackException)
                                        {
                                            Console.WriteLine("MyShard: Exception caught when rolling back transaction");
                                            Console.WriteLine(rollbackException);
                                        }
                                    }
                                }

                                try
                                {
                                    connection.Close();
                                }
                                catch
                                {
                                }

                                try
                                {
                                    connection.Dispose();
                                }
                                catch
                                {
                                }

                                try
                                {
                                    command.Dispose();
                                }
                                catch
                                {
                                }

                                try
                                {
                                    this.m_Sync.Close();
                                }
                                catch
                                {
                                }

                                Console.WriteLine(this.m_CompletionString, (DateTime.UtcNow - start).TotalSeconds);
                                this.m_HasCompleted = true;

                                return;
                            }
                            else
                            {
                                try
                                {
                                    connected = true;
                                    connection = new OdbcConnection(this.m_ConnectionString);
                                    connection.Open();
                                    command = connection.CreateCommand();

                                    if (WebConfig.WebUseTransactions)
                                    {
                                        transact = connection.BeginTransaction();
                                        command.Transaction = transact;
                                    }
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        if (transact != null)
                                            transact.Rollback();
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        if (connection != null)
                                            connection.Close();
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        if (connection != null)
                                            connection.Dispose();
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        if (command != null)
                                            command.Dispose();
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        this.m_Sync.Close();
                                    }
                                    catch
                                    {
                                    }

                                    Console.WriteLine("MyShard: Unable to connect to the database");
                                    Console.WriteLine(e);
                                    this.m_HasCompleted = true;
                                    return;
                                }
                            }
                        }
                        else if (obj is string)
                        {
                            command.CommandText = (string)obj;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            string[] parms = (string[])obj;

                            command.CommandText = parms[0];

                            if (command.ExecuteScalar() == null)
                            {
                                command.CommandText = parms[1];
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (shouldWriteException)
                        {
                            Console.WriteLine("MyShard: Exception caught in database thread");
                            Console.WriteLine(e);
                            shouldWriteException = false;
                        }
                    }
                }

                lock (this.m_Queue.SyncRoot)
                {
                    if (this.m_Queue.Count == 0)
                        this.m_Sync.Reset();
                }
            }
        }
    }
}