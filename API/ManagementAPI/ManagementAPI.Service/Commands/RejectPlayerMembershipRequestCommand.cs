﻿using System;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class RejectPlayerMembershipRequestCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; private set; }

        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the reject membership request request.
        /// </summary>
        /// <value>
        /// The reject membership request request.
        /// </value>
        public RejectMembershipRequestRequest RejectMembershipRequestRequest { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RejectPlayerMembershipRequestCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="commandId">The command identifier.</param>
        private RejectPlayerMembershipRequestCommand(Guid playerId, Guid golfClubId, RejectMembershipRequestRequest request, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.GolfClubId = golfClubId;
            this.RejectMembershipRequestRequest = request;
        }
        #endregion

        #region public static RejectPlayerMembershipRequestCommand Create(Guid playerId, Guid golfClubId, RejectMembershipRequestRequest request)
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static RejectPlayerMembershipRequestCommand Create(Guid playerId, Guid golfClubId, RejectMembershipRequestRequest request)
        {
            return new RejectPlayerMembershipRequestCommand(playerId, golfClubId, request, Guid.NewGuid());
        }
        #endregion
    }
}