using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data
{
  public class MessageRepository : IMessageRepository
  {
    private readonly DataContext _context;
    public readonly IMapper _mapper;
    public MessageRepository(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
        
    }

    public void AddMessage(Message message)
    {
      _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
      _context.Messages.Remove(message);
    }

    public async Task<Message> GetMessage(int id)
    {
      return await _context.Messages.FindAsync(id);
    }

    public async Task<PageList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
    {
      var query = _context.Messages
        .OrderByDescending(x => x.MessageSent)
        .AsQueryable()
      ;

      query = messageParams.Container switch 
      {
         "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),
         "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),
         _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null)
      };

      var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);

      return await PageList<MessageDTO>.CreateAsync(
        messages,
        messageParams.PageNumber,
        messageParams.PageSize
      );
    }

    public Task<IEnumerable<MessageDTO>> GetMessageThread(int currentUserId, int IX_Messages_RecipientId)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }
  }
}
