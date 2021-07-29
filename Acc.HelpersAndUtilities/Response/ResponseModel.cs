using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Acc.HelpersAndUtilities.Response
{
    public class ResponseModel : IResponseModel
    {
        public string Message { get; set; } = "";
        public bool DidError { get; set; }
        public string ErrorMessage { get; set; } = "";
    }


    public interface IResponseModel
    {
        string Message { get; set; }

        bool DidError { get; set; }
        string ErrorMessage { get; set; }
    }

    public interface ISingleResponseModel<TModel> : IResponseModel
    {
        TModel Model { get; set; }
    }

    public interface IListResponseModel<TModel> : IResponseModel
    {
        IEnumerable<TModel> Model { get; set; }
    }

    public interface IPagedResponseModel<TModel> : IListResponseModel<TModel>
    {
        int ItemsCount { get; set; }

        double PageCount { get; }
    }

    public class SingleResponseModel<TModel> : ISingleResponseModel<TModel>
    {
        public string Message { get; set; } = "";

        public bool DidError { get; set; }

        public string ErrorMessage { get; set; } = "";

        public TModel Model { get; set; }
    }

    public class ListResponseModel<TModel> : IListResponseModel<TModel>
    {
        public string Message { get; set; } = "";

        public bool DidError { get; set; }

        public string ErrorMessage { get; set; } = "";

        public IEnumerable<TModel> Model { get; set; }
    }

    public class PagedResponseModel<TModel> : IPagedResponseModel<TModel>
    {
        public string Message { get; set; } = "";

        public bool DidError { get; set; }

        public string ErrorMessage { get; set; } = "";

        public IEnumerable<TModel> Model { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int ItemsCount { get; set; }

        public double PageCount
        // => ((int)((double)ItemsCount / PageSize)) % 1 == 0 ? (int)((double)ItemsCount / PageSize) : (int)(((double)ItemsCount / PageSize) + 1);
        => ItemsCount < PageSize ? 1 : (int)(((double)ItemsCount / PageSize) + 1);
    }

    public static class ResponseExtensions
    {
        public static IActionResult ToHttpResponse(this IResponseModel response)
            => new ObjectResult(response)
            {
                StatusCode = (int)(response.DidError ? HttpStatusCode.InternalServerError : HttpStatusCode.OK)
            };

        public static IActionResult ToHttpResponse<TModel>(this ISingleResponseModel<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }
        public static IActionResult ToHttpResponse<TModel>(this IListResponseModel<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NoContent;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpCreatedResponse<TModel>(this IListResponseModel<TModel> response)
        {
            var status = HttpStatusCode.Created;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpCreatedResponse<TModel>(this ISingleResponseModel<TModel> response)
        {
            var status = HttpStatusCode.Created;

            if (response.DidError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

    }


    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }

    public class ValidationResultModel
    {
        public string Message { get; }
        public bool DidError { get; set; }
        public string ErrorMessage { get; set; }

        public List<ValidationError> Model { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Failed";
            DidError = true;
            ErrorMessage = "There was an model validation failed, please contact to developer.";
            Model = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
