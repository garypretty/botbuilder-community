﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace Bot.Builder.Community.Cards.Translation
{
    /// <summary>
    /// Translation result from Translator API v3.
    /// </summary>
    internal class TranslatorResult
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}