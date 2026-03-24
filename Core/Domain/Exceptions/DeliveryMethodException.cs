using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodException(int id) : NotFoundException($"No Delivery Method with Id: {id} was Found")
    {
    }
}
