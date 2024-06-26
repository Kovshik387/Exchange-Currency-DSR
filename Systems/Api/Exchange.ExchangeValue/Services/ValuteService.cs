﻿using Grpc.Core;
using ExchangeVoluteService;
using Exchange.Services.ValutaRate.Infrastructure;
using System.Globalization;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using Exchange.Services.ValutaRate.Data.DTO;

namespace Exchange.ExchangeValute.Services;

public class ValuteService(IVoluteRateService voluteRateService, IMapper mapper, ILogger<ValuteService> logger) : Volute.VoluteBase
{
    private readonly IVoluteRateService _voluteRateService = voluteRateService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;
    public override async Task<DailyValuteResponse> GetCurrentValue(DailyValuteRequest request, ServerCallContext context)
    {
        if (DateOnly.TryParseExact(request.Date, "dd.MM.yyyy", CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateOnly parsedDate))
        {
            _logger.LogInformation($"Date: {request.Date}|\tParsed Date: {parsedDate}");
            return _mapper.Map<DailyValuteResponse>(await _voluteRateService.GetCursByDateAsync(parsedDate));
        }
        else return null;
    }

    public override async Task<DynamicValueResponse> GetDynamicValue(DynamicValueRequest request, ServerCallContext context)
    {
        DateOnly.TryParseExact(request.Date1, "dd.MM.yyyy", CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateOnly parsedDate1);
        DateOnly.TryParseExact(request.Date2, "dd.MM.yyyy", CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateOnly parsedDate2);
        
        var records = await _voluteRateService.GetCursListByDateAsync(parsedDate1, parsedDate2, request.Name);
        
        var response = new DynamicValueResponse();
        response.Record.AddRange(_mapper.Map<List<RecordResponse>>(records));

        if (response.Record.Count.Equals(0)) throw new RpcException(new Status(StatusCode.NotFound, "За заданную дату нет данных"));

        return response;
    }
}
