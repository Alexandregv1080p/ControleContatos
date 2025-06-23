<<<<<<< HEAD
﻿using ControleContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleContatos.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        { 
            
        }
        public DbSet<ContatoModel> Contatos { get; set; }
=======
﻿namespace ControleContatos.Data
{
    public class BancoContext
    {
>>>>>>> 0fecd126d7d3dede56a6b7e02d294da4fd6f42a2
    }
}
