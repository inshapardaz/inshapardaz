﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslationModel.cs" company="Inshapardaz">
//   Muhammad Umer Farooq
// </copyright>
// <summary>
//   Defines the TranslationModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Inshapardaz.Api.Model
{
    public class TranslationView
    {
        public long Id { get; set; }

        public string Language { get; set; }

        public int LanguageId { get; set; }

        public IEnumerable<LinkView> Links { get; set; }

        public string Value { get; set; }

        public long WordId { get; set; }
    }
}