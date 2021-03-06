﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    public class RegisterPlayerResponse
    {
        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }
    }
}
