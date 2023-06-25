﻿global using Meetup.Infrastructure.Data;
global using Microsoft.EntityFrameworkCore;
global using MeetupAPI.Configuration;
global using Serilog;
global using Meetup.ApplicationCore.Interfaces.IRepository;
global using Meetup.Infrastructure.Repositories;
global using FluentValidation;
global using Meetup.ApplicationCore.DTO;
global using Meetup.ApplicationCore.Validation;
global using Microsoft.AspNetCore.Mvc;
global using Meetup.ApplicationCore.Interfaces.IService;
global using Meetup.ApplicationCore.Entities;
global using Microsoft.OpenApi.Models;
global using System.Reflection;
global using IdentityServer4.AccessTokenValidation;
global using Microsoft.AspNetCore.Authorization;
global using Meetup.ApplicationCore.Exceptions;
global using System.Net;
global using System.Text.Json;
global using Meetup.ApplicationCore.Interfaces.RabbitMQ;
global using Meetup.Infrastructure.RabbitMQ;
global using Meetup.ApplicationCore.Mapper;
global using Meetup.ApplicationCore.Services;
global using MeetupAPI.Interfaces;
global using MeetupAPI.Middlewares;