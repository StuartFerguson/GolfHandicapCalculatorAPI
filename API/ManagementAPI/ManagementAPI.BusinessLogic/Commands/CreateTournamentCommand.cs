﻿namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Shared.CommandHandling;

    public class CreateTournamentCommand : Command<CreateTournamentResponse>
    {
        #region Properties

        public Guid TournamentId { get; private set; }

        /// <summary>
        /// Gets or sets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { private set; get; }

        /// <summary>
        /// Gets or sets the create tournament request.
        /// </summary>
        /// <value>
        /// The create tournament request.
        /// </value>
        public CreateTournamentRequest CreateTournamentRequest { private set; get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTournamentCommand" /> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="createTournamentRequest">The create tournament request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateTournamentCommand(Guid tournamentId, Guid golfClubId, CreateTournamentRequest createTournamentRequest, Guid commandId) : base(commandId)
        {
            this.TournamentId = tournamentId;
            this.GolfClubId = golfClubId;
            this.CreateTournamentRequest = createTournamentRequest;            
        }
        #endregion

        #region public static CreateTournamentCommand Create()        
        /// <summary>
        /// Creates the specified create tournament request.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="createTournamentRequest">The create tournament request.</param>
        /// <returns></returns>
        public static CreateTournamentCommand Create(Guid tournamentId, Guid golfClubId, CreateTournamentRequest createTournamentRequest)
        {
            return new CreateTournamentCommand(tournamentId, golfClubId, createTournamentRequest, Guid.NewGuid());
        }
        #endregion
    }
}
