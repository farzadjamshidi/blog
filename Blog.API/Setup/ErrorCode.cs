namespace Blog.API.Setup;

internal enum ErrorCode
{
    NO_ERROR = 0,
    GENERIC_ERROR = 1,
    DB_ERROR = 5,
    DEPENDENCY_ERROR = 7,
    VALIDATION_ERROR = 8,
    OK = 200,
    BAD_REQUEST = 400,
    UNAUTHORIZED = 401,
    NOT_FOUND = 404,
    INTERNAL_SERVER_ERROR = 500
}