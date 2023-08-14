﻿using Application.UseCases.LoanJourney.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class LoanMappings : Profile {
        public LoanMappings() {
            CreateMap<LoanResult, Loan>().ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LoanStatus.Name))
                .ReverseMap();

        }

    }
}