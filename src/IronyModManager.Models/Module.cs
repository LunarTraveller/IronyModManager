﻿// ***********************************************************************
// Assembly         : IronyModManager.Models
// Author           : Mario
// Created          : 01-17-2020
//
// Last Modified By : Mario
// Last Modified On : 02-06-2020
// ***********************************************************************
// <copyright file="Module.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using IronyModManager.Shared;

namespace IronyModManager.Models
{
    /// <summary>
    /// Class Module.
    /// Implements the <see cref="IronyModManager.Shared.IModule" />
    /// </summary>
    /// <seealso cref="IronyModManager.Shared.IModule" />
    [ExcludeFromCoverage("Module info should not be tested.")]
    public class Module : IModule
    {
        #region Properties

        /// <summary>
        /// Gets the dependencies.
        /// </summary>
        /// <value>The dependencies.</value>
        public IEnumerable<string> Dependencies => new List<string> { typeof(Common.Module).Name,
            typeof(DI.Module).Name, typeof(Shared.Module).Name, typeof(Localization.Module).Name };

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => typeof(Module).Namespace;

        #endregion Properties
    }
}
