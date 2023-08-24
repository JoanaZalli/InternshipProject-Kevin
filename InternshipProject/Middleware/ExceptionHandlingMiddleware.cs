using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using InternshipProject.Localizations;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net;

namespace InternshipProject.Middleware {
    public class ExceptionHandlingMiddleware {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IStringLocalizer<LocalizationResources> localization) {
            _next = next;
            _logger = logger;
            _localization = localization;
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            }
            catch (BlockedException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (ConflictException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (FluentException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (ForbiddenException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (InvalidRequestException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (NotFoundException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (UnauthorizedException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (DatabaseException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (ServerException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (ThirdPartyException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (HttpRequestException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (AutoMapper.AutoMapperMappingException ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex) {

            // Handle Exception by Defining the status code and the errror message.

            string? result;

            switch (ex) {
                case BlockedException blocked:
                    context.Response.StatusCode = BlockedException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/429",
                        title = "Too Many Requests",
                        status = BlockedException.status,
                        detail = blocked.Message
                    });
                    return context.Response.WriteAsync(result);

                case ConflictException conflict:
                    context.Response.StatusCode = ConflictException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/409",
                        title = "Conflict",
                        status = ConflictException.status,
                        detail = conflict.Message
                    });
                    return context.Response.WriteAsync(result);

                case ForbiddenException fobidden:
                    context.Response.StatusCode = ForbiddenException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/403",
                        title = "Forbidden",
                        status = ForbiddenException.status,
                        detail = fobidden.Message
                    });
                    return context.Response.WriteAsync(result);

                case InvalidRequestException invalidRequest:
                    context.Response.StatusCode = InvalidRequestException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/400",
                        title = "Invalid Request",
                        status = ForbiddenException.status,
                        detail = invalidRequest.Message
                    });
                    return context.Response.WriteAsync(result);

                case NotFoundException notFound:
                    context.Response.StatusCode = NotFoundException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/404",
                        title = "Not Found",
                        status = NotFoundException.status,
                        detail = notFound.Message
                    });
                    return context.Response.WriteAsync(result);

                case UnauthorizedException unauthorized:
                    context.Response.StatusCode = UnauthorizedException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/401",
                        title = "Unauthorized",
                        status = UnauthorizedException.status,
                        detail = unauthorized.Message
                    });
                    return context.Response.WriteAsync(result);

                case FluentException:
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/422",
                        title = "Unprocessable Entity",
                        status = (int)HttpStatusCode.UnprocessableEntity,
                        details = GetErrors(ex)
                    });
                    return context.Response.WriteAsync(result);

                case DatabaseException database:
                    context.Response.StatusCode = DatabaseException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/503",
                        title = "Database Exception",
                        status = DatabaseException.status,
                        detail = database.Message
                    });
                    return context.Response.WriteAsync(result);

                case ServerException server:
                    context.Response.StatusCode = ServerException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/500",
                        title = "Server Exception",
                        status = ServerException.status,
                        detail = server.Message
                    });
                    return context.Response.WriteAsync(result);

                case ThirdPartyException thirdParty:
                    context.Response.StatusCode = ThirdPartyException.status;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/502",
                        title = "Third Party Exception",
                        status = ThirdPartyException.status,
                        detail = thirdParty.Message
                    });
                    return context.Response.WriteAsync(result);

                case AutoMapper.AutoMapperMappingException automapperException:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/500",
                        title = "Internal Server Error",
                        status = (int)HttpStatusCode.InternalServerError,
                        detail = automapperException.Message
                    });
                    return context.Response.WriteAsync(result);

                case HttpRequestException httpRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/503",
                        title = "Service Unavailable",
                        status = (int)HttpStatusCode.ServiceUnavailable,
                        detail = httpRequestException.Message
                    });
                    return context.Response.WriteAsync(result);

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/problem+json";
                    result = JsonConvert.SerializeObject(new {
                        type = "https://httpstatuses.io/500",
                        title = "Internal Server Error",
                        status = (int)HttpStatusCode.InternalServerError,
                        detail = _localization.GetString("InternalError").Value
                    });
                    return context.Response.WriteAsync(result);
            }
        }

        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception) {
            IReadOnlyDictionary<string, string[]>? errors = null;

            if (exception is FluentException validationException)
                errors = validationException.ErrorsDictionary;

            return errors;
        }
    }
}
