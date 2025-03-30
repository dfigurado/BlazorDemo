using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientLibrary.Services.Implementations;

public class GenericServiceImplementation<T>(HttpClientHelper httpClientHelper) : IGenericServiceInterface<T>
{
    public Task<List<T>> GetAll(string baseUrl)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetById(int id, string baseUrl)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResponse> Insert(T item, string baseUrl)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResponse> Update(T item, string baseUrl)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResponse> DeleteById(int id, string baseUrl)
    {
        throw new NotImplementedException();
    }
}