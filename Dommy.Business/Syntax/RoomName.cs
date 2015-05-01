//-----------------------------------------------------------------------
// <copyright file="RoomName.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    /// <summary>
    /// Name of the room.
    /// </summary>
    public class RoomName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomName"/> class.
        /// </summary>
        /// <param name="name">Name of the room.</param>
        /// <param name="prefixName">Prefixed name of the room.</param>
        /// <param name="gender">Gender of the room.</param>
        public RoomName(string name, string prefixName, Gender gender)
        {
            this.Name = name;
            this.PrefixName = prefixName;
            this.Gender = gender;
        }

        /// <summary>
        /// Gets name of the room.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets prefixed name of the room.
        /// </summary>
        public string PrefixName { get; private set; }

        /// <summary>
        /// Gets gender of the name room.
        /// </summary>
        /// <returns></returns>
        public Gender Gender { get; private set; }

        /// <summary>
        /// To string override of the room name.
        /// </summary>
        /// <returns>Name of the room.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
