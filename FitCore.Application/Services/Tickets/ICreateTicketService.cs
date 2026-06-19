using FitCore.Application.Services.Tickets.DTOs;
using FitCore.Common.Dto;

using System.Collections.Generic;

namespace FitCore.Application.Services.Tickets.Interfaces
{
    public interface ICreateTicketService
    {
        ResultDto<long> Execute(CreateTicketDto request, long senderUserId);
    }

    public interface IReplyTicketService
    {
        ResultDto Execute(ReplyTicketDto request, long replierUserId);
    }

    public interface IGetTicketDetailService
    {
        ResultDto<TicketDetailDto> Execute(long ticketId, long? currentGymId);
    }

    public interface IGetGymTicketsService
    {
        ResultDto<List<TicketDto>> Execute(long gymId);
    }

    public interface IGetAllTicketsService
    {
        ResultDto<List<TicketDto>> Execute();
    }
}