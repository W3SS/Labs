﻿using AutoMapper;
using Events.IO.Application.Interfaces;
using Events.IO.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Events.IO.Domain.Core.AppEvents;
using Events.IO.Domain.Core.Bus;
using Events.IO.Domain.Core.Notifications;
using Events.IO.Domain.Events.ApplicationEvents;
using Events.IO.Domain.Events.Commands;
using Events.IO.Domain.Events.Repository;
using Events.IO.Domain.Interfaces;
using Events.IO.Domain.Organizers.Commands;
using Events.IO.Domain.Organizers.Events;
using Events.IO.Domain.Organizers.Repository;
using Events.IO.Infra.CrossCutting.Bus;
using Events.IO.Infra.CrossCutting.Identity.Models;
using Events.IO.Infra.CrossCutting.Identity.Services;
using Events.IO.Infra.Data.Context;
using Events.IO.Infra.Data.Repository;
using Events.IO.Infra.Data.UoW;
using Microsoft.AspNetCore.Http;

namespace Events.IO.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            //Applications
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService)); //AutoMapper specific configuration
            services.AddScoped<IEventAppService, EventAppService>();
            services.AddScoped<IOrganizerAppService, OrganizerAppService>();

            //Domain - Commands
            services.AddScoped<IHandler<EventRegistrationCommand>, CommandEventHandler>();
            services.AddScoped<IHandler<EventUpdateCommand>, CommandEventHandler>();
            services.AddScoped<IHandler<EventDeleteCommand>, CommandEventHandler>();
            services.AddScoped<IHandler<AddAddressEventCommand>, CommandEventHandler>();
            services.AddScoped<IHandler<UpdateAddressEventCommand>, CommandEventHandler>();
            services.AddScoped<IHandler<OrganizerRegistrationCommand>, OrganizerCommandHandler>();

            //Domain - Events
            services.AddScoped<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IHandler<EventRegistrationEvent>, EventHandlerEvent>();
            services.AddScoped<IHandler<EventUpdateEvent>, EventHandlerEvent>();
            services.AddScoped<IHandler<EventDeleteEvent>, EventHandlerEvent>();
            services.AddScoped<IHandler<AddressEventAddedEvent>, EventHandlerEvent>();
            services.AddScoped<IHandler<AddressEventUpdatedEvent>, EventHandlerEvent>();
            services.AddScoped<IHandler<OrganizerRegisteredEvent>, OrganizerHandlerEvent>();

            //Infra - Data
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ContextEvents>();
            services.AddScoped<IOrganizerRepository, OrganizerRepository>();

            //Infra - Bus
            services.AddScoped<IBus, InMemoryBus>();

            //Infra - Identity
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}