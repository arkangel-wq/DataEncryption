using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MVVM
{
    public class ViewModelBase : ObservableObject
    {
        //TODO: public bool IsInDesignMode { get => false; }

        private bool loading;
        /// <summary>
        /// Indicates that is loading or getting data
        /// </summary>
        public bool Loading { get => loading; set => Set(ref loading, value); }

        private bool processing;
        /// <summary>
        /// Indicates that is processing
        /// </summary>
        public bool Processing { get => processing; set => Set(ref processing, value); }
    }
}
