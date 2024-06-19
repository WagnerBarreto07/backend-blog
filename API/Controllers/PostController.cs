using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/post")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _post;
        private readonly INotificacaoService _notificacao;

        public PostController(IPostService post, INotificacaoService notificacao)
        {
            _post = post;
            _notificacao = notificacao;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Post post)
        {
            if (ModelState.IsValid)
                await _post.CreateAsync(post);
            string retorno = await _notificacao.EnviaNotificacaoAsync();
            return Ok(retorno);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Post post)
        {
            if (post.Id > 0)
            {
                await _post.UpdateAsync(post);
                return Ok("Atualizado com sucesso");
            }
            else
            {
                return BadRequest("O Id informado é inválido!");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _post.DeleteAsync(id))
            {
                return Ok("Excluído com sucesso");
            }
            else
            {
                return BadRequest("Post não encontrado!");
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllPosts()
        {
            return Ok(await _post.GetAllPostsAsync());
        }
    }
}
