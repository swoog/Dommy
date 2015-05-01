//-----------------------------------------------------------------------
// <copyright file="ElementName.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    /// <summary>
    /// Name of the element.
    /// </summary>
    public class ElementName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementName"/> class.
        /// </summary>
        /// <param name="name">Name of the element.</param>
        /// <param name="prefixName">Prefixed name of the element.</param>
        /// <param name="gender">Gender of the element.</param>
        public ElementName(string name, string prefixName, Gender gender)
        {
            this.Name = name;
            this.PrefixName = prefixName;
            this.Gender = gender;
        }

        /// <summary>
        /// Gets name of the element.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets prefixed name of the element.
        /// </summary>
        public string PrefixName { get; private set; }

        /// <summary>
        /// Gets gender of the name element.
        /// </summary>
        /// <returns></returns>
        public Gender Gender { get; private set; }

        /// <summary>
        /// To string override of the element name.
        /// </summary>
        /// <returns>Name of the element.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
