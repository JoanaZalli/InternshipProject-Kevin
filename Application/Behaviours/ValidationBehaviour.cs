using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using ValidationException = Application.Exceptions.ClientErrors.FluentException;

namespace Application.Validator
{
    public class ValidationBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {

        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IStringLocalizer<LocalizationResources> localizer) {
            _validators = validators;
            _localizer = localizer;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {

            // if no validation errors, continue
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            // return a HashMap of all the errors occurred
            var errorsDictionary = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => _localizer.GetString(x.ErrorMessage).Value,
                    (propertyName, errorMessages) => new {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);

            if (errorsDictionary.Any())
                throw new ValidationException(errorsDictionary);


            return await next();
        }
    }

}
