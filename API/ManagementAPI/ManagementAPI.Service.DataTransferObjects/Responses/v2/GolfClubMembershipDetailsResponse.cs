﻿namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class GolfClubMembershipDetailsResponse
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the player.
        /// </summary>
        /// <value>
        /// The full name of the player.
        /// </value>
        public String PlayerFullName { get; set; }

        /// <summary>
        /// Gets or sets the player date of birth.
        /// </summary>
        /// <value>
        /// The player date of birth.
        /// </value>
        public String PlayerDateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the player gender.
        /// </summary>
        /// <value>
        /// The player gender.
        /// </value>
        public String PlayerGender { get; set; }

        /// <summary>
        /// Gets or sets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        public String MembershipNumber { get; set; }

        /// <summary>
        /// Gets or sets the membership status.
        /// </summary>
        /// <value>
        /// The membership status.
        /// </value>
        public MembershipStatus MembershipStatus { get; set; }
    }
}
