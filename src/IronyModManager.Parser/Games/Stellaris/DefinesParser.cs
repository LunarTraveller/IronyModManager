﻿// ***********************************************************************
// Assembly         : IronyModManager.Parser
// Author           : Mario
// Created          : 02-21-2020
//
// Last Modified By : Mario
// Last Modified On : 02-25-2020
// ***********************************************************************
// <copyright file="DefinesParser.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using IronyModManager.Parser.Common.Args;
using IronyModManager.Parser.Common.Definitions;
using IronyModManager.Parser.Common.Parsers;

namespace IronyModManager.Parser.Games.Stellaris
{
    /// <summary>
    /// Class DefinesParser.
    /// Implements the <see cref="IronyModManager.Parser.Games.Stellaris.BaseStellarisParser" />
    /// </summary>
    /// <seealso cref="IronyModManager.Parser.Games.Stellaris.BaseStellarisParser" />
    public class DefinesParser : BaseStellarisParser
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinesParser" /> class.
        /// </summary>
        /// <param name="textParser">The text parser.</param>
        public DefinesParser(ITextParser textParser) : base(textParser)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Determines whether this instance can parse the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns><c>true</c> if this instance can parse the specified arguments; otherwise, <c>false</c>.</returns>
        public override bool CanParse(CanParseArgs args)
        {
            return IsStellaris(args) && args.File.StartsWith(Common.Constants.Stellaris.Defines, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Parses the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>IEnumerable&lt;IDefinition&gt;.</returns>
        public override IEnumerable<IDefinition> Parse(ParserArgs args)
        {
            var result = new List<IDefinition>();
            int? openBrackets = null;
            int closeBrackets = 0;
            string type = string.Empty;
            foreach (var line in args.Lines)
            {
                if (line.Trim().StartsWith(Common.Constants.Scripts.ScriptCommentId))
                {
                    continue;
                }
                if (!openBrackets.HasValue)
                {
                    var cleaned = textParser.CleanWhitespace(line);
                    if (cleaned.Contains(Common.Constants.Scripts.DefinitionSeparatorId) || cleaned.EndsWith(Common.Constants.Scripts.VariableSeparatorId))
                    {
                        type = textParser.GetKey(line, Common.Constants.Scripts.VariableSeparatorId);
                        openBrackets = line.Count(s => s == Common.Constants.Scripts.OpeningBracket);
                        closeBrackets = line.Count(s => s == Common.Constants.Scripts.ClosingBracket);
                        var content = textParser.PrettifyLine(cleaned.Replace($"{type}{Common.Constants.Scripts.DefinitionSeparatorId}", string.Empty).Replace($"{type}{Common.Constants.Scripts.VariableSeparatorId}", string.Empty).Trim());
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var key = textParser.GetKey(content, Common.Constants.Scripts.VariableSeparatorId);
                            var def = GetDefinitionInstance();
                            var parsingArgs = ConstructArgs(args, def);
                            MapDefinitionFromArgs(parsingArgs);
                            var definesType = textParser.PrettifyLine($"{type}{Common.Constants.Scripts.DefinitionSeparatorId}");
                            def.Code = $"{definesType}{Environment.NewLine}{content}{Environment.NewLine}{Common.Constants.Scripts.ClosingBracket}";
                            def.Type = FormatType(args.File, $"{type}-{Common.Constants.TxtType}");
                            def.Id = key;
                            def.ValueType = Common.ValueType.Variable;
                            result.Add(def);
                        }
                        if (openBrackets.GetValueOrDefault() > 0 && openBrackets == closeBrackets)
                        {
                            openBrackets = null;
                            closeBrackets = 0;
                        }
                    }
                }
                else
                {
                    if (line.Contains(Common.Constants.Scripts.OpeningBracket))
                    {
                        openBrackets += line.Count(s => s == Common.Constants.Scripts.OpeningBracket);
                    }
                    if (line.Contains(Common.Constants.Scripts.ClosingBracket))
                    {
                        closeBrackets += line.Count(s => s == Common.Constants.Scripts.ClosingBracket);
                    }
                    var cleaned = textParser.CleanWhitespace(line);
                    if (cleaned.EndsWith(Common.Constants.Scripts.ClosingBracket) && openBrackets.GetValueOrDefault() > 0 && openBrackets == closeBrackets)
                    {
                        cleaned = cleaned.Substring(0, cleaned.Length - 1);
                    }
                    if (!string.IsNullOrWhiteSpace(cleaned))
                    {
                        var definesType = textParser.PrettifyLine($"{type}{Common.Constants.Scripts.DefinitionSeparatorId}");
                        var key = textParser.GetKey(cleaned, Common.Constants.Scripts.VariableSeparatorId);
                        var def = GetDefinitionInstance();
                        var parsingArgs = ConstructArgs(args, def);
                        MapDefinitionFromArgs(parsingArgs);
                        def.Code = $"{definesType}{Environment.NewLine}{textParser.PrettifyLine(cleaned)}{Environment.NewLine}{Common.Constants.Scripts.ClosingBracket}";
                        def.Type = FormatType(args.File, $"{type}-{Common.Constants.TxtType}");
                        def.Id = key;
                        def.ValueType = Common.ValueType.Variable;
                        result.Add(def);
                    }
                    if (openBrackets.GetValueOrDefault() > 0 && openBrackets == closeBrackets)
                    {
                        openBrackets = null;
                        closeBrackets = 0;
                    }
                }
            }
            return result;
        }

        #endregion Methods
    }
}