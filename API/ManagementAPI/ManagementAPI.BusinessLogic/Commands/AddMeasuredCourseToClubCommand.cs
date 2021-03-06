﻿namespace ManagementAPI.BusinessLogic.Commands
{
    using Service.DataTransferObjects.Requests;
    using Shared.CommandHandling;
    using System;

    public class AddMeasuredCourseToClubCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the add measured course to club request.
        /// </summary>
        /// <value>
        /// The add measured course to club request.
        /// </value>
        public AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest { get; private set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGolfClubCommand" /> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <param name="commandId">The command identifier.</param>
        private AddMeasuredCourseToClubCommand(Guid golfClubId, Guid measuredCourseId, AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest, Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.MeasuredCourseId = measuredCourseId;
            this.AddMeasuredCourseToClubRequest = addMeasuredCourseToClubRequest;
        }
        #endregion

        #region public static AddMeasuredCourseToClubCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <returns></returns>
        public static AddMeasuredCourseToClubCommand Create(Guid golfClubId, Guid measuredCourseId, AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest)
        {
            return new AddMeasuredCourseToClubCommand(golfClubId, measuredCourseId, addMeasuredCourseToClubRequest, Guid.NewGuid());
        }
        #endregion
    }
}
