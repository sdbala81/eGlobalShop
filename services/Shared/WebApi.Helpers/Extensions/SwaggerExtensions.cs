﻿using Microsoft.AspNetCore.Builder;

namespace ElementLogiq.eGlobalShop.WebApi.Helpers.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
