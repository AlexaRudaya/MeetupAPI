﻿global using Microsoft.EntityFrameworkCore;
global using Meetup.ApplicationCore.Entities;
global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Meetup.Infrastructure.ModelConfiguration;
global using Meetup.ApplicationCore.Interfaces.IRepository;
global using Meetup.Infrastructure.Data;
global using Meetup.ApplicationCore.DTO;
global using Meetup.ApplicationCore.Interfaces.IService;
global using AutoMapper;
global using FluentValidation;
global using Meetup.ApplicationCore.Exceptions;
global using Microsoft.EntityFrameworkCore.Query;
global using System.Linq.Expressions;
global using Meetup.ApplicationCore.Interfaces.RabbitMQ;
global using Newtonsoft.Json;
global using RabbitMQ.Client;
global using System.Text;