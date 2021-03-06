﻿namespace ManagementAPI.BusinessLogic.CommandHandlers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using ManagementAPI.GolfClub;
    using ManagementAPI.HandicapCalculationProcess;
    using ManagementAPI.Player;
    using ManagementAPI.Tournament;
    using Services;
    using Services.ApplicationServices;
    using Services.ExternalServices;
    using Shared.CommandHandling;
    using Shared.EventStore;

    [ExcludeFromCodeCoverage]
    public class CommandRouter : ICommandRouter
    {
        #region Fields

        /// <summary>
        /// The club aggregate repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> ClubRepository;

        /// <summary>
        /// The golf club membership application service
        /// </summary>
        private readonly IGolfClubMembershipApplicationService GolfClubMembershipApplicationService;

        /// <summary>
        /// The handicap adjustment calculator service
        /// </summary>
        private readonly IHandicapAdjustmentCalculatorService HandicapAdjustmentCalculatorService;

        private readonly IAggregateRepository<HandicapCalculationProcessAggregate> HandicapCalculationProcessRepository;

        /// <summary>
        /// The o auth2 security service
        /// </summary>
        private readonly ISecurityService OAuth2SecurityService;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The tournament application service
        /// </summary>
        private readonly ITournamentApplicationService TournamentApplicationService;

        /// <summary>
        /// The tournament repository
        /// </summary>
        private readonly IAggregateRepository<TournamentAggregate> TournamentRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRouter" /> class.
        /// </summary>
        /// <param name="clubRepository">The club repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        /// <param name="handicapAdjustmentCalculatorService">The handicap adjustment calculator service.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        /// <param name="golfClubMembershipApplicationService">The golf club membership application service.</param>
        /// <param name="tournamentApplicationService">The tournament application service.</param>
        /// <param name="handicapCalculationProcessRepository">The handicap calculation process repository.</param>
        public CommandRouter(IAggregateRepository<GolfClubAggregate> clubRepository,
                             IAggregateRepository<TournamentAggregate> tournamentRepository,
                             IHandicapAdjustmentCalculatorService handicapAdjustmentCalculatorService,
                             IAggregateRepository<PlayerAggregate> playerRepository,
                             ISecurityService oAuth2SecurityService,
                             IGolfClubMembershipApplicationService golfClubMembershipApplicationService,
                             ITournamentApplicationService tournamentApplicationService,
                             IAggregateRepository<HandicapCalculationProcessAggregate> handicapCalculationProcessRepository)
        {
            this.HandicapCalculationProcessRepository = handicapCalculationProcessRepository;
            this.ClubRepository = clubRepository;
            this.TournamentRepository = tournamentRepository;
            this.HandicapAdjustmentCalculatorService = handicapAdjustmentCalculatorService;
            this.PlayerRepository = playerRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
            this.GolfClubMembershipApplicationService = golfClubMembershipApplicationService;
            this.TournamentApplicationService = tournamentApplicationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Routes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task Route(ICommand command,
                                CancellationToken cancellationToken)
        {
            ICommandHandler commandHandler = CreateHandler((dynamic)command);

            await commandHandler.Handle(command, cancellationToken);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(StartHandicapCalculationProcessForTournamentCommand command)
        {
            return new HandicapCalculationCommandHandler(this.HandicapCalculationProcessRepository, this.TournamentRepository);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateGolfClubCommand command)
        {
            return new GolfClubCommandHandler(this.ClubRepository, this.OAuth2SecurityService, this.GolfClubMembershipApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(AddMeasuredCourseToClubCommand command)
        {
            return new GolfClubCommandHandler(this.ClubRepository, this.OAuth2SecurityService, this.GolfClubMembershipApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubRepository, this.TournamentRepository, this.TournamentApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(SignUpForTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubRepository, this.TournamentRepository, this.TournamentApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(RecordPlayerTournamentScoreCommand command)
        {
            return new TournamentCommandHandler(this.ClubRepository, this.TournamentRepository, this.TournamentApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CompleteTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubRepository, this.TournamentRepository, this.TournamentApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CancelTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubRepository, this.TournamentRepository, this.TournamentApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(ProduceTournamentResultCommand command)
        {
            return new TournamentCommandHandler(this.ClubRepository, this.TournamentRepository, this.TournamentApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(RegisterPlayerCommand command)
        {
            return new PlayerCommandHandler(this.PlayerRepository, this.OAuth2SecurityService, this.ClubRepository);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(RequestClubMembershipCommand command)
        {
            return new GolfClubCommandHandler(this.ClubRepository, this.OAuth2SecurityService, this.GolfClubMembershipApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(AddTournamentDivisionToGolfClubCommand command)
        {
            return new GolfClubCommandHandler(this.ClubRepository, this.OAuth2SecurityService, this.GolfClubMembershipApplicationService);
        }

        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateMatchSecretaryCommand command)
        {
            return new GolfClubCommandHandler(this.ClubRepository, this.OAuth2SecurityService, this.GolfClubMembershipApplicationService);
        }

        #endregion
    }
}