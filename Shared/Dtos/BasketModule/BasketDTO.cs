using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.BasketModule
{
    public record BasketDTO
    {
        public string Id { get; init; } = null!;

        public IEnumerable<BasketItemDTO> Items { get; init; } = [];
    }
}
