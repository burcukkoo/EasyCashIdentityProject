using EasyCashIdentityProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCashIdentityProject.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext <AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-H52HVCE; initial catalog=EasyCashDB; integrated Security=true");
        }

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<CustomerAccountProcess> CustomerAccountProcesses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            // CustomerAccountProcess için yapılandırma başlatılıyor
            builder.Entity<CustomerAccountProcess>()
                // Her CustomerAccountProcess'ın bir borçlandıran (debiting) müşterisi olmalıdır.
                // Bu, işlemi başlatan veya "borçlandıran" müşteriyi temsil eder.
                .HasOne(x => x.SenderCustomer)
                // Bir DebitingCustomer, birden fazla CustomerAccountProcess'i başlatabilir.
                // Yani bir müşteri birden fazla işlem başlatmış olabilir.
                .WithMany(y => y.CustomerSender)
                // DebitorID, DebitingCustomer ile CustomerAccountProcess'i ilişkilendiren yabancı anahtar görevi görür.
                // Bu, hangi işlemin hangi müşteri tarafından başlatıldığını belirlememizi sağlar.
                .HasForeignKey(x => x.SenderID)
                // Eğer bir DebitingCustomer silinirse, ona bağlı tüm CustomerAccountProcess'lerin DebitorID'si null'a ayarlanır.
                // Bu, müşterinin silinmesi durumunda veri bütünlüğünü korur.
                // İşlem geçmişi kaybolmaz, sadece hangi müşterinin bu işlemi başlattığının bilgisi silinir.
                .OnDelete(DeleteBehavior.ClientSetNull);

            // CustomerAccountProcess için yapılandırma devam ediyor
            builder.Entity<CustomerAccountProcess>()
                // Her CustomerAccountProcess'in bir alacaklandıran (crediting) müşterisi olmalıdır.
                // Bu, işlemin sonucunda "alacaklandırılan" müşteriyi temsil eder.
                .HasOne(x => x.ReceiverCustomer)
                // Bir CreditingCustomer, birden fazla CustomerAccountProcess'i alabilir.
                // Yani bir müşteri birden fazla işlem alabilir.
                .WithMany(y => y.CustomerReceiver)
                // CreditorID, CreditingCustomer ile CustomerAccountProcess'i ilişkilendiren yabancı anahtar görevi görür.
                // Bu, hangi işlemin hangi müşteriye gittiğini belirlememizi sağlar.
                .HasForeignKey(z => z.ReceiverID)
                // Eğer bir CreditingCustomer silinirse, ona bağlı tüm CustomerAccountProcess'lerin CreditorID'si null'a ayarlanır.
                // Bu, müşterinin silinmesi durumunda veri bütünlüğünü korur.
                // İşlem geçmişi kaybolmaz, sadece hangi müşterinin bu işlemi aldığının bilgisi silinir.
                .OnDelete(DeleteBehavior.ClientSetNull);

        }

    }
}
