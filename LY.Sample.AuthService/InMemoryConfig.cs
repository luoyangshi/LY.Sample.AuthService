﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace LY.Sample.AuthService
{
    public static class InMemoryConfig
    {
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("authservice","授权服务"),
                new ApiResource("orderservice","订单服务")
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static List<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "pwd",
                    ClientName = "密码模式",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "authservice",
                        "orderservice"
                    }
                },
                new Client()
                {
                    ClientId = "mvc",
                    ClientName = "授权模式",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = {"http://localhost:5003/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5003/signout-callback-oidc"},
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "authservice",
                        "orderservice"
                    }
                },
                new Client()
                {
                    ClientId = "code",
                    ClientName = "验证码",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = {"sms"},
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "authservice",
                        "orderservice"
                    }
                }
            };
        }

        public static List<TestUser> GeTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "001",
                    Username = "smallwall",
                    Password = "123123",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.PhoneNumber,"15100000000")
                    }
                }
            };
        }

    }
}
