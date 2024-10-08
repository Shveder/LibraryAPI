﻿global using FluentAssertions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using Moq;
global using Library.Infrastructure.Repository;
global using Library.Core.Models;
global using Library.Tests.Services.Base;
global using AutoMapper;
global using System.Text;
global using System.Security.Claims;
global using Library.Infrastructure.DatabaseContext;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;
global using System.Security.Cryptography;
global using Library.Application.DTO;
global using Library.Application.Exceptions;
global using Library.Application.Mappings;
global using Library.Application.UseCases.AuthorizationUseCases;
global using Library.Application.UseCases.AuthorUseCases;
global using Library.Application.UseCases.BookUseCases;
global using Library.Application.UseCases.UserBookUseCases;
global using Library.Application.UseCases.UserUseCases;
global using Library.Application.Services;
