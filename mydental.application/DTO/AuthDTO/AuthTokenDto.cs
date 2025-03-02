﻿namespace mydental.application.DTO.AuthDTO
{
    public class AuthTokenDto
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public AuthTokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
