using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmountController : ControllerBase
    {
        private readonly IAmountRepository _amountRepository;
        private readonly IMapper _mapper;

        public AmountController(IAmountRepository amountRepository, IMapper mapper)
        {
            _amountRepository = amountRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AmountDTO>> GetAllAmounts()
        {
            var amounts = _amountRepository.GetAllAmounts();
            var amountDTOs = _mapper.Map<IEnumerable<AmountDTO>>(amounts);
            return Ok(amountDTOs);
        }
    }
}
