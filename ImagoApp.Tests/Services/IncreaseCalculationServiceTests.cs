﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Services;
using Xunit;

namespace ImagoApp.Tests.Services
{
    public class IncreaseCalculationServiceTests
    {
        private readonly ICharacterCreationService _characterCreationService;

        public IncreaseCalculationServiceTests(ICharacterCreationService characterCreationService)
        {
            _characterCreationService = characterCreationService;
        }
        
    }
}