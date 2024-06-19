using Application.Helpers;
using Application.Interfaces;
using Domain.DTO;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[Route("api/usuario")]
[ApiController]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuario;
    private readonly IConfiguration _configuration;

    public UsuarioController(IUsuarioService usuario, IConfiguration configuration)
    {
        _usuario = usuario;
        _configuration = configuration;
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] UsuarioDto usuario)
    {
        var usuarioCadastrado = await _usuario.UsuarioCadastradoAsync(usuario);

        if (!usuarioCadastrado)
        {
            if (ModelState.IsValid)
                if (await _usuario.CreateAsync(usuario))
                    return Ok("Usuário inserido com sucesso!");
                else return BadRequest("A senha informada não atende os requisitos mínimos de segurança!");
            else return BadRequest("Dados inválidos!");
        }
        else
        {
            return Ok("O Usuário já possui cadastrado no sistema!");
        }
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]

    public async Task<ActionResult<dynamic>> Login([FromBody] LoginDto model)
    {
        var usuario = await _usuario.GetUserAsync(model.Email, model.Senha);

        Messages retorno = new Messages();
        retorno.Message = "Email ou senha inválidos";

        if (usuario == null) return BadRequest(retorno);

        TokenService tokenService = new(_configuration);

        var _token = tokenService.GenerateToken(usuario);

        await _usuario.UltimoLoginAsync(usuario);
        usuario.Senha = "";
        return new
        {
            user = usuario,
            token = _token,
            horarioExpiracaoToken = DateTime.Now.AddMinutes(30).ToString("HH:mm")
        };
    }    

    [HttpPut("update")]
    [Authorize]

    public async Task<IActionResult> Update([FromBody] Usuario model)
    {
        if (model.Id > 0)
        {
            await _usuario.UpdateAsync(model);
            return Ok("Atualizado com sucesso");
        }
        else
        {
            return BadRequest("O Id informado é inválido!");
        }
    }

    [HttpDelete("delete/{id}")]
    [Authorize]

    public async Task<IActionResult> Delete(int id)
    {
        if (await _usuario.DeleteAsync(id))
        {
            return Ok("Excluído com sucesso");
        }
        else
        {
            return BadRequest("Usuário não encontrado!");
        }
    }

    [HttpPost]
    [Route("usuario-cadastrado")]
    public async Task<bool> UsuarioCadastrado([FromBody] UsuarioDto model)
    {
        return await _usuario.UsuarioCadastradoAsync(model);
    }

    [HttpPost]
    [Route("alterar-senha")]
    [Authorize]

    public async Task<IActionResult> AlterarSenha(AlterarSenha senha)
    {
        if (ModelState.IsValid)
            return await _usuario.AlterarSenhaAsync(senha.Id, senha.SenhaAtual, senha.NovaSenha) == true ? Ok("Senha alterada com sucesso!") : BadRequest("Erro ao alterar a senha!");
        else
            return BadRequest("Erro ao alterar a senha!");
    }

    [HttpPost]
    [Route("forca-senha")]
    [AllowAnonymous]

    public IActionResult Senha([FromBody] string senha)
    {
        var retorno = ForcaSenha.GetForcaDaSenha(senha);

        return Ok(JsonConvert.SerializeObject(retorno.ToString()));
    }
}
