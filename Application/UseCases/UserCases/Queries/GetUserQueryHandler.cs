using Application.Exceptions.ClientErrors;
using Application.Persistance;
using Application.UseCases.UserCases.Results;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.UserCases.Queries {

    public class GetUserQuery : IRequest<UserResult> {
        public Guid Id { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResult> {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(IMapper mapper,
                                   IUserRepository userRepository,
                                   ILogger<GetUserQueryHandler> logger) {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserResult> Handle(GetUserQuery request, CancellationToken cancellationToken) {

            try {
                if (await _userRepository.ContainsAsync(request.Id) is false)
                    throw new NotFoundException("UserDoesntExist");

                var user = await _userRepository.GetByIdAsync(request.Id);
                return _mapper.Map<UserResult>(user);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get User Query Handler", request);

                throw;
            }
        }
    }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery> {
        public GetUserQueryValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("EmptyId");
        }
    }
}
