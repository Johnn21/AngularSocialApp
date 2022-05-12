using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(StoreContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IFriendshipRequestRepository FriendshipRequestRepository => new FriendshipRequestRepository(_context);
        public IFriendshipRepository FriendshipRepository => new FriendshipRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}