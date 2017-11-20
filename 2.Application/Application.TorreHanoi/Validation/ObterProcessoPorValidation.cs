using Application.TorreHanoi.Message;
using System;

namespace Application.TorreHanoi.Validation
{
    internal static class ObterProcessoPorValidation
    {
        internal static ObterProcessoPorResponse ValidationProcesso(this string id)
        {
            var response = new ObterProcessoPorResponse();
            var _result = new Guid();

            if (Guid.TryParse(id, out _result))
            {
                return response;
            }
            response.AdicionarMensagemDeErro($"É o id {id} não esta em um formato valido");
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            return response;
        }
    }
}
