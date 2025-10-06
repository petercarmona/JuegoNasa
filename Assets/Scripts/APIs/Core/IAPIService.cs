using System;
using UnityEngine;

namespace Farm.APIs.Core
{
    /// <summary>
    /// Interfaz base para todos los servicios de API
    /// Base interface for all API services
    /// </summary>
    public interface IAPIService
    {
        /// <summary>
        /// Realiza una petición GET a la API
        /// Performs a GET request to the API
        /// </summary>
        void FetchData(Action<string> onSuccess, Action<string> onError);
        
        /// <summary>
        /// Obtiene el nombre del servicio
        /// Gets the service name
        /// </summary>
        string GetServiceName();
    }
}