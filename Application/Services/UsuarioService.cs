
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
public enum ForcaDaSenha
{
    Inaceitável,
    Fraca,
    Aceitável,
    Forte,
    Segura
}

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly BlogContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(BlogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AlterarSenhaAsync(int id, string senhaAtual, string novaSenha)
        {
            var user = await _context.Usuarios.Where(x => x.Id == id && x.Senha == EncryptionHelper.Encrypt(senhaAtual)).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Senha = EncryptionHelper.Encrypt(novaSenha);
                _context.Usuarios.Update(user);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateAsync(UsuarioDto user)
        {
            var forcaSenha = ForcaSenha.GetForcaDaSenha(user.Senha);

            if (forcaSenha == Helpers.ForcaDaSenha.Inaceitável)
            {
                return false;
            }

            var usuario = _mapper.Map<Usuario>(user);
            usuario.Role = "Funcionário";
            usuario.Senha = EncryptionHelper.Encrypt(user.Senha);
            await _context.AddAsync(_mapper.Map<Usuario>(usuario));
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (user == null) 
            {
                return false;
            }
            else
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Usuario> GetUserAsync(string email, string senha)
        {
            return await _context.Usuarios.AsNoTracking().Where(x => x.Email == email && x.Senha == EncryptionHelper.Encrypt(senha)).FirstOrDefaultAsync();
        }

        public async Task UltimoLoginAsync(Usuario user)
        {
            user.UltimoLogin = DateTime.Now;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario user)
        {
            _context.Update(user);
            _context.Entry(user).Property(p => p.Id).IsModified = false;
            _context.Entry(user).Property(p => p.UltimoLogin).IsModified = false;
            _context.Entry(user).Property(p => p.Role).IsModified = false;
            _context.Entry(user).Property(p => p.Senha).IsModified = false;                               
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsuarioCadastradoAsync(UsuarioDto user)
        {
            return await _context.Usuarios.AsNoTracking().Where(x => x.Email == user.Email).FirstOrDefaultAsync() != null ? true : false;
        }
    }
}
