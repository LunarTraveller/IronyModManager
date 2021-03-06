﻿// ***********************************************************************
// Assembly         : IronyModManager.IO.Common
// Author           : Mario
// Created          : 06-19-2020
//
// Last Modified By : Mario
// Last Modified On : 12-07-2020
// ***********************************************************************
// <copyright file="ModMergeDefinitionExporterParameters.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using IronyModManager.Shared;
using IronyModManager.Shared.Models;

namespace IronyModManager.IO.Common.Mods
{
    /// <summary>
    /// Class ModMergeExporterParameters.
    /// </summary>
    [ExcludeFromCoverage("Parameters.")]
    public class ModMergeDefinitionExporterParameters
    {
        #region Properties

        /// <summary>
        /// Gets or sets the definitions.
        /// </summary>
        /// <value>The definitions.</value>
        public IEnumerable<IDefinition> Definitions { get; set; }

        /// <summary>
        /// Gets or sets the export path.
        /// </summary>
        /// <value>The export path.</value>
        public string ExportPath { get; set; }

        /// <summary>
        /// Gets or sets the game.
        /// </summary>
        /// <value>The game.</value>
        public string Game { get; set; }

        #endregion Properties
    }
}
