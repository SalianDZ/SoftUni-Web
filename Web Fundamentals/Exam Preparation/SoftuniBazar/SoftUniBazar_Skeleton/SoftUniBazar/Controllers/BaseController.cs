﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SoftUniBazar.Controllers
{
	[Authorize]
	public class BaseController : Controller
	{
		protected string GetUserId()
		{
			string id = String.Empty;
			if (User != null)
			{
				id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			}
			return id;
		}
	}
}
