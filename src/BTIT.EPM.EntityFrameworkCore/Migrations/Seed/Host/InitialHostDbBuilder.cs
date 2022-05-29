using BTIT.EPM.EntityFrameworkCore;

namespace BTIT.EPM.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly EPMDbContext _context;

        public InitialHostDbBuilder(EPMDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
