﻿using CesarBmx.CryptoWatcher.Application.FakeResponses;
using CesarBmx.CryptoWatcher.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.CryptoWatcher.Api.ResponseExamples
{
    public class ScriptVariableListExample : IExamplesProvider<ScriptVariableSet>
    {
        public ScriptVariableSet GetExamples()
        {
            return FakeScriptVariableSet.GetFake_List();
        }
    }
}