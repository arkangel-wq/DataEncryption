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

        private string urlApisPublicas;
        public string UrlApisPublicas { get => (urlApisPublicas); set => Set(ref urlApisPublicas, value); }

        private string urlVM;
        public string UrlVM { get => (urlVM); set => Set(ref urlVM, value); }
    }
}
