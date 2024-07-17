using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Exceptions
{
    public class FormError
    {
        public FormError()
        {

        }
        public FormError(string Property, string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
            this.Property = Property;
        }
        public string Property { get; set; }
        public string ErrorMessage { get; set; }
    }
}