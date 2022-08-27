﻿using Application.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobRepository _blobRepository;//TODO: MediatR

        public BlobController(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetImageByName(string name)
        {
            return Ok((await _blobRepository.GetImageByName(name)).Content);

        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(string filePath, string name)
        {
            return Ok((await _blobRepository.UploadImage(filePath, name)).Content);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(string name)
        {
            return Ok((await _blobRepository.DeleteImage(name)));
        }
    }
}
