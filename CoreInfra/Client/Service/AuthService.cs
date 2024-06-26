﻿using Blazored.LocalStorage;
using CoreInfra.Shared.DTOs;
using CoreInfra.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using CoreInfra.Client.Utility;

namespace CoreInfra.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }


    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);
        var response = await _httpClient.PostAsync("api/Auth/Login", 
            new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
        var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (!response.IsSuccessStatusCode)
        {
            return loginResult!;
        }

        await _localStorage.SetItemAsync("authToken", loginResult!.Token);
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email!);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<RegisterResult> Register(RegisterModel registerModel)
    {
        var result = await _httpClient.PostAsJsonAsync("api/Auth/Register", registerModel);
        if (!result.IsSuccessStatusCode)
            return new RegisterResult { Successful = false, Errors = new List<string> { "Error occured" } };
        return new RegisterResult { Successful = true, Errors = null };
    }
}
