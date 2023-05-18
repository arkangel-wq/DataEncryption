using Core.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEncryption.Core.Helpers
{
    public class ApiHelper : ObservableObject
    {
        [ThreadStatic]
        static ApiHelper instance;
        /// <summary>
        /// Obtiene la instancia de la clase.
        /// </summary>
        public static ApiHelper Instance => instance ?? (instance = new ApiHelper());

        /// <summary>
        /// Obtiene el radio.
        /// </summary>
        private string urlApisPublicas;
        public string UrlApisPublicas { get => (urlApisPublicas); set => Set(ref urlApisPublicas, value); }
    }
}
