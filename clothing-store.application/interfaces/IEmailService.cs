using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.interfaces
{
    public interface IEmailService
    {
        Task<bool> SendOrderConfirmationEmailAsync(OrderEmailRequest request);
    }
}
