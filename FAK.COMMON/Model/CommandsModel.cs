using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.Common.Model
{
    public class CommandsModel<TRequest, TResponse> : IRequest<TResponse> where TRequest : class
       where TResponse : class
    {
        public UserIdentityModel UserIdentity { get; set; }

        public TRequest CommandModel { get; set; }

        public DateTime CurrentDateTime { get; set; }
    }
}
