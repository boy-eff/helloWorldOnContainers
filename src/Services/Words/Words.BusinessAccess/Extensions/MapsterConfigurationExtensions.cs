﻿using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Models;

namespace Words.BusinessAccess.Extensions;

public static class MapsterConfigurationExtensions
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<WordCollectionTest, WordCollectionTestQuestionDto>
            .NewConfig()
            .Map(dest => dest.Word, src => src.Word.Value)
            .Map(dest => dest.AnswerOptions, src => src.AnswerOptions.Select(x => x.Value));
    }
}