//-----------------------------------------------------------------------
// <copyright file="BaseAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Action
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dommy.Model;

    /// <summary>
    /// Base class for all action.
    /// </summary>
    /// <typeparam name="T">Action data type.</typeparam>
    public abstract class BaseAction<T> : IAction
        where T : ActionData, new()
    {
        /// <summary>
        /// Gets or sets id of action.
        /// </summary>
        public int Id
        {
            get 
            { 
                return this.Data.Id; 
            }

            set
            {
                if (this.Data == null)
                {
                    this.Data = new T();
                }

                this.Data.Id = value;
            }
        }

        /// <summary>
        /// Gets or sets name of action.
        /// </summary>
        public string Name
        {
            get 
            { 
                return this.Data.Name; 
            }

            set
            {
                if (this.Data == null)
                {
                    this.Data = new T();
                }

                this.Data.Name = value;
            }
        }

        protected T CreateData()
        {
            return new T() { Id = this.Id, Name = this.Name };
        }

        /// <summary>
        /// Gets all matched sentences with the name engine.
        /// </summary>
        public abstract IList<string> Sentences { get; }

        /// <summary>
        /// Gets all matched sentences without the name engine.
        /// </summary>
        public virtual IList<string> SentencesNoPrefixName
        {
            get { return new string[0]; }
        }

        /// <summary>
        /// Gets or sets generic data of action.
        /// </summary>
        public T TData
        {
            get { return (T)this.Data; }
            set { this.Data = value; }
        }

        /// <summary>
        /// Gets data of action.
        /// </summary>
        public ActionData Data { get; private set; }

        /// <summary>
        /// Run action match.
        /// </summary>
        /// <param name="sentence">Information about sentence match.</param>
        /// <returns>Result to execute.</returns>
        public abstract IResult RunAction(ISentence sentence);

        /// <summary>
        /// Run action match.
        /// </summary>
        /// <param name="data">Information about sentence match.</param>
        /// <returns>Result to execute.</returns>
        public IResult RunAction(Model.ActionData data)
        {
            this.Data = data;
            return this.RunAction();
        }

        /// <summary>
        /// Run action without sentence match.
        /// </summary>
        /// <returns>Result to execute.</returns>
        public abstract IResult RunAction();
    }
}
