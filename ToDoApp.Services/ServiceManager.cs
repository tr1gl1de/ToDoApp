using AutoMapper;
using ToDoApp.Domain.Repositories;
using ToDoApp.Services.Abstraction;

namespace ToDoApp.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<INoteService> _lazyNoteService;
    private readonly Lazy<IUserService> _lazyUserService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _lazyNoteService = new Lazy<INoteService>(() => new NoteService(repositoryManager, mapper));
        _lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper));
    }

    public IUserService UserService => _lazyUserService.Value;
    public INoteService NoteService => _lazyNoteService.Value;
}