﻿namespace AuthService.Services.Contracts
{
    public interface IHttpContextHelper
    {
        string GetTokenFromHeaders();
    }
}
