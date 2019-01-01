﻿using System;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class PlayerClubMembershipRequestCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; private set; }

        /// <summary>
        /// Gets the club identifier.
        /// </summary>
        /// <value>
        /// The club identifier.
        /// </value>
        public Guid ClubId { get; private set; }

        #endregion

        #region Constructor                
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerClubMembershipRequestCommand"/> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private PlayerClubMembershipRequestCommand(Guid playerId, Guid clubId, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.ClubId = clubId;
        }
        #endregion

        #region public static PlayerClubMembershipRequestCommand Create()                
        /// <summary>
        /// Creates the specified player identifier.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        public static PlayerClubMembershipRequestCommand Create(Guid playerId, Guid clubId)
        {
            return new PlayerClubMembershipRequestCommand(playerId, clubId, Guid.NewGuid());
        }
        #endregion
    }
}