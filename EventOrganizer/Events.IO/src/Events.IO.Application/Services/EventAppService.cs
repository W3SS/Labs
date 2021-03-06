﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Events.IO.Application.Interfaces;
using Events.IO.Application.ViewModels;
using Events.IO.Domain.Core.Bus;
using Events.IO.Domain.Events.Commands;
using Events.IO.Domain.Events.Repository;
using Events.IO.Domain.Interfaces;

namespace Events.IO.Application.Services
{
    public class EventAppService : IEventAppService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;
        private readonly IUser _user;

        public EventAppService(IBus bus, IMapper mapper, IEventRepository eventRepository, IUser user)
        {
            _bus = bus;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _user = user;
        }

        public void Register(EventViewModel eventViewModel)
        {
            var registerCommand = _mapper.Map<EventRegistrationCommand>(eventViewModel);
            _bus.SendCommand(registerCommand);
        }

        public void Update(EventViewModel eventViewModel)
        {
            //TODO: Validate whether or not the organizer owns the event

            var updateEventCommand = _mapper.Map<EventUpdateCommand>(eventViewModel);
            _bus.SendCommand(updateEventCommand);
        }

        public void Delete(Guid id)
        {
            _bus.SendCommand(new EventDeleteCommand(id));
        }

        public void AddAddress(AddressViewModel addressViewModel)
        {
            var addressCommand = _mapper.Map<AddAddressEventCommand>(addressViewModel);
            _bus.SendCommand(addressCommand);
        }

        public void UpdateAddress(AddressViewModel addressViewModel)
        {
            var addressCommand = _mapper.Map<UpdateAddressEventCommand>(addressViewModel);
            _bus.SendCommand(addressCommand);
        }

        public AddressViewModel GetAddressById(Guid id)
        {
            return _mapper.Map<AddressViewModel>(_eventRepository.GetAddressById(id));
        }

        public IEnumerable<EventViewModel> GetAll()
        {
            return _mapper.Map<IEnumerable<EventViewModel>>(_eventRepository.GetAll());
        }

        public EventViewModel GetById(Guid id)
        {
            return _mapper.Map<EventViewModel>(_eventRepository.GetById(id));
        }

        public IEnumerable<EventViewModel> GetEventByOrganizer(Guid organizerId)
        {
            return _mapper.Map<IEnumerable<EventViewModel>>(_eventRepository.GetEventByOrganizer(organizerId));
        }

        public void Dispose()
        {
            _eventRepository.Dispose();
        }
    }
}
