﻿// ***********************************************************************
// Assembly         : IronyModManager.Services
// Author           : Mario
// Created          : 02-12-2020
//
// Last Modified By : Mario
// Last Modified On : 06-07-2020
// ***********************************************************************
// <copyright file="Constants.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace IronyModManager.Services
{
    /// <summary>
    /// Class Constants.
    /// </summary>
    public static class Constants
    {
        #region Fields

        /// <summary>
        /// The paradox mod identifier
        /// </summary>
        public const string Paradox_mod_id = "pdx_";

        /// <summary>
        /// The paradox URL
        /// </summary>
        public const string Paradox_Url = "https://mods.paradoxplaza.com/mods/{0}/Any";

        /// <summary>
        /// The steam mod identifier
        /// </summary>
        public const string Steam_mod_id = "ugc_";

        /// <summary>
        /// The steam protocol URI
        /// </summary>
        public const string Steam_protocol_uri = "steam://openurl/{0}";

        /// <summary>
        /// The steam URL
        /// </summary>
        public const string Steam_Url = "https://steamcommunity.com/sharedfiles/filedetails/?id={0}";

        #endregion Fields

        #region Classes

        /// <summary>
        /// Class Themes.
        /// </summary>
        public static class Themes
        {
            #region Classes

            /// <summary>
            /// Class Dark.
            /// </summary>
            public static class Dark
            {
                #region Fields

                /// <summary>
                /// The accent resource
                /// </summary>
                public const string AccentResource = "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml";

                /// <summary>
                /// The main resource
                /// </summary>
                public const string MainResource = "avares://Avalonia.Themes.Default/DefaultTheme.xaml";

                /// <summary>
                /// The name
                /// </summary>
                public const string Name = "Dark";

                /// <summary>
                /// The override theme
                /// </summary>
                public const string OverrideTheme = "avares://IronyModManager/Controls/Themes/Dark/ThemeOverride.xaml";

                #endregion Fields
            }

            /// <summary>
            /// Class Light.
            /// </summary>
            public static class Light
            {
                #region Fields

                /// <summary>
                /// The accent resource
                /// </summary>
                public const string AccentResource = "avares://Avalonia.Themes.Default/Accents/BaseLight.xaml";

                /// <summary>
                /// The main resource
                /// </summary>
                public const string MainResource = "avares://Avalonia.Themes.Default/DefaultTheme.xaml";

                /// <summary>
                /// The name
                /// </summary>
                public const string Name = "Light";

                /// <summary>
                /// The override theme
                /// </summary>
                public const string OverrideTheme = "avares://IronyModManager/Controls/Themes/Light/ThemeOverride.xaml";

                #endregion Fields
            }

            /// <summary>
            /// Class MaterialDark.
            /// </summary>
            public static class MaterialDark
            {
                #region Fields

                /// <summary>
                /// The accent resource
                /// </summary>
                public const string AccentResource = "avares://Material.Avalonia/Material.Avalonia.Dark.xaml";

                /// <summary>
                /// The main resource
                /// </summary>
                public const string MainResource = "avares://Material.Avalonia/Material.Avalonia.Templates.xaml";

                /// <summary>
                /// The name
                /// </summary>
                public const string Name = "MaterialDark";

                /// <summary>
                /// The override theme
                /// </summary>
                public const string OverrideTheme = "avares://IronyModManager/Controls/Themes/MaterialDark/ThemeOverride.xaml";

                #endregion Fields
            }

            /// <summary>
            /// Class MaterialDeepPurple.
            /// </summary>
            public static class MaterialDeepPurple
            {
                #region Fields

                /// <summary>
                /// The accent resource
                /// </summary>
                public const string AccentResource = "avares://Material.Avalonia/Material.Avalonia.DeepPurple.xaml";

                /// <summary>
                /// The main resource
                /// </summary>
                public const string MainResource = "avares://Material.Avalonia/Material.Avalonia.Templates.xaml";

                /// <summary>
                /// The name
                /// </summary>
                public const string Name = "MaterialDeepPurple";

                /// <summary>
                /// The override theme
                /// </summary>
                public const string OverrideTheme = "avares://IronyModManager/Controls/Themes/MaterialDeepPurple/ThemeOverride.xaml";

                #endregion Fields
            }

            /// <summary>
            /// Class MaterialLightGreen.
            /// </summary>
            public static class MaterialLightGreen
            {
                #region Fields

                /// <summary>
                /// The accent resource
                /// </summary>
                public const string AccentResource = "avares://Material.Avalonia/Material.Avalonia.LightGreen.xaml";

                /// <summary>
                /// The main resource
                /// </summary>
                public const string MainResource = "avares://Material.Avalonia/Material.Avalonia.Templates.xaml";

                /// <summary>
                /// The name
                /// </summary>
                public const string Name = "MaterialLightGreen";

                /// <summary>
                /// The override theme
                /// </summary>
                public const string OverrideTheme = "avares://IronyModManager/Controls/Themes/MaterialLightGreen/ThemeOverride.xaml";

                #endregion Fields
            }

            #endregion Classes
        }

        #endregion Classes
    }
}
